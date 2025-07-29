using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles;

namespace DestroyerTest.Content.MetallurgySeries
{
	public class Tempering_Station : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 25;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.

			// The Chlorophyte Extractinator can exchange items. Here we tell it to allow a one-way exchanging of 5 ExampleBar for 2 ChlorophyteBar.
			ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 5, ItemID.ChlorophyteBar, 2);
		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_TemperingStation>());
			Item.width = 20;
			Item.height = 20;
			Item.value = 3000; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 9999;
			Item.rare = ModContent.RarityType<MetallurgyRarity>();
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.GoldBar, 4)
                .AddIngredient(ItemID.IronBar, 8)
				.AddIngredient(ItemID.Furnace, 1)
                .AddIngredient(ItemID.GrayBrick, 15)
                .AddIngredient(ItemID.RedBrick, 10)
				.AddTile(TileID.GrayBrick)
				.Register();
		}
	}
}