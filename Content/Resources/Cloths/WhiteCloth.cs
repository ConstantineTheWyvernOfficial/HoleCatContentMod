using DestroyerTest.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Resources.Cloths
{
	public class WhiteCloth : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 25;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 2; // Influences the inventory sort order.
		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.width = 26;
			Item.height = 28;
			Item.value = 100; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.TatteredCloth, 2)
                .AddIngredient<EchoFluid>(1)
				.AddTile(TileID.DyeVat)
				.Register();
            CreateRecipe()
				.AddIngredient(ItemID.Silk, 2)
                .AddIngredient<EchoFluid>(1)
				.AddTile(TileID.DyeVat)
				.Register();
			foreach (int itemType in ContentSamples.ItemsByType.Keys)
        {
            Item item = new Item();
            item.SetDefaults(itemType);

            if (item.TryGetGlobalItem(out NonWhiteCloth globalItem) && globalItem.isNonWhiteCloth)
            {
                Recipe recipe = CreateRecipe();
                recipe.AddIngredient(itemType); // Uses the non-white cloth item
                recipe.AddIngredient<EchoFluid>(2); // Your special dye remover
                recipe.AddTile(TileID.DyeVat); // Example crafting station
                recipe.Register();
            }
        }
				
		}
	}
}