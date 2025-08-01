  
using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles.RiftConfigurator;

namespace DestroyerTest.Content.Tools
{
	public class RiftElectrifier : ModItem
	{
		public override void SetStaticDefaults() {
		}

		public override void SetDefaults() {
			// ModContent.TileType returns the ID of the tile that this item should place when used. ModContent.TileType<T>() method returns an integer ID of the tile provided to it through its generic type argument (the type in angle brackets)
			Item.width = 10;
			Item.height = 64;
			Item.value = 1400; // The cost of the item in copper coins. (1 = 1 copper, 100 = 1 silver, 1000 = 1 gold, 10000 = 1 platinum)
			Item.maxStack = 1;
			Item.rare = ModContent.RarityType<RiftRarity1>();
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_Riftplate>(10)
                .AddIngredient<ShadowCircuitry>(10)
				.AddTile<Tile_RiftConfiguratorTools>()
				.Register();
		}
	}
}