using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles.Riftplate;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.RiftConfigurator
{
	public class FurnitureDrive: ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.width = 50;
			Item.height = 64;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_Riftplate>(2)
                .AddIngredient<Item_Riftpane>(8)
                .AddIngredient(ItemID.Nanites, 2)
				.AddTile<Tile_RiftCrucible>()
				.Register();
		}

	}
}