using System;
using DestroyerTest.Common;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.Riftplate;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Resources
{
	public class ShadowCircuitry : ModItem
	{
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			Item.ResearchUnlockCount = 100;
		}

		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 1000;
			Item.rare = ItemRarityID.White;
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, ColorLib.DarkRift4.ToVector3() * 0.55f * Main.essScale);
		}
        public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<Item_Riftplate>(2)
                .AddIngredient(ItemID.Wire, 5)
                .AddIngredient(ItemID.Glass, 3)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

   
}
