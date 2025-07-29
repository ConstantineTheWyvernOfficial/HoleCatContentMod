using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Dusts
{
	public class RainbowDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
			dust.noGravity = true; // Makes the dust have no gravity.
			dust.noLight = true; // Makes the dust emit no light.
			dust.scale *= 3.5f; // Multiplies the dust's initial scale by 1.5.
		}

		public override bool Update(Dust dust) { // Calls every frame the dust is active
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.scale *= 0.99f;
            dust.color = new Color(Main.DiscoR / 2, (byte)(Main.DiscoG / 1.25f), (byte)(Main.DiscoB / 1.5f));

			float light = 0.05f * dust.scale;

			Lighting.AddLight(dust.position, Main.DiscoR / 2 , (byte)(Main.DiscoG / 1.25f), (byte)(Main.DiscoB / 1.5f));

			if (dust.scale < 0.5f) {
				dust.active = false;
			}

			return false; // Return false to prevent vanilla behavior.
		}
	}
}