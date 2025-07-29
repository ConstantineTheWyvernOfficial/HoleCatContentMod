
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	public class Item_RiftBookcase : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_RiftBookcase>());
			Item.width = 24;
			Item.height = 32;
			Item.value = 150;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Book, 15)
				.AddIngredient<Item_Riftplate>(10)
                .AddTile<Tile_RiftCrucible>()
				.Register();
		}
	}
}