using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;
using Terraria.DataStructures;
using DestroyerTest.Common;

namespace DestroyerTest.Content.Particles
{
    public class HitAnimParticle_Javelin : ModProjectile
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

        float randomRotation = Main.rand.NextFloat(0f, MathHelper.TwoPi); // Random rotation

        bool backupCheck = false;

        public override void OnSpawn(IEntitySource source)
        {
            if (backupCheck == false)
            {
            Projectile.rotation = randomRotation;
            backupCheck = true;
            }
        }


        private NPC targetNPC; // Replace targetPlayer with targetNPC

        
        public override void AI()
        {
            
            // Increase size over time
            float lifeRatio = (MaxLifetime - Projectile.timeLeft) / (float)MaxLifetime;
            Projectile.scale = MathHelper.Lerp(0.1f, MaxSizeMultiplier, lifeRatio);

            // Find the closest enemy NPC
            if (targetNPC == null || !targetNPC.active || targetNPC.friendly || targetNPC.dontTakeDamage)
            {
                targetNPC = FindClosestNPC();
            }

            // Lock onto the target NPC's position
            if (targetNPC != null && targetNPC.active && !targetNPC.friendly && !targetNPC.dontTakeDamage)
            {
                Projectile.Center = targetNPC.Center;
            }

            if (Projectile.scale > 3.0f)
            {
                Projectile.Kill();
            }
        }

        private NPC FindClosestNPC()
        {
            NPC closestNPC = null;
            float closestDistance = float.MaxValue;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;

                float distance = Vector2.Distance(Projectile.Center, npc.Center);
                if (distance < closestDistance)
                {
                    closestNPC = npc;
                    closestDistance = distance;
                }
            }

            return closestNPC;
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
              
                lightColor = ColorLib.JavelinEnergy * 0.7f;

                // Prepare for sprite drawing
                SpriteBatch spriteBatch = Main.spriteBatch;
                Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/HitAnim_512x512").Value;

                // Calculate the current frame based on the projectile's lifetime
                int totalFrames = 9; // Total number of frames in the animation
                int frameWidth = 512; // Width of each frame
                int frameHeight = 512; // Height of each frame
                int framesPerRow = 3; // Number of frames per row in the texture

                int currentFrame = (int)((1f - (Projectile.timeLeft / (float)MaxLifetime)) * totalFrames);
                if (currentFrame >= totalFrames) currentFrame = totalFrames - 1; // Clamp to the last frame

                // Calculate the source rectangle for the current frame
                int frameX = (currentFrame % framesPerRow) * frameWidth;
                int frameY = (currentFrame / framesPerRow) * frameHeight;
                Rectangle sourceRectangle = new Rectangle(frameX, frameY, frameWidth, frameHeight);

                // End previous batch before starting a new one
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                // Draw the current frame of the animation
                Main.EntitySpriteDraw(glowTexture, Projectile.Center - Main.screenPosition, sourceRectangle, lightColor, Projectile.rotation, new Vector2(frameWidth / 2, frameHeight / 2), Projectile.scale * 0.6f, SpriteEffects.None, 0);

                // Restore default sprite batch
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                return true;
            }
    }

    public class HitAnimParticle_TK : ModProjectile
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

        float randomRotation = Main.rand.NextFloat(0f, MathHelper.TwoPi); // Random rotation

        bool backupCheck = false;

        public override void OnSpawn(IEntitySource source)
        {
            if (backupCheck == false)
            {
            Projectile.rotation = randomRotation;
            backupCheck = true;
            }
        }


        private NPC targetNPC; // Replace targetPlayer with targetNPC

        
        public override void AI()
        {
            
            // Increase size over time
            float lifeRatio = (MaxLifetime - Projectile.timeLeft) / (float)MaxLifetime;
            Projectile.scale = MathHelper.Lerp(0.1f, MaxSizeMultiplier, lifeRatio);

            // Find the closest enemy NPC
            if (targetNPC == null || !targetNPC.active || targetNPC.friendly || targetNPC.dontTakeDamage)
            {
                targetNPC = FindClosestNPC();
            }

            // Lock onto the target NPC's position
            if (targetNPC != null && targetNPC.active && !targetNPC.friendly && !targetNPC.dontTakeDamage)
            {
                Projectile.Center = targetNPC.Center;
            }

            if (Projectile.scale > 3.0f)
            {
                Projectile.Kill();
            }
        }

        private NPC FindClosestNPC()
        {
            NPC closestNPC = null;
            float closestDistance = float.MaxValue;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;

                float distance = Vector2.Distance(Projectile.Center, npc.Center);
                if (distance < closestDistance)
                {
                    closestNPC = npc;
                    closestDistance = distance;
                }
            }

            return closestNPC;
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
              
                lightColor = ColorLib.TenebrisGradient;

                // Prepare for sprite drawing
                SpriteBatch spriteBatch = Main.spriteBatch;
                Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/HitAnim_512x512").Value;

                // Calculate the current frame based on the projectile's lifetime
                int totalFrames = 9; // Total number of frames in the animation
                int frameWidth = 512; // Width of each frame
                int frameHeight = 512; // Height of each frame
                int framesPerRow = 3; // Number of frames per row in the texture

                int currentFrame = (int)((1f - (Projectile.timeLeft / (float)MaxLifetime)) * totalFrames);
                if (currentFrame >= totalFrames) currentFrame = totalFrames - 1; // Clamp to the last frame

                // Calculate the source rectangle for the current frame
                int frameX = (currentFrame % framesPerRow) * frameWidth;
                int frameY = (currentFrame / framesPerRow) * frameHeight;
                Rectangle sourceRectangle = new Rectangle(frameX, frameY, frameWidth, frameHeight);

                // End previous batch before starting a new one
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                // Draw the current frame of the animation
                Main.EntitySpriteDraw(glowTexture, Projectile.Center - Main.screenPosition, sourceRectangle, lightColor, Projectile.rotation, new Vector2(frameWidth / 2, frameHeight / 2), Projectile.scale * 0.8f, SpriteEffects.None, 0);

                // Restore default sprite batch
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                return true;
            }
    }

    public class TileDeathAnimParticle : ModProjectile
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

        float randomRotation = Main.rand.NextFloat(0f, MathHelper.TwoPi); // Random rotation

        bool backupCheck = false;

        public override void OnSpawn(IEntitySource source)
        {
            if (backupCheck == false)
            {
            Projectile.rotation = randomRotation;
            backupCheck = true;
            }
        }


        

        
        public override void AI()
        {
            
            // Increase size over time
            float lifeRatio = (MaxLifetime - Projectile.timeLeft) / (float)MaxLifetime;
            Projectile.scale = MathHelper.Lerp(0.1f, MaxSizeMultiplier, lifeRatio);

           

            if (Projectile.scale > 3.0f)
            {
                Projectile.Kill();
            }
        }

        

        public override bool PreDraw(ref Color lightColor)
            {
              
                lightColor = ColorLib.JavelinEnergy * 0.7f;

                // Prepare for sprite drawing
                SpriteBatch spriteBatch = Main.spriteBatch;
                Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/HitAnim_512x512").Value;

                // Calculate the current frame based on the projectile's lifetime
                int totalFrames = 9; // Total number of frames in the animation
                int frameWidth = 512; // Width of each frame
                int frameHeight = 512; // Height of each frame
                int framesPerRow = 3; // Number of frames per row in the texture

                int currentFrame = (int)((1f - (Projectile.timeLeft / (float)MaxLifetime)) * totalFrames);
                if (currentFrame >= totalFrames) currentFrame = totalFrames - 1; // Clamp to the last frame

                // Calculate the source rectangle for the current frame
                int frameX = (currentFrame % framesPerRow) * frameWidth;
                int frameY = (currentFrame / framesPerRow) * frameHeight;
                Rectangle sourceRectangle = new Rectangle(frameX, frameY, frameWidth, frameHeight);

                // End previous batch before starting a new one
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                // Draw the current frame of the animation
                Main.EntitySpriteDraw(glowTexture, Projectile.Center - Main.screenPosition, sourceRectangle, lightColor, Projectile.rotation, new Vector2(frameWidth / 2, frameHeight / 2), Projectile.scale * 0.6f, SpriteEffects.None, 0);

                // Restore default sprite batch
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                return true;
            }
    }
}
