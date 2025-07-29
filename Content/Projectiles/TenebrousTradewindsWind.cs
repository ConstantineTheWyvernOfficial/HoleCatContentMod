using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;
using System;

namespace DestroyerTest.Content.Projectiles
{
    public class TenebrousTradewindsWind : ModProjectile
    {
        public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity"; // Path to the texture for the projectile


        public override void SetStaticDefaults()
        {
        }

		public override void SetDefaults()
		{
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.friendly = true;
			Projectile.penetrate = -1; // Infinite pierce
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 180; // 10 seconds max lifespan
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.alpha = 160; // Start fully transparent
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
		{
            



             // Use a sine wave to smoothly transition between the two colors
            float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
            lightColor = Color.Lerp(Color.Magenta, Color.Wheat, lerpAmount);
            float fadeFactor = 1f - Projectile.alpha / 255f;
            lightColor *= fadeFactor;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;

			// Draw the base projectile using the default drawing system (Deferred)
			Main.EntitySpriteDraw(
				projectileTexture,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor, 
				Projectile.rotation,
				projectileTexture.Size() / 2,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			// Glow effect (Immediate drawing with Additive blending)
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/TenebrousTradewindsParticle").Value;
			Main.EntitySpriteDraw(
				glowTexture,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor,
				Projectile.rotation,
				glowTexture.Size() / 2,
				0.3f * Projectile.scale,
				SpriteEffects.None,
				0
			);

            // Glow effect (Immediate drawing with Additive blending)
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D glowTextureDim = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/TenebrousTradewindsParticleDim").Value;
			Main.EntitySpriteDraw(
				glowTextureDim,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor * 0.9f,
				Projectile.rotation,
				glowTexture.Size() / 2,
				0.8f * Projectile.scale,
				SpriteEffects.None,
				0
			);
			
			// Restore the deferred mode (for the next drawing of things)
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            
    
            



			return false; // Let the default system handle the base projectile drawing
		}

    

        public override void AI()
        {
            Projectile.velocity *= 0.83f;
            
            

            
            Player player = Main.player[Projectile.owner];

            // Always spinning
            Projectile.rotation += 0.4f * Projectile.direction;
            Projectile.rotation *= 0.99f;

              // Generate flying dust effect
            if (Main.rand.NextBool(3)) // 33% chance per tick
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, Projectile.velocity * 0.2f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                SoundEngine.PlaySound(SoundID.Drown, Projectile.position); 
                target.AddBuff(ModContent.BuffType<FriendlyShimmeringFlames>(), 120);
            }
    }

}

