
﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class Fracture : ModItem
	{

		public override void SetDefaults() {
			Item.width = 36; // Width of the item
			Item.height = 32; // Height of the item
			Item.value = Item.sellPrice(gold: 70); // How many coins the item is worth
			Item.rare = ModContent.RarityType<ContenderRarity>(); // The rarity of the item
			Item.defense = 60; // The amount of defense the item will give when equipped
            Item.vanity = true;
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		//public override bool IsArmorSet(Item head, Item body, Item legs) {
			//return body.type == ModContent.ItemType<ExampleBreastplate>() && legs.type == ModContent.ItemType<ExampleLeggings>();
		//}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		//public override void UpdateArmorSet(Player player) {
			//player.setBonus = SetBonusText.Value; // This is the setbonus tooltip: "Increases dealt damage by 20%"
			//player.GetDamage(DamageClass.Generic) += AdditiveGenericDamageBonus / 100f; // Increase dealt damage for all weapon classes by 20%
		//}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		//public override void AddRecipes() {
			//CreateRecipe()
				//.Register();
		//}
	}
}
