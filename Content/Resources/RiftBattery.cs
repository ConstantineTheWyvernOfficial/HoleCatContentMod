using System;
using DestroyerTest.Common;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Resources
{
	public class RiftBattery : ModItem
	{
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 15));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			Item.ResearchUnlockCount = 100;
		}

		public override void SetDefaults() {
			Item.width = 10;
			Item.height = 32;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 1000;
			Item.rare = ModContent.RarityType<RiftRarity1>();
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, ColorLib.DarkRift4.ToVector3() * 0.55f * Main.essScale);
		}
        public override void AddRecipes() {
			CreateRecipe(30)
                .AddIngredient<Item_Riftplate>(30)
                .AddIngredient<Living_Shadow>(15)
                .AddIngredient(ItemID.Glass, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

   
}
