
﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Common;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class MythrilVisage : ModItem
	{

		public override void SetDefaults() {
			Item.width = 24; // Width of the item
			Item.height = 22; // Height of the item
			Item.value = Item.sellPrice(gold: 70); // How many coins the item is worth
			Item.rare = ItemRarityID.Blue; // The rarity of the item
			Item.defense = 10; // The amount of defense the item will give when equipped
            Item.vanity = true;
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ItemID.MythrilChainmail && legs.type == ItemID.MythrilGreaves;
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
			ScepterClassStats.Range += player.statDefense / 3;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient(ItemID.MythrilBar, 10) // Add an ingredient to the recipe
                .AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
