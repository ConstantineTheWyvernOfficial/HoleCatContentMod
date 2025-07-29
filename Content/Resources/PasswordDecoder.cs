using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using DestroyerTest.Content.Tiles.Riftplate;

namespace DestroyerTest.Content.Resources
{
	public class PasswordDecoder : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 25;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.

			// The Chlorophyte Extractinator can exchange items. Here we tell it to allow a one-way exchanging of 5 ExampleBar for 2 ChlorophyteBar.
			ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 5, ItemID.Wire, 2);
            Item.CloneDefaults(ItemID.GoldenKey);
            Item.consumable = true; 
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Living_Shadow>(10)
                .AddIngredient(ItemID.Wire, 10)
                .AddIngredient(ItemID.Glass, 3)
				.AddTile<Tile_RiftCrucible>()
				.Register();
		}
	}
}