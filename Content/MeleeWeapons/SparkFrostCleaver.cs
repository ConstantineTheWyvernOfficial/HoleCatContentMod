using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Resources.Cloths;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.MeleeWeapons
{
	// ExampleCustomSwingSword is an example of a sword with a custom swing using a held projectile
	// This is great if you want to make melee weapons with complex swing behavior
	public class SparkFrostCleaver : ModItem
	{


		public override void SetDefaults() {
			// Common Properties
			Item.width = 162;
			Item.height = 162;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ItemRarityID.Green;
            

			// Use Properties
			// Note that useTime and useAnimation for this item don't actually affect the behavior because the held projectile handles that. 
			// Each attack takes a different amount of time to execute
			// Conforming to the item useTime and useAnimation makes it much harder to design
			// It does, however, affect the item tooltip, so don't leave it out.
			Item.useTime = 60;
			Item.useAnimation =60;
			Item.useStyle = ItemUseStyleID.Shoot;

			// Weapon Properties
			Item.knockBack = 70;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.autoReuse = true; // This determines whether the weapon has autoswing
			Item.damage = 95; // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = DamageClass.Melee; // Deals melee damage
			Item.noMelee = true;  // This makes sure the item does not deal damage from the swinging animation
			Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand

			// Projectile Properties
			Item.shoot = ModContent.ProjectileType<SparkFrostCleaverSwing>(); // The sword as a projectile
		}

		public override bool MeleePrefix() {
			return true; // return true to allow weapon to have melee prefixes (e.g. Legendary)
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<ColdSnapSteel>(6)
                .AddIngredient<EvermoltenSteel>(6)
                .AddIngredient(ItemID.WandofSparking)
				.AddIngredient(ItemID.WandofFrosting)
				.AddIngredient(ItemID.SoulofMight, 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}