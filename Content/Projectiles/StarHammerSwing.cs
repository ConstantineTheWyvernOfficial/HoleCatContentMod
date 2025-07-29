using DestroyerTest.Common;
using DestroyerTest.Content.Particles;
using InnoVault.PRT;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{

    public class StarHammerSwing : ModProjectile
    {
        public SoundStyle Swing = new SoundStyle("DestroyerTest/Assets/Audio/Constitution/ConSwing", 6);
        public SoundStyle Throw = new SoundStyle("DestroyerTest/Assets/Audio/StarHammerThrow") with { Volume = 3 };
        private const float SWINGRANGE = 1.67f * (float)Math.PI;
        private const float FIRSTHALFSWING = 0.45f;
        private const float WINDUP = 0.05f;
        private const float UNWIND = 0.4f;
        private const float SPINRANGE = 3.5f * (float)Math.PI; // The angle a spin attack covers (630 degrees)
        private const float SPINTIME = 2.5f; // How much longer a spin is than a swing

        private enum AttackType
        {

            SwingDown,
            SwingUp,
            Throw,
            Spin,
        }

        private enum AttackStage
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
                Timer = 0;
            }
        }


        private ref float InitialAngle => ref Projectile.ai[1];
        private ref float Timer => ref Projectile.ai[2];
        private ref float Progress => ref Projectile.localAI[1];
        private ref float Size => ref Projectile.localAI[2];
        private float prepTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float execTime => 16f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private float hideTime => 12f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
        private Player Owner => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.timeLeft = 10000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.spriteDirection = Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;
            float targetAngle = (Main.MouseWorld - Owner.MountedCenter).ToRotation();

            if (CurrentAttack == AttackType.SwingUp)
            {
                if (Projectile.spriteDirection == 1)
                {

                    targetAngle = MathHelper.Clamp(targetAngle, (float)Math.PI * 1 / 6, (float)Math.PI * 2 / 3);
                }
                else
                {
                    if (targetAngle < 0)
                    {
                        targetAngle += 2 * (float)Math.PI;
                    }
                    targetAngle = MathHelper.Clamp(targetAngle, (float)Math.PI * 4 / 3, (float)Math.PI * 5 / 3);
                }
                InitialAngle = targetAngle + FIRSTHALFSWING * SWINGRANGE * Projectile.spriteDirection; // Inverse: add instead of subtract
            }
            else
            {
                if (Projectile.spriteDirection == 1)
                {

                    targetAngle = MathHelper.Clamp(targetAngle, (float)-Math.PI * 1 / 3, (float)Math.PI * 1 / 6);
                }
                else
                {
                    if (targetAngle < 0)
                    {
                        targetAngle += 2 * (float)Math.PI;
                    }

                    targetAngle = MathHelper.Clamp(targetAngle, (float)Math.PI * 5 / 6, (float)Math.PI * 4 / 3);
                }

                InitialAngle = targetAngle - FIRSTHALFSWING * SWINGRANGE * Projectile.spriteDirection; // Otherwise, we calculate the angle
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {

            writer.Write((sbyte)Projectile.spriteDirection);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.spriteDirection = reader.ReadSByte();
        }

        public int ProjectileAmount;

        public override void AI()
        {
            ProjectileAmount = Main.rand.Next(6, 36);

            Owner.itemAnimation = 2;
            Owner.itemTime = 2;


            if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
            {
                Projectile.Kill();
                return;
            }


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

            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, default, lightColor * Projectile.Opacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);

            // Since we are doing a custom draw, prevent it from normally drawing
            return false;
        }


        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Owner.MountedCenter;
            Vector2 end = start + Projectile.rotation.ToRotationVector2() * ((Projectile.Size.Length()) * Projectile.scale);
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 15f * Projectile.scale, ref collisionPoint);
        }

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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = (int?)(target.position.Y + 5);
        }


        public void SetSwordPosition()
        {

            Projectile.rotation = InitialAngle + Projectile.spriteDirection * Progress;


            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f));
            Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2);

            armPosition.Y += Owner.gfxOffY;
            Projectile.Center = armPosition;
            Projectile.scale = Size * 1.2f * Owner.GetAdjustedItemScale(Owner.HeldItem);

            Owner.heldProj = Projectile.whoAmI;


        }


        private void PrepareStrike()
        {

            Progress = WINDUP * SWINGRANGE * (1f - Timer / prepTime);
            Size = MathHelper.SmoothStep(0, 1, Timer / prepTime);

            if (Timer >= prepTime)
            {
                //SoundEngine.PlaySound(SoundID.Item1);
                CurrentStage = AttackStage.Execute;
            }
        }

        private bool HasThrown = false;
        private bool Sound1 = false;
        private bool Sound2 = false;
        private bool Sound3 = false;
        private bool Sound4 = false;

        private void ExecuteStrike()
        {

            Player player = Main.player[Projectile.owner];

            if (CurrentAttack == AttackType.SwingDown)
            {
                Projectile.alpha = 0;
                Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) * Timer / execTime);
                if (Sound1 == false)
                {
                    SoundEngine.PlaySound(SoundID.Item69, player.Center);
                    Sound1 = true;
                }



                Lighting.AddLight(player.Center, ColorLib.StellarColor.ToVector3());

                if (Timer >= execTime)
                {
                    CurrentStage = AttackStage.Unwind;
                }
            }
            else if (CurrentAttack == AttackType.SwingUp)
            {
                Projectile.alpha = 0;
                if (player.direction == 1)
                {
                    Progress = MathHelper.SmoothStep(SWINGRANGE, 0, (1f - UNWIND) * Timer / execTime);
                }
                if (player.direction == -1)
                {
                    Progress = MathHelper.SmoothStep(SWINGRANGE, 2.0f, (1f - UNWIND) * Timer / execTime);
                }
                if (Sound2 == false)
                {
                    SoundEngine.PlaySound(SoundID.Item69, player.Center);
                    Sound2 = true;
                }


                Lighting.AddLight(player.Center, ColorLib.StellarColor.ToVector3());

                if (Timer >= execTime)
                {
                    CurrentStage = AttackStage.Unwind;
                }
            }
            else if (CurrentAttack == AttackType.Throw)
            {
                Projectile.alpha = 255;
                if (HasThrown == false)
                {
                    if (Sound3 == false)
                    {
                        SoundEngine.PlaySound(Throw, player.Center);
                        Sound3 = true;
                    }

                    for (int i = 0; i < 100; i++)
                    {
                        float angle = MathHelper.TwoPi * i / 100f;
                        float radius = 40f + (float)Math.Sin(6 * angle) * 20f; // 6 = number of petals

                        Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                        Vector2 position = Projectile.Center + offset;

                        Dust.NewDustPerfect(position, DustID.YellowTorch).noGravity = true;
                    }
                    float angleIncrement = MathHelper.TwoPi / ProjectileAmount;
                    for (int i = 0; i < ProjectileAmount; i++)
                    {
                        Vector2 velocity = new Vector2(8, 0).RotatedBy(angleIncrement * i);
                        Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, velocity, ProjectileID.Starfury, 5, 1);
                    }
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center, (Main.MouseWorld - player.Center) * 0.65f, ModContent.ProjectileType<StarHammerThrown>(), 20, 6, player.whoAmI);
                    HasThrown = true;
                }
                Lighting.AddLight(player.Center, ColorLib.StellarColor.ToVector3());
                if (HasThrown == true)
                {
                    CurrentStage = AttackStage.Unwind;
                }
            }
            else if (CurrentAttack == AttackType.Spin)
            {
                Projectile.alpha = 0;
                Progress = MathHelper.SmoothStep(0, SPINRANGE, (1f - UNWIND / 2) * Timer / (execTime * SPINTIME));


                if (Timer == (int)(execTime * SPINTIME * 3 / 4))
                {
                    if (Sound4 == false)
                    {
                        SoundEngine.PlaySound(SoundID.Item69, player.Center); // Play sword sound again
                        Sound4 = true;
                    }
                    Projectile.ResetLocalNPCHitImmunity(); // Reset the local npc hit immunity for second half of spin


                }

                if (Timer >= execTime * SPINTIME)
                {
                    CurrentStage = AttackStage.Unwind;
                }
            }
        }

        private void UnwindStrike()
        {
            Player player = Main.player[Projectile.owner];
            if (CurrentAttack == AttackType.SwingDown)
            {
                Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) + UNWIND * Timer / hideTime);
                Size = 1f - MathHelper.SmoothStep(0, 1, Timer / hideTime);

                if (Timer >= hideTime)
                {
                    Projectile.Kill();
                }
            }
            else if (CurrentAttack == AttackType.SwingUp)
            {
                if (player.direction == 1)
                {
                    Progress = MathHelper.SmoothStep(SWINGRANGE, 0, (1f - UNWIND) + UNWIND * Timer / hideTime);
                }
                if (player.direction == -1)
                {
                    Progress = MathHelper.SmoothStep(SWINGRANGE, 1.6f, (1f - UNWIND) + UNWIND * Timer / hideTime);
                }
                Size = 1f - MathHelper.SmoothStep(0, 1, Timer / hideTime);

                if (Timer >= hideTime)
                {
                    Projectile.Kill();
                }
            }
            else if (CurrentAttack == AttackType.Throw)
            {
                Projectile.Kill();
            }
            else if (CurrentAttack == AttackType.Spin)
            {
                Progress = MathHelper.SmoothStep(0, SPINRANGE, (1f - UNWIND / 2) + UNWIND / 2 * Timer / (hideTime * SPINTIME / 2));
                Size = 1f - MathHelper.SmoothStep(0, 1, Timer / (hideTime * SPINTIME / 2));

                if (Timer >= hideTime * SPINTIME / 2)
                {
                    Projectile.Kill();
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            Sound1 = false;
            Sound2 = false;
            Sound3 = false;
            Sound4 = false;
        }
	}
}