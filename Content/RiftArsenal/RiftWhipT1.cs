
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.Riftplate;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.RiftArsenal
{
	public class RiftWhipT1 : ModItem
	{

		public override void SetDefaults() {
			// This method quickly sets the whip's properties.
			// Mouse over to see its parameters.
			Item.DefaultToWhip(ModContent.ProjectileType<RiftWhipT1Projectile>(), 70, 2, 4);
			Item.rare = ModContent.RarityType<RiftRarity2>();
			Item.channel = true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Living_Shadow>(8)
				.AddIngredient<Item_Riftplate>(12)
				.AddTile<Tile_RiftCrucible>()
				.Register();
		}

		// Makes the whip receive melee prefixes
		public override bool MeleePrefix() {
			return true;
		}
	}
}