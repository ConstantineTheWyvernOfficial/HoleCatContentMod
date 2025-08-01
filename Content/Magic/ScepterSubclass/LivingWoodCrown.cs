
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

namespace DestroyerTest.Content.Magic.ScepterSubclass
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class LivingWoodCrown : ModItem
	{

        public override void SetStaticDefaults() {
			// If your head equipment should draw hair while drawn, use one of the following:
			// ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			//ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;

		}

		public override void SetDefaults() {
			Item.width = 22; // Width of the item
			Item.height = 14; // Height of the item
			Item.value = Item.sellPrice(platinum: 25); // How many coins the item is worth
			Item.rare = ItemRarityID.Expert; // The rarity of the item
			Item.defense = 60; // The amount of defense the item will give when equipped
            Item.vanity = true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient(ItemID.Wood, 14)
                .AddIngredient(ItemID.GoldCrown, 1)
                .AddIngredient<LifeEcho>(15)
				.AddTile(TileID.LivingLoom)
				.Register();
		}
	}
}
