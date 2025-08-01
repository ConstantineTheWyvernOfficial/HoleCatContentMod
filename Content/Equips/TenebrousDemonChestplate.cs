using rail;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class TenebrousDemonChestplate : ModItem
	{

		public override void SetDefaults() {
			Item.width = 44; // Width of the item
			Item.height = 24; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<ShimmeringRarity>(); // The rarity of the item
			Item.defense = 16; // The amount of defense the item will give when equipped
		}


		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<RiftGuardChestPlate>(1)
                .AddIngredient<Tenebris>(6)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}