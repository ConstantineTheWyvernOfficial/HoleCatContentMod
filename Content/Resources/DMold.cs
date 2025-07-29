using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Resources
{
	public class DMold : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 25;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
            
		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.width = 40;
			Item.height = 40;
			Item.value = 50; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 9999;
			Item.rare = ModContent.RarityType<MetallurgyRarity>();
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.StoneBlock, 5)
				.AddIngredient(ItemID.AshBlock, 10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}