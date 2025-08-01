using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Resources;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Equips.ContenderSet
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class ContenderGreaves : ModItem
	{
		public override void SetDefaults() {
			Item.width = 26; // Width of the item
			Item.height = 20; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<ContenderRarity>(); // The rarity of the item
			Item.defense = 40; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
        player.AddBuff(BuffID.BabyBird, 9000);
        }


		
	}
}