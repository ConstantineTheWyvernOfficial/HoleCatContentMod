
using DestroyerTest.Content.Dusts;
using DestroyerTest.Content.RiftBiomeSpread;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.SHADEMANAGEMENT
{
	public class ShadeWater : ModWaterStyle
	{

		public override int ChooseWaterfallStyle() {
			return ModContent.GetInstance<ShadeWaterfall>().Slot;
		}

		public override int GetSplashDust() {
			return DustID.Wraith;
		}

		public override int GetDropletGore() {
			return ModContent.GoreType<ShadeDroplet>();
		}

		public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
			r = 0;
			g = 0;
			b = 0;
		}

		public override Color BiomeHairColor() {
        return new Color(0, 0, 0);
        }


	}
	public class ShadeDroplet : ModGore
	{
		public override void SetStaticDefaults() {
			ChildSafety.SafeGore[Type] = true;
			GoreID.Sets.LiquidDroplet[Type] = true;

			// Rather than copy in all the droplet specific gore logic, this gore will pretend to be another gore to inherit that logic.
			UpdateType = GoreID.WaterDrip;
		}
	}

    public class ShadeWaterfall : ModWaterfallStyle
	{
		// Makes the waterfall provide light
		// Learn how to make a waterfall: https://terraria.wiki.gg/wiki/Waterfall
		public override void AddLight(int i, int j) =>
			Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), new Color(0, 0, 0).ToVector3() * 0.5f);
	}
}
