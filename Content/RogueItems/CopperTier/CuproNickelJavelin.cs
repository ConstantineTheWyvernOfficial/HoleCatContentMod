using System;
using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RogueItems.CopperTier
{
    
	public class CuproNickelJavelin : ModItem
	{
        public static readonly SoundStyle Throw = new SoundStyle("DestroyerTest/Assets/Audio/MCTrident", 2) {
        Volume = 1.0f,
        PitchVariance = 0.2f,
        MaxInstances = 3
        };
		public override void SetDefaults() {
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools
            Item.width = 52;
            Item.height = 52;
			// Common Properties
			Item.rare = ModContent.RarityType<MetallurgyRarity>();
			Item.value = Item.sellPrice(silver: 5);
			Item.maxStack = 1;
			// Use Properties
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.UseSound = Throw;
			Item.autoReuse = false;

			// Weapon Properties			
			Item.damage = 15;
			Item.knockBack = 5f;
			Item.noUseGraphic = true; // The item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item
			Item.DamageType = DamageClass.Ranged;
            Item.noUseGraphic = true;

			// Projectile Properties
			Item.shootSpeed = 16f;
			Item.shoot = ModContent.ProjectileType<CuproNickelJavelinThrown>(); // The projectile that will be thrown
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

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<CuproNickel>(16)
                .AddIngredient(ItemID.Wood, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}