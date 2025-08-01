  
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.MeleeWeapons
{

	public class HeliciteShank : ModItem
	{

		public override void SetDefaults() {
			// Common Properties
			Item.width = 46;
			Item.height = 46;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ModContent.RarityType<RiftRarity2>();
			Item.UseSound = new SoundStyle("DestroyerTest/Assets/Audio/SwiftSlice");
        

			// Use Properties
			// Note that useTime and useAnimation for this item don't actually affect the behavior because the held projectile handles that. 
			// Each attack takes a different amount of time to execute
			// Conforming to the item useTime and useAnimation makes it much harder to design
			// It does, however, affect the item tooltip, so don't leave it out.
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;

			// Weapon Properties
			Item.autoReuse = true;
			Item.reuseDelay = 10;
			Item.knockBack = 7;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.damage = 60; // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = DamageClass.MeleeNoSpeed; // Deals melee damage
			Item.noMelee = true;  // This makes sure the item does not deal damage from the swinging animation
			Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand

			// Projectile Properties
			Item.shoot = ModContent.ProjectileType<HeliciteShankSlice>(); // The sword as a projectile
		}

		public override bool MeleePrefix() {
			return true; // return true to allow weapon to have melee prefixes (e.g. Legendary)
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_HeliciteCrystal>(26)
                .AddIngredient<Item_Riftplate>(10)
				.AddTile<Tile_RiftConfiguratorWeaponry>()
				.Register();
		}
	}
}