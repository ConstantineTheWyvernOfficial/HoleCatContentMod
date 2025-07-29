using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace DestroyerTest.Content.Particles
{
    public class ParticleDrawEntity : ModProjectile
    {
        private NPC targetNPC;
        private const float MaxSizeMultiplier = 2.5f; // Maximum scale increase
        private const int FadeOutStartTime = 10; // Time left when fading starts
        private const int MaxLifetime = 30; // Total lifetime of the ring effect

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = MaxLifetime;
            Projectile.scale = 0.1f; // Start small
        }

        public override void AI()
        {
            // Increase size over time
            float lifeRatio = (MaxLifetime - Projectile.timeLeft) / (float)MaxLifetime;
            Projectile.scale = MathHelper.Lerp(0.1f, MaxSizeMultiplier, lifeRatio);

            // Find the closest NPC
            if (targetNPC == null || !targetNPC.active)
            {
                targetNPC = FindClosestNPC();
            }

            // Lock onto the target NPC's position
            if (targetNPC != null && targetNPC.active)
            {
                Projectile.Center = targetNPC.Center;
            }
        }

        private NPC FindClosestNPC()
        {
            NPC closestNPC = null;
            float closestDistance = float.MaxValue;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly) continue;

                float distance = Vector2.Distance(Projectile.Center, npc.Center);
                if (distance < closestDistance)
                {
                    closestNPC = npc;
                    closestDistance = distance;
                }
            }

            return closestNPC;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Set base color and adjust transparency based on time left
            lightColor = new Color(255, 60, 60);
            
            if (Projectile.timeLeft < FadeOutStartTime)
            {
                float fadeFactor = Projectile.timeLeft / (float)FadeOutStartTime;
                lightColor *= fadeFactor; // Fade out as time ends
            }

            // Prepare for sprite drawing
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/BloomRing").Value;

            // End previous batch before starting a new one
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            // Draw the expanding glow ring
            Main.EntitySpriteDraw(glowTexture, Projectile.Center - Main.screenPosition, null, lightColor, 0f, glowTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            // Restore default sprite batch
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            // Clean up, remove any effect on the NPC
            if (targetNPC != null && targetNPC.active)
            {
                targetNPC.velocity = targetNPC.oldVelocity;
            }
        }
    }
}
