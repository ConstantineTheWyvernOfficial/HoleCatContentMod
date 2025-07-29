using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Bars;

namespace DestroyerTest.Content.MetallurgySeries
{
	public class EvermoltenSteel : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 25;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 10; // Influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.

		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_ColdSnapSteel>());
			Item.width = 20;
			Item.height = 20;
			Item.value = 750; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 9999;
			Item.rare = ModContent.RarityType<MetallurgyRarity>();
		}

		public bool SteelHoldSlow(Player player)
		{
			if (player.HeldItem.type == ModContent.ItemType<EvermoltenSteel>())
			{
				player.AddBuff(BuffID.OnFire3, 60);
			}
			else
			{
				player.ClearBuff(BuffID.OnFire3);
			}
			return true;
		}

		public void UpdateEquips()
		{
			SteelHoldSlow(Main.LocalPlayer);
		}


		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(10)
				.AddIngredient(ItemID.HellstoneBar, 10)
                .AddIngredient<Steel>(5)
				.AddTile<Tile_Crucible>()
				.Register();
		}
	}
}