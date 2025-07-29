
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	public class Item_RiftTable : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_RiftTable>());
			Item.width = 38;
			Item.height = 24;
			Item.value = 150;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_Riftplate>(6)
                .AddIngredient<Item_Riftpane>(4)
                .AddIngredient<Item_HeliciteCrystal>(2)
                .AddTile<Tile_RiftCrucible>()
				.Register();
		}
	}
}