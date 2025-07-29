using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Bars;

namespace DestroyerTest.Content.MetallurgySeries.TemperedAlloys
{
	public class TemperedSteel : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 40; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.

		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_TemperedSteel>());
			Item.width = 30;
			Item.height = 24;
			Item.value = 2500; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 9999;
			Item.rare = ModContent.RarityType<MetallurgyRarity>();
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<Steel>(2)
				.AddIngredient<Carbon>(1)
				.AddTile<Tile_TemperingStation>()
				.Register();
		}
	}
}