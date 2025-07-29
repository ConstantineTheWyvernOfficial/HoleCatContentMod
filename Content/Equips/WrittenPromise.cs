
using DestroyerTest.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Equips
{
	public class WrittenPromise : ModItem
	{

		public override void SetDefaults() {
			Item.width = 12; // Width of the item
			Item.height = 14; // Height of the item
			Item.value = Item.sellPrice(gold: 10); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
            Item.vanity = false;
            Item.accessory = true;
		}

		public override void UpdateEquip(Player player) {
			DTUtils.PromiseEquipped = true;
		}


	}
}
