using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Dusts
{
	public class BlackFlame : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
			dust.noGravity = true; // Makes the dust have no gravity.
			dust.noLight = false; // Makes the dust emit no light.
			dust.scale *= 2.5f; // Multiplies the dust's initial scale by 1.5.
			int frameHeight = 20; // Adjust based on your texture
            dust.frame = new Rectangle(0, Main.rand.Next(4) * frameHeight, frameHeight, frameHeight);
            dust.alpha *= 140;
		}

		public override bool Update(Dust dust) { // Calls every frame the dust is active
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.scale *= 1.0f;

			float light = 0.35f * dust.scale;

			if (dust.scale < 0.5f) {
				dust.active = false;
			}

			return false; // Return false to prevent vanilla behavior.
		}
	}
}