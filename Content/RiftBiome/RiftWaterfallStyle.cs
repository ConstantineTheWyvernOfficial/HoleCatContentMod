using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RiftBiome
{
	public class RiftWaterfallStyle : ModWaterfallStyle
	{
		// Makes the waterfall provide light
		// Learn how to make a waterfall: https://terraria.wiki.gg/wiki/Waterfall
		public override void AddLight(int i, int j) =>
			Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(), Color.White.ToVector3() * 0.5f);
	}
}