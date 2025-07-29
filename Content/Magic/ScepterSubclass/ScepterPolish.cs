
using DestroyerTest.Common;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Magic.ScepterSubclass
{
	public class ScepterPolish : ModItem
	{

		public override void SetDefaults() {
			Item.width = 22; // Width of the item
			Item.height = 28; // Height of the item
			Item.value = Item.sellPrice(gold: 86); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
            Item.vanity = false;
            Item.accessory = true;
		}

		public override void UpdateEquip(Player player) {
            ScepterClassStats.Range += 3;
		}
	}
}
