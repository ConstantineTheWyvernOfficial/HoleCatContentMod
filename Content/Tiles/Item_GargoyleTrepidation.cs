
﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles
{
	// The item used to place the statue.
	public class Item_GargoyleTrepidation : ModItem
	{
		public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tile_GargoyleTrepidation>());
            Item.height = 200;
            Item.width = 130;
			Item.placeStyle = 0;
		}
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.StoneBlock, 50)
                .AddIngredient(ItemID.CatBast)
				.AddTile(TileID.HeavyWorkBench)
				.Register();
		}
	}
}
