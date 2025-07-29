
using DestroyerTest.Content.Dusts;
using DestroyerTest.Content.RiftBiomeSpread;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RiftBiome
{
	public class RiftWaterStyle : ModWaterStyle
	{

		public override int ChooseWaterfallStyle() {
			return ModContent.GetInstance<RiftWaterfallStyle>().Slot;
		}

		public override int GetSplashDust() {
			return DustID.Wraith;
		}

		public override int GetDropletGore() {
			return ModContent.GoreType<RiftWaterDroplet>();
		}

		public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
			r = 1f;
			g = 1f;
			b = 1f;
		}

		public override Color BiomeHairColor() {
        return new Color(255, 155, 0);
        }


	}
}
