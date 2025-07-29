using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Dusts
{
	public class ShimmeringDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity *= 0.4f; // Multiply the dust's start velocity by 0.4, slowing it down
            dust.velocity.Y *= 1.6f;
			dust.noGravity = true; // Makes the dust have no gravity.
			dust.noLight = false; // Makes the dust emit no light.
			dust.scale *= 1.5f; // Multiplies the dust's initial scale by 1.5.
             // Manually setting the dust frame (assuming an 8x8 frame size)
            int frameHeight = 8; // Adjust based on your texture
            dust.frame = new Rectangle(0, Main.rand.Next(4) * frameHeight, frameHeight, frameHeight);
            dust.alpha *= 140;
		}

		public override bool Update(Dust dust) { // Calls every frame the dust is active
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.scale *= 0.9f;
           // Define two colors to cycle between
            Color color1 = new Color(180, 0, 255);  // Change to your first color
            Color color2 = new Color(0, 55, 255); // Change to your second color

            // Use a sine wave to cycle between 0 and 1 smoothly
            float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI))); 
            dust.color = Color.Lerp(color1, color2, lerpAmount);

			float light = 0.05f * dust.scale;

			Lighting.AddLight(dust.position, dust.color.ToVector3() * 0.1f);

			if (dust.scale < 0.5f) {
				dust.active = false;
			}

			return false; // Return false to prevent vanilla behavior.
		}
	}
}