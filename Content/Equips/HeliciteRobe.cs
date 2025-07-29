using rail;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class HeliciteRobe : ModItem
	{
		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<RiftRarity2>(); // The rarity of the item
			Item.defense = 35; // The amount of defense the item will give when equipped
		}
        
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Living_Shadow>(10)
                .AddIngredient<Item_HeliciteCrystal>(25)
                .AddIngredient(ItemID.Silk, 15)
                .AddIngredient(ItemID.Robe)
				.AddTile<Tile_RiftCrucible>()
                .AddCondition(Condition.DownedGolem)
				.Register();
		}
	}
}