using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles
{
	public class BarrierLatticeItemR : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 60; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.

		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.DefaultToPlaceableTile(ModContent.TileType<BarrierLatticeR>());
			Item.width = 18;
			Item.height = 20;
			Item.value = 10000; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Blue;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(2)
				.AddIngredient(ItemID.Glass, 16)
				.AddTile(TileID.GlassKiln)
				.Register();
		}
	}
    public class BarrierLatticeItemL : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 60; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.

		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.DefaultToPlaceableTile(ModContent.TileType<BarrierLatticeL>());
			Item.width = 18;
			Item.height = 20;
			Item.value = 10000; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Blue;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(2)
				.AddIngredient(ItemID.Glass, 16)
				.AddTile(TileID.GlassKiln)
				.Register();
		}
	}
}