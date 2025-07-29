using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	public class Item_RiftChair : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_RiftChair>());
			Item.width = 12;
			Item.height = 30;
			Item.value = 150;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_Riftplate>(5)
                .AddIngredient<Living_Shadow>(3)
				.AddTile<Tile_RiftConfiguratorCore>()
				.Register();
		}
	}
}