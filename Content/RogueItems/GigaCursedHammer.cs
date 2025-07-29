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
using DestroyerTest.Rarity; // Add this line if CT3_Swing is in the Projectiles namespace

namespace DestroyerTest.Content.RogueItems
{
	public class GigaCursedHammerWeapon : ModItem
	{

        public override void SetStaticDefaults()
        {
        }
        //Weapon Properties
        public override void SetDefaults() {
			Item.width = 164;
			Item.height = 164;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ModContent.RarityType<CorruptionSpecialRarity>();
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
			Item.knockBack = 6;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.autoReuse = true; // This determines whether the weapon has autoswing
			Item.damage = 140; // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = DamageClass.Ranged;
            Item.crit = 10; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.ion
			Item.shoot = ModContent.ProjectileType<GigaCursedHammerThrown>(); // The sword as a projectile
            Item.shootSpeed = 40f;
			Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
		}
		
		public override void UseItemFrame(Player player)
		{
			if (player.altFunctionUse == 2) // Throwing mode
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
		}
    }
} 