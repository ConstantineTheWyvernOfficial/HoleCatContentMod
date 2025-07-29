using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.AchievementPaintingTiles
{
	public class WhackAMole_Master_Painting: ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 60; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.

		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.DefaultToPlaceableTile(ModContent.TileType<WhackAMole_Master_Painting_Tile>());
			Item.width = 64;
			Item.height = 64;
			Item.value = 10000; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 1;
			Item.rare = ItemRarityID.Blue;
		}
	}
}