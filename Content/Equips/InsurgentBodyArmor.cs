using rail;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Resources;

using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Rarity;


namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class InsurgentBodyArmor : ModItem
	{
		public override void SetDefaults() {
			Item.width = 30; // Width of the item
			Item.height = 22; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<ScepterArmorPHMRarity>(); // The rarity of the item
			Item.defense = 8; // The amount of defense the item will give when equipped
		}
	}
}