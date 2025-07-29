using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Dusts
{
	public class SoulDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity *= 0.8f; // Multiply the dust's start velocity by 0.4, slowing it down
			dust.noGravity = true; // Makes the dust have no gravity.
			dust.noLight = true; // Makes the dust emit no light.
			dust.scale *= 1.5f; // Multiplies the dust's initial scale by 1.5.
		}

		public void PreDraw(ref Color lightColor)
		{
			lightColor = new Color(184, 228, 242);
			
			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D DustTexture = TextureAssets.Dust.Value;
			
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			foreach (Dust dust in Main.dust)
            {
                if (dust != null && dust.active && dust.type == this.Type) // Ensure it's active and the correct type
                {
                    Main.EntitySpriteDraw(
                        DustTexture,
                        dust.position - Main.screenPosition,
                        null,
                        lightColor,
                        dust.rotation,
                        DustTexture.Size() / 2,
                        dust.scale,
                        SpriteEffects.None,
                        0
                    );
                }
            }

			// Restore normal batch
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		public override bool Update(Dust dust) { // Calls every frame the dust is active
			Color lightColor = new Color(184, 228, 242);
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.scale *= 0.99f;
			dust.velocity -= new Microsoft.Xna.Framework.Vector2(0, 0.12f);

			float light = 0.35f * dust.scale;

			Lighting.AddLight(dust.position, light, light, light);

			if (dust.scale < 0.5f) {
				dust.active = false;
			}

			return false; // Return false to prevent vanilla behavior.
		}

		

	}
}