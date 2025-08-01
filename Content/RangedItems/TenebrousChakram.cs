using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Common;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tools;
using System.Collections.Generic;
using DestroyerTest.Content.RiftArsenalNoCharge;
using Terraria.Localization;
  

namespace DestroyerTest.Content.RangedItems
{
	public class TenebrousChakram : ModItem
	{

        public override void SetStaticDefaults()
        {
        }
        //Weapon Properties
        public override void SetDefaults() {
			Item.width = 54;
			Item.height = 64;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ModContent.RarityType<ShimmeringRarity>();
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item169;
			Item.knockBack = 0;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.autoReuse = true; // This determines whether the weapon has autoswing
			Item.damage = 45; // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = DamageClass.Ranged;
            Item.crit = 30; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.ion
			Item.shoot = ModContent.ProjectileType<TenebrousChakramThrown>(); // The sword as a projectile
            Item.shootSpeed = 20f;
			Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
		}

		public override void UseItemFrame(Player player)
		{
				float animationSpeed = 8.0f; // You can modify this to change the animation speed.

				// Calculate the progress, but limit it to a max of 1.0
				float progress = ((player.itemAnimationMax - player.itemAnimation) / (float)player.itemAnimationMax);
				progress = Math.Min(progress * animationSpeed, 1.0f); // Clamps progress to a max of 1

				// Start angle at 180 degrees (upwards)
				float startAngle = MathHelper.ToRadians(180f);

				// Declare endAngle here to ensure it's accessible outside of the if blocks
				float endAngle;

				// Set the end angle based on player direction
				if (player.direction == 1)
				{
					endAngle = MathHelper.ToRadians(270f); // Right side, end angle 270
				}
				else if (player.direction == -1)
				{
					endAngle = MathHelper.ToRadians(90f); // Left side, end angle 90
				}
				else
				{
					endAngle = startAngle; // Default case (shouldn't happen unless player.direction is unexpected)
				}

				// Interpolate between start and end angle
				float armRotation = MathHelper.Lerp(startAngle, endAngle, progress);

				// If the progress has reached the end, stop the arm from rotating further
				if (progress == 1.0f)
				{
					// Ensure the arm stays at the final angle and doesn't continue animating
					armRotation = endAngle;
				}

				// Apply the final rotation to the player's arm
				player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRotation);
		}
        




		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Check if another projectile of the same type is active
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<TenebrousChakramThrown>())
				{
					return false; // Prevent new projectile from being fired
				}
			}

			return true; // Allow firing if no other projectiles exist
		}

        public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<Tenebris>(12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
    }
} 