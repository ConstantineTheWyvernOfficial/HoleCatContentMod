using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	public class Item_RiftplatePlatform : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 200;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_RiftplatePlatform>());
			Item.width = 8;
			Item.height = 10;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(10)
				.AddIngredient<Item_Riftplate>(2)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}