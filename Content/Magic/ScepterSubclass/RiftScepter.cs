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
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Rarity;
using Terraria.Localization;
using DestroyerTest.Content.Tools;
using DestroyerTest.Content.Tiles.Riftplate;
using System.Collections.Generic; // Add this line if CT3_Swing is in the Projectiles namespace

namespace DestroyerTest.Content.Magic.ScepterSubclass
{
	public class RiftScepter : ModItem
	{

        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        //Weapon Properties
        public override void SetDefaults() {
			Item.width = 34;
			Item.height = 34;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ModContent.RarityType<RiftRarity1>();
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item25;
			Item.knockBack = 0;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.autoReuse = true; // This determines whether the weapon has autoswing
			Item.damage = 35 + (int)Math.Round(ScepterClassStats.DamageModifier); // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = ModContent.GetInstance<ScepterClass>();
            Item.crit = 10; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.ion
			Item.shoot = ModContent.ProjectileType<RiftLaser>(); // The sword as a projectile
            Item.shootSpeed = 10f;
			Item.noUseGraphic = false; // The sword is actually a "projectile", so the item should not be visible when used
		}

		
		public override bool AltFunctionUse(Player player) {
			return true;
		}

		public override bool CanUseItem(Player player) {
            var modPlayer = Main.LocalPlayer.GetModPlayer<LivingShadowPlayer>();
            if (modPlayer.LivingShadowCurrent == 0)
			{
				return false;
			}
			// THROW AI
			if (player.altFunctionUse == 2) {
				Item.useStyle = ItemUseStyleID.Shoot; // Change to the desired use style
				Item.useTime = 60; // Adjust as needed
				Item.autoReuse = false;
				Item.useAnimation = 60; // Adjust as needed
				Item.damage = 35 + 10 + (int)Math.Round(ScepterClassStats.DamageModifier);
				Item.shoot = ModContent.ProjectileType<RiftScepterThrown>(); // The projectile is what makes a shortsword work
				Item.shootSpeed = 25.0f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
				Item.DamageType = ModContent.GetInstance<ScepterClass>();
				Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
				Item.noMelee = false; // The projectile will do the damage and not the item
				Item.UseSound = SoundID.Item169;
				Item.crit = 46;
			}
			// SHOOT AI
			else {
			Item.width = 34;
			Item.height = 34;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ModContent.RarityType<RiftRarity1>();
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item25;
			Item.damage = 35 + (int)Math.Round(ScepterClassStats.DamageModifier);
			Item.knockBack = 0;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.autoReuse = true; // This determines whether the weapon has autoswing
			Item.DamageType = ModContent.GetInstance<ScepterClass>();
            Item.crit = 10; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.ion
			Item.shoot = ModContent.ProjectileType<RiftLaser>(); // The sword as a projectile
            Item.shootSpeed = 10f;
			Item.noUseGraphic = false; // The sword is actually a "projectile", so the item should not be visible when used
			}
			return base.CanUseItem(player);
			
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

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2) // Throwing mode
			{
				// Find the active instance of the thrown projectile
				foreach (Projectile proj in Main.projectile)
				{
					// Ensure the projectile exists, is owned by the player, and is the right type
					if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<RiftScepterThrown>())
					{
						// Safely get the existenceTimer value
						if (proj.ModProjectile is RiftScepterThrown thrownProj)
						{
							// Set useTime to match the projectile's lifetime
							Item.useTime = thrownProj.existenceTimer;
						}
						break; // Stop searching once we find the right projectile
					}
				}

				return true;
			}

			return base.UseItem(player);
		}




		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Check if another projectile of the same type is active
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<RiftScepterThrown>())
				{
					return false; // Prevent new projectile from being fired
				}
			}

			return true; // Allow firing if no other projectiles exist
		}


        
		public override void AddRecipes() {
			CreateRecipe()
                .AddCondition(Language.GetText("Mods.DestroyerTest.RecipeCondition.Charge"), () => Main.LocalPlayer.HasItem(ModContent.ItemType<Husk_RiftScepter>()))
				.Register();
		}

        public override void UpdateInventory(Player player)
        {
            NoChargeLeft(player);
            
        }

		public override bool CanRightClick() {
                return true;
            }

        private const int CrucibleProximityRange = 3;
        private const int RequiredBatteries = 8;

        public override void RightClick(Player player)
        {
            SoundStyle zapSound = new SoundStyle("DestroyerTest/Assets/Audio/RiftCharge");

            if (player.HeldItem != null && player.HeldItem.type == ModContent.ItemType<RiftElectrifier>())
            {
                if (IsNearRiftCrucible(player))
                {
                    ReplenishLivingShadow(player, zapSound, consumeBatteries: false);
                }
                else if (player.CountItem(ModContent.ItemType<RiftBattery>(), RequiredBatteries) >= RequiredBatteries)
                {
                    ConsumeBatteries(player, RequiredBatteries);
                    ReplenishLivingShadow(player, zapSound, consumeBatteries: true);
                }
                else
                {
                    CombatText.NewText(player.Hitbox, ColorLib.Rift, $"{RequiredBatteries} Rift Batteries needed, or, plug the Electrifier into a rift crucible.", true);
                }
            }
			else
			{
				return;
			}
        }

        public override bool ConsumeItem(Player player)
        {
            return false; // Prevents the item from being consumed on use
        }

        private bool IsNearRiftCrucible(Player player)
        {
            Point playerTilePosition = player.Center.ToTileCoordinates();
            int riftCrucibleTileType = ModContent.TileType<Tile_RiftCrucible>();

            for (int x = -CrucibleProximityRange; x <= CrucibleProximityRange; x++)
            {
                for (int y = -CrucibleProximityRange; y <= CrucibleProximityRange; y++)
                {
                    Point checkPosition = new Point(playerTilePosition.X + x, playerTilePosition.Y + y);
                    if (Main.tile[checkPosition.X, checkPosition.Y].TileType == riftCrucibleTileType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ConsumeBatteries(Player player, int count)
        {
            int riftBatteryType = ModContent.ItemType<RiftBattery>();
            for (int i = 0; i < count; i++)
            {
                player.ConsumeItem(riftBatteryType);
            }
        }

        private void ReplenishLivingShadow(Player player, SoundStyle zapSound, bool consumeBatteries)
        {
            
            SoundEngine.PlaySound(zapSound, player.position);
            ScreenFlashSystem.FlashIntensity = 0.9f;

            var modPlayer = player.GetModPlayer<LivingShadowPlayer>();
            modPlayer.LivingShadowCurrent = modPlayer.LivingShadowMax2;
        }

        private bool hasReplaced = false;
        private void NoChargeLeft(Player player)
        {
            var modPlayer = player.GetModPlayer<LivingShadowPlayer>();
            if (modPlayer.LivingShadowCurrent == 0 && hasReplaced == false)
			{
            hasReplaced = true;
			Item.NewItem(player.GetSource_FromThis(), player.position, ModContent.ItemType<Husk_RiftScepter>());
            Item.TurnToAir();
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string batteryTooltip = $"Requires {RequiredBatteries} Rift Batteries to recharge.";
			tooltips.Add(new TooltipLine(Mod, "RiftBatteryRequirement", batteryTooltip)
			{
				OverrideColor = ColorLib.Rift // Optional: Set a custom color for the tooltip text
			});
		}
	}
} 