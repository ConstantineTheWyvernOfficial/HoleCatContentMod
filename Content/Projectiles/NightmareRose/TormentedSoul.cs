using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles.NightmareRose
{
    public class TormentedSoul : ModProjectile
    {

        private Player HomingTarget
        {
            get => Projectile.ai[0] == 0 ? null : Main.player[(int)Projectile.ai[0] - 1];
            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
            }
        }

        public ref float DelayTimer => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24; // The width of projectile hitbox
            Projectile.height = 30; // The height of projectile hitbox

            Projectile.DamageType = DamageClass.Generic; // What type of damage does this projectile affect?
            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = true; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 1f; // How much light emit around the projectile
            Projectile.timeLeft = 180; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
        }

        private void AnimateProjectile()
        {
            // Loop through the frames, assuming each frame lasts 5 ticks
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            // Calculate source rectangle for current frame
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);

            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            sb.End(); // End vanilla drawing
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            TelegraphLine(sb);
            sb.Draw(texture, drawPos, sourceRect, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            

            sb.End(); // End additive
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false; // We handled drawing
        }

        public void TelegraphLine(SpriteBatch SB)
        {
            Texture2D telegraphTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Extras/FlowerBombTelegraphLine").Value;
            float totalLength = 2400f;
            Vector2 start = IntialPos;

            float opacity = 0f;
            int totalLifetime = 180; // The total lifetime of the projectile

            // Opacity goes from 1 (255) to 0, then back to 1 over the lifetime
            float t = Projectile.timeLeft / (float)totalLifetime;
            // t goes from 1 (spawn) to 0 (death)
            // Use SmoothStep for smooth fade in/out
            opacity = 4f * t * (1f - t);

            if (Projectile.active)
            {
                Vector2 drawPos = start - Main.screenPosition;
                float segmentHeight = telegraphTexture.Height;
                int numSegments = (int)(totalLength / segmentHeight);

                for (int i = 0; i < numSegments; i++)
                {
                    Vector2 segmentPos = drawPos - new Vector2(0, i * segmentHeight);
                    SB.Draw(
                        telegraphTexture,
                        segmentPos,
                        null,
                        Color.MediumPurple * opacity,
                        0f,
                        new Vector2(telegraphTexture.Width / 2f, 0f), // origin: middle bottom of each tile
                        1f,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }


        public Vector2 IntialPos;

        public override void OnSpawn(IEntitySource source)
        {
            IntialPos = Projectile.Center;
        }

        // Custom AI
        public override void AI()
        {
            AnimateProjectile();

            float maxDetectRadius = 120f; // The maximum radius at which a projectile can detect a target

            // First, we find a homing target if we don't have one
            if (HomingTarget == null)
            {
                HomingTarget = FindClosestPlayer(maxDetectRadius);
            }

            // If we have a homing target, make sure it is still valid. If the NPC dies or moves away, we'll want to find a new target
            if (HomingTarget != null && !IsValidTarget(HomingTarget))
            {
                HomingTarget = null;
            }

            // If we don't have a target, don't adjust trajectory
            if (HomingTarget == null)
                return;

            // If found, we rotate the projectile velocity in the direction of the target.
            // We only rotate by 3 degrees an update to give it a smooth trajectory. Increase the rotation speed here to make tighter turns
            float length = Projectile.velocity.Length();
            float targetAngle = Projectile.AngleTo(HomingTarget.Center);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(180) + MathHelper.PiOver2;
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(30)).ToRotationVector2() * length;



            Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.DemonTorch, 0, 0, 70, default, 1.0f);

        }

        // Finding the closest NPC to attack within maxDetectDistance range
        // If not found then returns null
        public Player FindClosestPlayer(float maxDetectDistance)
        {
            Player closestPlayer = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs
            foreach (var target in Main.player)
            {
                // Check if NPC able to be targeted. 
                if (IsValidTarget(target))
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestPlayer = target;
                    }
                }
            }

            return closestPlayer;
        }

        public bool IsValidTarget(Player target)
        {

            return (target.active == true && target.statLife > 1);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.DemonTorch, Projectile.velocity.X * 0.7f, Projectile.velocity.Y * 0.7f, 0, default, 1);
        }

    }
}