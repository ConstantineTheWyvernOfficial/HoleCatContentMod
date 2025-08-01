using DestroyerTest.Content.Buffs;
  
using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;

namespace DestroyerTest.Content.Resources
{
	public class RiftData : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 150;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 37; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
            
		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.width = 16;
			Item.height = 16;
			Item.value = 1060; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Blue; 
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<Item_Riftplate>(2)
                .AddIngredient<Living_Shadow>()
				.AddTile<Tile_RiftCrucible>()
				.Register();
		}
	}
}