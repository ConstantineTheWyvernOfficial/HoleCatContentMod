using System;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Tiles.Bars;

namespace DestroyerTest.Content
{
	public class Tenebris : ModItem
	{
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 13));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = false;
			Item.ResearchUnlockCount = 25;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 1000;
			Item.alpha = 0;
			Item.rare = ModContent.RarityType<ShimmeringRarity>();
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_Tenebris>());
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Item_Riftplate>(4)
                .AddIngredient<ShimmeringSludge>(4)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
        }
    }

}
