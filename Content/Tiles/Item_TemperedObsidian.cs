
using DestroyerTest.Content.Resources;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles
{
	public class Item_TemperedObsidian : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 100;
            Item.material = true;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_TemperedObsidian>());
			Item.width = 16;
			Item.height = 16;
		}
	}
}