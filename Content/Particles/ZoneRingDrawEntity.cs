using DestroyerTest.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Particles
{
public class ZoneRingDrawEntity : ModProjectile
    {
        public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity"; // Path to the texture for the projectile


		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120000;
			Projectile.scale = 1.0f; // Start small
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
        }

        public override bool PreDraw(ref Color lightColor)
		{
			lightColor = ColorLib.Rift;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


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

			Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/BloomRingSharp1").Value;
			Main.EntitySpriteDraw(
				glowTexture,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor,
				Projectile.rotation,
				glowTexture.Size() / 2,
				2 * Projectile.scale,
				SpriteEffects.None,
				0
			);

			// Restore the deferred mode (for the next drawing of things)
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			return false; // Let the default system handle the base projectile drawing
		}
    }
}