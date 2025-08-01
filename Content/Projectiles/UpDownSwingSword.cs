using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace DestroyerTest.Content.Projectiles
{
    internal class UpDownSwingSword : ModProjectile
    {
    private const float SWINGRANGE = 1.25f * (float)Math.PI;
    private const float FIRSTHALFSWING = 0.45f; // How much of the swing happens before it reaches the target angle (in relation to swingRange)
    private const float WINDUP = 0.25f; // How far back the player's hand goes when winding their attack (in relation to swingRange)
    private const float UNWIND = 0.4f; // When should the sword start disappearing
    private enum AttackType // Which attack is being performed
        {
        // Swings are normal sword swings that can be slightly aimed
        // Swings goes through the full cycle of animations
        Swingdown,
        Swingup,
        }
    private enum AttackStage // What stage of the attack is being executed, see functions found in AI for description
        {
        Prepare,
        Execute,
        Unwind
        }
    private AttackType CurrentAttack
        {
        get => (AttackType)Projectile.ai[0];
        set => Projectile.ai[0] = (float)value;
        }
    private AttackStage CurrentStage
        {
        get => (AttackStage)Projectile.localAI[0];
        set
        {
        Projectile.localAI[0] = (float)value;
        Timer = 0; // reset the timer when the projectile switches states
        }
        }
    private ref float InitialAngle => ref Projectile.ai[1]; // Angle aimed in (with constraints)
    private ref float Timer => ref Projectile.ai[2]; // Timer to keep track of progression of each stage
    private ref float Progress => ref Projectile.localAI[1]; // Position of sword relative to initial angle
    private ref float Size => ref Projectile.localAI[2]; // Size of sword

    // We define timing functions for each stage, taking into account melee attack speed
    // Note that you can change this to suit the need of your projectile
    private float prepTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
    private float execTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
    private float hideTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
    private Player Owner => Main.player[Projectile.owner];
    public override void SetStaticDefaults()
        {
        ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 68; // Hitbox width of projectile
            Projectile.height = 68; // Hitbox height of projectile
            Projectile.friendly = true; // Projectile hits enemies
            Projectile.timeLeft = 10000; // Time it takes for projectile to expire
            Projectile.penetrate = -1; // Projectile pierces infinitely
            Projectile.tileCollide = false; // Projectile does not collide with tiles
            Projectile.usesLocalNPCImmunity = true; // Uses local immunity frames
            Projectile.localNPCHitCooldown = -1; // We set this to -1 to make sure the projectile doesn't hit twice
            Projectile.ownerHitCheck = true; // Make sure the owner of the projectile has line of sight to the target (aka can't hit things through tile).
            Projectile.DamageType = DamageClass.Melee; // Projectile is a melee projectile
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

    public override string Texture => "DestroyerTest/Content/Projectiles/UpDownSwingSword";
    public override void OnSpawn(IEntitySource source)
        {
        Projectile.spriteDirection = Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;
        float targetAngle = (Main.MouseWorld - Owner.MountedCenter).ToRotation();
        if (Projectile.spriteDirection == 1)
            {
            // However, we limit the rangle of possible directions so it does not look too ridiculous
            targetAngle = MathHelper.Clamp(targetAngle, (float)-Math.PI * 1 / 3, (float)Math.PI * 1 / 6);
            }
            else
                {
                if (targetAngle < 0)
                    {
                    targetAngle += 2 * (float)Math.PI; // This makes the range continuous for easier operations
                    }

                targetAngle = MathHelper.Clamp(targetAngle, (float)Math.PI * 5 / 6, (float)Math.PI * 4 / 3);
                }
            if (CurrentAttack == AttackType.Swingdown)
                {
                InitialAngle = targetAngle - FIRSTHALFSWING * SWINGRANGE * Projectile.spriteDirection;
                }
                else if (CurrentAttack == AttackType.Swingup)
                    {
                        InitialAngle = targetAngle + FIRSTHALFSWING * SWINGRANGE * Projectile.spriteDirection;
                    }
        }
    public override void SendExtraAI(BinaryWriter writer)
            {
            // Projectile.spriteDirection for this projectile is derived from the mouse position of the owner in OnSpawn, as such it needs to be synced. spriteDirection is not one of the fields automatically synced over the network. All Projectile.ai slots are used already, so we will sync it manually.
            writer.Write((sbyte)Projectile.spriteDirection);
            }
        public override void ReceiveExtraAI(BinaryReader reader)
            {
            Projectile.spriteDirection = reader.ReadSByte();
            }
    public override void AI()
        {
        // Extend use animation until projectile is killed
        Owner.itemAnimation = 2;
        Owner.itemTime = 2;

        // Kill the projectile if the player dies or gets crowd controlled
        if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
            {
            Projectile.Kill();
            return;
            }

        // AI depends on stage and attack
        // Note that these stages are to facilitate the scaling effect at the beginning and end
        // If this is not desireable for you, feel free to simplify
        switch (CurrentStage)
            {
            case AttackStage.Prepare:
            PrepareStrike();
            break;
            case AttackStage.Execute:
            ExecuteStrike();
            break;
            default:
            UnwindStrike();
            break;
            }

        SetSwordPosition();
        Timer++;
        }
    public override bool PreDraw(ref Color lightColor)
    {
    // Calculate origin of sword (hilt) based on orientation and offset sword rotation (as sword is angled in its sprite)
    Vector2 origin;
    float rotationOffset;
    SpriteEffects effects;

    if (Projectile.spriteDirection > 0)
        {
        origin = new Vector2(0, Projectile.height);
        rotationOffset = MathHelper.ToRadians(45f);
        effects = SpriteEffects.None;
        }
    else
        {
        origin = new Vector2(Projectile.width, Projectile.height);
        rotationOffset = MathHelper.ToRadians(135f);
        effects = SpriteEffects.FlipHorizontally;
        }

    Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, default, lightColor * Projectile.Opacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);

    // Since we are doing a custom draw, prevent it from normally drawing
    return false;
    }
    // Find the start and end of the sword and use a line collider to check for collision with enemies
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
        Vector2 start = Owner.MountedCenter;
        Vector2 end = start + Projectile.rotation.ToRotationVector2() * ((Projectile.Size.Length()) * Projectile.scale);
        float collisionPoint = 0f;
        return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 15f * Projectile.scale, ref collisionPoint);
        }
    // Do a similar collision check for tiles
    public override void CutTiles()
        {
        Vector2 start = Owner.MountedCenter;
        Vector2 end = start + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() * Projectile.scale);
        Utils.PlotTileLine(start, end, 15 * Projectile.scale, DelegateMethods.CutTiles);
        }
    public override bool? CanDamage()
        {
        if (CurrentStage == AttackStage.Prepare)
        return false;
        return base.CanDamage();
        }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        modifiers.HitDirectionOverride= target.position.X > Owner.MountedCenter.X ? 1 : -1;
        }
    public void SetSwordPosition()
        {
        Projectile.rotation = InitialAngle + Projectile.spriteDirection * Progress; // Set projectile rotation

        // Set composite arm allows you to set the rotation of the arm and stretch of the front and back arms independently
        Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f)); // set arm position (90 degree offset since arm starts lowered)
        Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2); // get position of hand

        armPosition.Y += Owner.gfxOffY;
        Projectile.Center = armPosition; // Set projectile to arm position
        Projectile.scale = Size * 1.2f * Owner.GetAdjustedItemScale(Owner.HeldItem); // Slightly scale up the projectile and also take into account melee size modifiers

        Owner.heldProj = Projectile.whoAmI; // set held projectile to this projectile
        }
    // Function facilitating the taking out of the sword
    private void PrepareStrike()
        {
        Progress = WINDUP * SWINGRANGE * (1f - Timer / prepTime); // Calculates rotation from initial angle
        Size = MathHelper.SmoothStep(0, 1, Timer / prepTime); // Make sword slowly increase in size as we prepare to strike until it reaches max

        if (Timer >= prepTime)
            {
            SoundEngine.PlaySound(SoundID.Item1); // Play sword sound here since playing it on spawn is too early
            CurrentStage = AttackStage.Execute; // If attack is over prep time, we go to next stage
            }
        }
    private void ExecuteStrike()
        {
            if (CurrentAttack == AttackType.Swingdown)
                {
                Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) * Timer / execTime);

                if (Timer >= execTime)
                    {
                    CurrentStage = AttackStage.Unwind;
                    }
                }
            if (CurrentAttack == AttackType.Swingup)
                {
                Progress = MathHelper.SmoothStep(0, -SWINGRANGE, (1f - UNWIND) * Timer / execTime);
                //^ swap -SWINGRANGE ^and the 0, that's how I got it to work
                if (Timer >= execTime)
                    {
                    CurrentStage = AttackStage.Unwind;
                    }
                }
        }
    private void UnwindStrike()
    {
        if (CurrentAttack == AttackType.Swingdown)
        {
            Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) + UNWIND * Timer / hideTime);
        }
        else
        {
            Progress = MathHelper.SmoothStep(SWINGRANGE, 0, (-3f + UNWIND) + UNWIND * Timer / hideTime); // -2f + UNWIND is Ideal
        }

        Size = 1f - MathHelper.SmoothStep(0, 1, Timer / hideTime); // Shrinks sword size on unwind

        if (Timer >= hideTime)
        {
            Projectile.Kill();
        }
    }
    }
}