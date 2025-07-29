
using rail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Content.Projectiles.AmmoProjectiles;

namespace DestroyerTest.Content.Ammunitions
{
	// This example is similar to the Wooden Arrow item
	public class TacticalArrow : ModItem
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
			Item.shoot = ModContent.ProjectileType<TacticalArrowProjectile>(); // The projectile that weapons fire when using this item as ammunition.
			Item.shootSpeed = 20f; // The speed of the projectile.
			Item.ammo = AmmoID.Arrow; // The ammo class this ammo belongs to.
		}

		// For a more detailed explanation of recipe creation, please go to Content/ExampleRecipes.cs.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Nanites, 5)
                .AddIngredient(ItemID.Wire, 5)
                .AddIngredient<PrecisionArrow>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}