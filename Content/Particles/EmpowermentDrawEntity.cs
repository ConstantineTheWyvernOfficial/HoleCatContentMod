using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace DestroyerTest.Content.Particles
{
    public class HeroEmpowermentDrawEntity : ModProjectile
    {
        public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity"; // Path to the texture for the projectile
        private const float MaxSizeMultiplier = 2.5f; // Maximum scale increase
        private const int FadeOutStartTime = 10; // Time left when fading starts
        private const int MaxLifetime = 30; // Total lifetime of the ring effect

        private Player targetPlayer; // Replace targetNPC with targetPlayer

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = MaxLifetime;
            Projectile.scale = 0.1f; // Start small
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        public override void AI()
        {
            // Ensure only one projectile of this type is active
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile otherProjectile = Main.projectile[i];
                if (otherProjectile.active && otherProjectile.type == Projectile.type && otherProjectile.whoAmI != Projectile.whoAmI)
                {
                    Projectile.Kill(); // Kill this projectile if another one is active
                    return;
                }
            }
            
            // Increase size over time
            float lifeRatio = (MaxLifetime - Projectile.timeLeft) / (float)MaxLifetime;
            Projectile.scale = MathHelper.Lerp(0.1f, MaxSizeMultiplier, lifeRatio);
            

            // Find the closest player
            if (targetPlayer == null || !targetPlayer.active)
            {
                targetPlayer = FindClosestPlayer();
            }

            // Lock onto the target player's position
            if (targetPlayer != null && targetPlayer.active)
            {
                Projectile.Center = targetPlayer.Center;
            }

            if (Projectile.scale > 3.0f)
            {
                Projectile.Kill();
            }
            
        }

        private Player FindClosestPlayer()
        {
            Player closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (Player player in Main.player)
            {
                if (!player.active) continue;

                float distance = Vector2.Distance(Projectile.Center, player.Center);
                if (distance < closestDistance)
                {
                    closestPlayer = player;
                    closestDistance = distance;
                }
            }

            return closestPlayer;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Set base color and adjust transparency based on time left
            Color BeamColor = new Color(Main.DiscoR / 2, (byte)(Main.DiscoG / 1.25f), (byte)(Main.DiscoB / 1.5f));
            lightColor = BeamColor;
            
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
    }

    public class NephilimEmpowermentDrawEntity : ModProjectile
    {
        public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity"; // Path to the texture for the projectile
        private const float MaxSizeMultiplier = 2.5f; // Maximum scale increase
        private const int FadeOutStartTime = 10; // Time left when fading starts
        private const int MaxLifetime = 30; // Total lifetime of the ring effect

        private Player targetPlayer; // Replace targetNPC with targetPlayer

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
            // Ensure only one projectile of this type is active
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile otherProjectile = Main.projectile[i];
                if (otherProjectile.active && otherProjectile.type == Projectile.type && otherProjectile.whoAmI != Projectile.whoAmI)
                {
                    Projectile.Kill(); // Kill this projectile if another one is active
                    return;
                }
            }
            
            // Increase size over time
            float lifeRatio = (MaxLifetime - Projectile.timeLeft) / (float)MaxLifetime;
            Projectile.scale = MathHelper.Lerp(0.1f, MaxSizeMultiplier, lifeRatio);
            

            // Find the closest player
            if (targetPlayer == null || !targetPlayer.active)
            {
                targetPlayer = FindClosestPlayer();
            }

            // Lock onto the target player's position
            if (targetPlayer != null && targetPlayer.active)
            {
                Projectile.Center = targetPlayer.Center;
            }

            if (Projectile.scale > 3.0f)
            {
                Projectile.Kill();
            }
            
        }

        private Player FindClosestPlayer()
        {
            Player closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (Player player in Main.player)
            {
                if (!player.active) continue;

                float distance = Vector2.Distance(Projectile.Center, player.Center);
                if (distance < closestDistance)
                {
                    closestPlayer = player;
                    closestDistance = distance;
                }
            }

            return closestPlayer;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Set base color and adjust transparency based on time left
            lightColor = Color.SkyBlue;
            
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
    }

    public class DemigodEmpowermentDrawEntity : ModProjectile
    {
        public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity"; // Path to the texture for the projectile
        private const float MaxSizeMultiplier = 2.5f; // Maximum scale increase
        private const int FadeOutStartTime = 10; // Time left when fading starts
        private const int MaxLifetime = 30; // Total lifetime of the ring effect

        private Player targetPlayer; // Replace targetNPC with targetPlayer

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
            // Ensure only one projectile of this type is active
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile otherProjectile = Main.projectile[i];
                if (otherProjectile.active && otherProjectile.type == Projectile.type && otherProjectile.whoAmI != Projectile.whoAmI)
                {
                    Projectile.Kill(); // Kill this projectile if another one is active
                    return;
                }
            }
            
            // Increase size over time
            float lifeRatio = (MaxLifetime - Projectile.timeLeft) / (float)MaxLifetime;
            Projectile.scale = MathHelper.Lerp(0.1f, MaxSizeMultiplier, lifeRatio);
            

            // Find the closest player
            if (targetPlayer == null || !targetPlayer.active)
            {
                targetPlayer = FindClosestPlayer();
            }

            // Lock onto the target player's position
            if (targetPlayer != null && targetPlayer.active)
            {
                Projectile.Center = targetPlayer.Center;
            }

            if (Projectile.scale > 3.0f)
            {
                Projectile.Kill();
            }
            
        }

        private Player FindClosestPlayer()
        {
            Player closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (Player player in Main.player)
            {
                if (!player.active) continue;

                float distance = Vector2.Distance(Projectile.Center, player.Center);
                if (distance < closestDistance)
                {
                    closestPlayer = player;
                    closestDistance = distance;
                }
            }

            return closestPlayer;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Set base color and adjust transparency based on time left
            lightColor = Color.Lavender;
            
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
    }

    public class SaviorEmpowermentDrawEntity : ModProjectile
    {
        public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity"; // Path to the texture for the projectile
        private const float MaxSizeMultiplier = 2.5f; // Maximum scale increase
        private const int FadeOutStartTime = 10; // Time left when fading starts
        private const int MaxLifetime = 30; // Total lifetime of the ring effect

        private Player targetPlayer; // Replace targetNPC with targetPlayer

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
            // Ensure only one projectile of this type is active
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile otherProjectile = Main.projectile[i];
                if (otherProjectile.active && otherProjectile.type == Projectile.type && otherProjectile.whoAmI != Projectile.whoAmI)
                {
                    Projectile.Kill(); // Kill this projectile if another one is active
                    return;
                }
            }
            
            // Increase size over time
            float lifeRatio = (MaxLifetime - Projectile.timeLeft) / (float)MaxLifetime;
            Projectile.scale = MathHelper.Lerp(0.1f, MaxSizeMultiplier, lifeRatio);
            

            // Find the closest player
            if (targetPlayer == null || !targetPlayer.active)
            {
                targetPlayer = FindClosestPlayer();
            }

            // Lock onto the target player's position
            if (targetPlayer != null && targetPlayer.active)
            {
                Projectile.Center = targetPlayer.Center;
            }

            if (Projectile.scale > 3.0f)
            {
                Projectile.Kill();
            }
            
        }

        private Player FindClosestPlayer()
        {
            Player closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (Player player in Main.player)
            {
                if (!player.active) continue;

                float distance = Vector2.Distance(Projectile.Center, player.Center);
                if (distance < closestDistance)
                {
                    closestPlayer = player;
                    closestDistance = distance;
                }
            }

            return closestPlayer;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Set base color and adjust transparency based on time left
            lightColor = Color.Red;
            
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
    }
    public class AuraThiefEmpowermentDrawEntity : ModProjectile
    {
        public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity"; // Path to the texture for the projectile
        private const float MaxSizeMultiplier = 2.5f; // Maximum scale increase
        private const int FadeOutStartTime = 10; // Time left when fading starts
        private const int MaxLifetime = 30; // Total lifetime of the ring effect

        private Player targetPlayer; // Replace targetNPC with targetPlayer

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
            // Ensure only one projectile of this type is active
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile otherProjectile = Main.projectile[i];
                if (otherProjectile.active && otherProjectile.type == Projectile.type && otherProjectile.whoAmI != Projectile.whoAmI)
                {
                    Projectile.Kill(); // Kill this projectile if another one is active
                    return;
                }
            }
            
            // Increase size over time
            float lifeRatio = (MaxLifetime - Projectile.timeLeft) / (float)MaxLifetime;
            Projectile.scale = MathHelper.Lerp(0.1f, MaxSizeMultiplier, lifeRatio);
            

            // Find the closest player
            if (targetPlayer == null || !targetPlayer.active)
            {
                targetPlayer = FindClosestPlayer();
            }

            // Lock onto the target player's position
            if (targetPlayer != null && targetPlayer.active)
            {
                Projectile.Center = targetPlayer.Center;
            }

            if (Projectile.scale > 3.0f)
            {
                Projectile.Kill();
            }
            
        }

        private Player FindClosestPlayer()
        {
            Player closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (Player player in Main.player)
            {
                if (!player.active) continue;

                float distance = Vector2.Distance(Projectile.Center, player.Center);
                if (distance < closestDistance)
                {
                    closestPlayer = player;
                    closestDistance = distance;
                }
            }

            return closestPlayer;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Set base color and adjust transparency based on time left
            lightColor = new Color(184, 228, 242);
            
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
    }
}