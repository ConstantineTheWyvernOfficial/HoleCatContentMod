using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Resources;

using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Equips.HeroSet
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class HeroGreaves : ModItem
	{
		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<GoliathRarity>(); // The rarity of the item
			Item.defense = 3; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
        player.armorEffectDrawOutlinesForbidden = true; // or whatever action you're trying to trigger
        }
	}
}