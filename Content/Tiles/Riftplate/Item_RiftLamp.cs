using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	internal class Item_RiftLamp : ModItem
	{

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_RiftLamp>());
			Item.width = 32;
			Item.height = 16;
			Item.value = 500;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Living_Shadow>(6)
                .AddIngredient<Item_Riftplate>(8)
                .AddIngredient<Item_Riftpane>(6)
				.AddTile<Tile_RiftConfiguratorCore>()
				.Register();
		}
	}
}