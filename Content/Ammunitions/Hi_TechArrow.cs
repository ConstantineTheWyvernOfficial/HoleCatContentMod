
using rail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Content.Projectiles.AmmoProjectiles;
using SteelSeries.GameSense;
using DestroyerTest.Content.Resources;

namespace DestroyerTest.Content.Ammunitions
{
	// This example is similar to the Wooden Arrow item
	public class Hi_TechArrow : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.width = 14;
			Item.height = 42;

			Item.damage = 6; // Keep in mind that the arrow's final damage is combined with the bow weapon damage.
			Item.DamageType = DamageClass.Ranged;

			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.knockBack = 0f;
			Item.value = Item.sellPrice(copper: 16);
			Item.shoot = ModContent.ProjectileType<Hi_TechArrowProjectile>(); // The projectile that weapons fire when using this item as ammunition.
			Item.shootSpeed = 10f; // The speed of the projectile.
			Item.ammo = AmmoID.Arrow; // The ammo class this ammo belongs to.
		}

		// For a more detailed explanation of recipe creation, please go to Content/ExampleRecipes.cs.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.MartianConduitPlating, 5)
                .AddIngredient(ItemID.Wire, 5)
                .AddIngredient<TacticalArrow>(1)
                .AddIngredient<TeslaScrap>(3)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}