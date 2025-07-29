
﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class CoatStantine : ModItem
	{
		public static readonly int MaxManaIncrease = 20;
		public static readonly int MaxMinionIncrease = 8;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxManaIncrease, MaxMinionIncrease);

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 10); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 160; // The amount of defense the item will give when equipped
            Item.vanity = false;
		}

		public override void UpdateEquip(Player player) {
			player.buffImmune[BuffID.Electrified] = true; // Make the player immune to Electrified
			player.statManaMax2 += MaxManaIncrease; // Increase how many mana points the player can have by 20
			player.maxMinions += MaxMinionIncrease; // Increase how many minions the player can have by one
			player.GetDamage(DamageClass.Generic) += 5f;
		}
	}
}
