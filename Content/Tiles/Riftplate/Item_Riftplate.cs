
using DestroyerTest.Content.Resources;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	public class Item_Riftplate : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 100;
            Item.material = true;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_Riftplate>());
			Item.width = 12;
			Item.height = 12;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(20)
				.AddIngredient<Living_Shadow>(5)
                .AddIngredient(ItemID.IronBar, 2)
				.AddTile(TileID.AdamantiteForge)
				.Register();
            CreateRecipe(20)
				.AddIngredient<Living_Shadow>(5)
                .AddIngredient(ItemID.LeadBar, 2)
				.AddTile(TileID.AdamantiteForge)
				.Register();
		}

	}
}