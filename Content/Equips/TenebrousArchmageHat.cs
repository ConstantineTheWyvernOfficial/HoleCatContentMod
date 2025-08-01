
﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using Steamworks;
using DestroyerTest.Common;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Resources;
using Microsoft.Xna.Framework;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Buffs;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class TenebrousArchmageHat : ModItem
	{

        public override void SetStaticDefaults() {
			// If your head equipment should draw hair while drawn, use one of the following:
			// ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			//ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;

		}

		public override void SetDefaults() {
			Item.width = 30; // Width of the item
			Item.height = 20; // Height of the item
			Item.value = Item.sellPrice(gold: 70); // How many coins the item is worth
			Item.rare = ModContent.RarityType<ShimmeringRarity>(); // The rarity of the item
			Item.defense = 60; // The amount of defense the item will give when equipped
            Item.vanity = true;
		}

		//IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<TenebrousArchmageCoat>() && legs.type == ModContent.ItemType<TenebrousArchmagePants>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
			if (IsArmorSet(player.armor[0], player.armor[1], player.armor[2])) {
            player.GetDamage(ModContent.GetInstance<ScepterClass>()) *= 1.15f; // Increase dealt damage for all weapon classes by 20%
			player.moveSpeed += 0.6f;
            player.statDefense -= 28;
            player.AddBuff(ModContent.BuffType<ShimmeringEmpowerment>(), 1200);
			}
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<Item_HeliciteCrystal>(14)
                .AddIngredient<ShimmeringSludge>(3)
                .AddIngredient<Tenebris>(8)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
