using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	public class Item_RiftCache : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_RiftCache>());
			// Item.placeStyle = 1; // Use this to place the chest in its locked style
			Item.width = 26;
			Item.height = 22;
			Item.value = 500;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_Riftplate>(5)
                .AddIngredient<Item_Riftpane>(5)
				.AddTile<Tile_RiftCrucible>()
				.Register();
		}
	}
}