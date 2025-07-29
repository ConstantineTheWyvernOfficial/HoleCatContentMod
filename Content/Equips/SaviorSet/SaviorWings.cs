
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Equips.HeroSet;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.MetallurgySeries.TemperedAlloys;

namespace DestroyerTest.Content.Equips.SaviorSet
{
	[AutoloadEquip(EquipType.Wings)]
	public class SaviorWings : ModItem
	{
		public override void SetStaticDefaults() {
			// These wings use the same values as the solar wings
			// Fly time: 180 ticks = 3 seconds
			// Fly speed: 9
			// Acceleration multiplier: 2.5
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(120, 16f, 1.5f);
		}

		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 36;
			Item.value = 10000;
			Item.rare = ModContent.RarityType<GoliathRarity>(); // The rarity of the item
			Item.accessory = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
			ascentWhenFalling = 3.0f; // Falling glide speed
			ascentWhenRising = 3.0f; // Rising speed
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<TemperedCunife>(16)
                .AddIngredient<Item_TemperedObsidian>(6)
                .AddIngredient(ItemID.ChlorophyteBar, 12)
				.AddTile(TileID.MythrilAnvil)
				.SortBefore(Main.recipe.First(recipe => recipe.createItem.wingSlot != -1)) // Places this recipe before any wing so every wing stays together in the crafting menu.
				.Register();
		}
	}
}