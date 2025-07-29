using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RiftBiome.RiftDesertResources
{
	public class RiftCactus : ModCactus
	{
		private Asset<Texture2D> texture;
		private Asset<Texture2D> fruitTexture;

		public override void SetStaticDefaults() {
			// Makes Example Cactus grow on ExampleSand. You will need to use ExampleSolution to convert regular sand since ExampleCactus will not grow naturally yet.
			GrowsOnTileId = new int[1] { ModContent.TileType<Tile_RiftSilt>() };
			texture = ModContent.Request<Texture2D>("DestroyerTest/Content/RiftBiome/RiftDesertResources/RiftCactus");
			fruitTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/RiftBiome/RiftDesertResources/RiftCactus_Fruit");
		}

		public override Asset<Texture2D> GetTexture() => texture;

		// This would be where the Cactus Fruit Texture would go, if we had one.
		public override Asset<Texture2D> GetFruitTexture() => fruitTexture;
	}
}