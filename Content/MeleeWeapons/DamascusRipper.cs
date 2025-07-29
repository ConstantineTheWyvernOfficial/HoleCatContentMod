using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.MetallurgySeries.TemperedAlloys;
using DestroyerTest.Content.MetallurgySeries; // Add this line if CT3_Swing is in the Projectiles namespace

namespace DestroyerTest.Content.MeleeWeapons
{
	public class DamascusRipper : ModItem
	{

        //Weapon Properties
        public override void SetDefaults() {
			// Common Properties
			Item.width = 57;
			Item.height = 57;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ItemRarityID.Pink;

			// Use Properties
			// Note that useTime and useAnimation for this item don't actually affect the behavior because the held projectile handles that. 
			// Each attack takes a different amount of time to execute
			// Conforming to the item useTime and useAnimation makes it much harder to design
			// It does, however, affect the item tooltip, so don't leave it out.
			Item.useTime = 40;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/DamascusSlash") with {
				Volume = 1.0f, 
    			Pitch = 0.0f, 
    			PitchVariance = 0.5f, 
			}; // The sound when the weapon is being used.

			// Weapon Properties
			Item.knockBack = 30;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.autoReuse = true; // This determines whether the weapon has autoswing
			Item.damage = 15; // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = DamageClass.Melee; // Deals melee damage
            Item.crit = 46; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.
			Item.noMelee = true;  // This makes sure the item does not deal damage from the swinging animation
			Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand

			// Projectile Properties
			Item.shoot = ModContent.ProjectileType<DamascusSwing>(); // The sword as a projectile
		}

		
		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override bool CanUseItem(Player player) {
			// STAB AI
			if (player.altFunctionUse == 2) {
				Item.useStyle = ItemUseStyleID.Rapier; // Change to the desired use style
				Item.useTime = 10; // Adjust as needed
				Item.autoReuse = true;
				Item.useAnimation = 10; // Adjust as needed
				Item.shoot = ModContent.ProjectileType<DamascusJab>(); // The projectile is what makes a shortsword work
				Item.shootSpeed = 3.0f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
				Item.DamageType = DamageClass.MeleeNoSpeed;
				Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
				Item.noMelee = false; // The projectile will do the damage and not the item
				Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/AlloySwing") {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; // The sound when the weapon is being used.

				// Launch the player towards the cursor
				Vector2 cursorPosition = Main.MouseWorld;
				Vector2 directionToCursor = (cursorPosition - player.Center).SafeNormalize(Vector2.Zero);
				Vector2 forwardVelocity = directionToCursor * 3.5f; // Adjust the multiplier to control the launch speed
				player.velocity += forwardVelocity;

				// Shoot the projectile (CHECK PASSED)
				if (Main.myPlayer == player.whoAmI) {
					//Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, directionToCursor * Item.shootSpeed, Item.shoot, Item.damage, Item.knockBack, player.whoAmI);
				}
			}
			// SWING AI
			else {
				// Common Properties
				Item.width = 76;
				Item.height = 82;
				Item.value = Item.sellPrice(gold: 2, silver: 50);
				Item.rare = ItemRarityID.Pink;

				// Use Properties
				// Note that useTime and useAnimation for this item don't actually affect the behavior because the held projectile handles that. 
				// Each attack takes a different amount of time to execute
				// Conforming to the item useTime and useAnimation makes it much harder to design
				// It does, however, affect the item tooltip, so don't leave it out.
				Item.useTime = 25;
				Item.useAnimation = 25;
				Item.useStyle = ItemUseStyleID.Shoot;
				// The sound when the weapon is being used.
				Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/DamascusSlash") {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
				// Weapon Properties
				Item.knockBack = 30;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
				Item.autoReuse = true; // This determines whether the weapon has autoswing
				Item.damage = 15; // The damage of your sword, this is dynamically adjusted in the projectile code.
				Item.DamageType = DamageClass.Melee; // Deals melee damage
				Item.crit = 6; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.
				Item.noMelee = true;  // This makes sure the item does not deal damage from the swinging animation
				Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand

				// Projectile Properties
				Item.shoot = ModContent.ProjectileType<DamascusSwing>(); // The sword as a projectile
			}
			return base.CanUseItem(player);
		}

        

		//Hit Inflictions
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            
            
			if (player.altFunctionUse == 2) {
                
				//player.AddBuff(BuffID.RapidHealing, 360);
			}
			else {
				target.AddBuff(BuffID.Bleeding, 600);
			}
		}

	

		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<DamascusSteel>(20)
                .AddIngredient<Steel>(8)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
} 