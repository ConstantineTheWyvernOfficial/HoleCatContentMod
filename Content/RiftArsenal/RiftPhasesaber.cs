using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Projectiles;
using System;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Common;
using System.Collections.Generic;
using DestroyerTest.Content.Tools;
using DestroyerTest.Content.RiftArsenalNoCharge;

namespace DestroyerTest.Content.RiftArsenal
{
    public class RiftPhasesaber : ModItem
    {
        private bool isThrowingMode = false; // Tracks the current mode

        public override void SetDefaults() {
            SetSwingModeDefaults(); // Initialize with swing mode defaults
        }

        private void SetSwingModeDefaults() {
            Item.width = 80;
            Item.height = 80;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 70;
            Item.knockBack = 12;
            Item.crit = 16;
            Item.value = Item.buyPrice(gold: 16);
            Item.rare = ModContent.RarityType<RiftRarity1>();
            Item.UseSound = SoundID.Item71;
            Item.noMelee = false;
            Item.noUseGraphic = false;
            Item.shoot = ProjectileID.None;
            Item.shootSpeed = 0f;
        }

        private void SetThrowingModeDefaults() {
            Item.width = 66;
            Item.height = 66;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 70;
            Item.useAnimation = 70;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 180;
            Item.knockBack = 30;
            Item.crit = 46;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item169;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<RiftPhasesaberThrown>();
            Item.shootSpeed = 20f;
        }

        public override bool CanUseItem(Player player) {
            if (isThrowingMode) {
                SetThrowingModeDefaults();
            } else {
                SetSwingModeDefaults();
            }

            var modPlayer = Main.LocalPlayer.GetModPlayer<LivingShadowPlayer>();
            if (modPlayer.LivingShadowCurrent == 0)
			{
				return false;
			}
            return base.CanUseItem(player);
        }

        public override bool AltFunctionUse(Player player) {
            // Allow alternate function use to toggle modes
            return true;
        }

        public override bool? UseItem(Player player) {
            if (player.altFunctionUse == 2) { // Check if the alternate function is being used
                isThrowingMode = !isThrowingMode; // Toggle the mode
                SoundEngine.PlaySound(SoundID.Item35); // Play a sound to indicate the mode change
                if (isThrowingMode) {
                    Main.NewText("Switched to Throwing Mode", Color.Orange);
                } else {
                    Main.NewText("Switched to Swing Mode", Color.LightGreen);
                }
                return true; // Indicate that the alternate function was used
            }
            return base.UseItem(player);
        }

        public override void UseItemFrame(Player player) {
            if (isThrowingMode) {
                float animationSpeed = 8.0f;
                float progress = ((player.itemAnimationMax - player.itemAnimation) / (float)player.itemAnimationMax);
                progress = Math.Min(progress * animationSpeed, 1.0f);
                float startAngle = MathHelper.ToRadians(180f);
                float endAngle = player.direction == 1 ? MathHelper.ToRadians(270f) : MathHelper.ToRadians(90f);
                float armRotation = MathHelper.Lerp(startAngle, endAngle, progress);
                if (progress == 1.0f) {
                    armRotation = endAngle;
                }
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRotation);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(3)) {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.RiftDust>());
            }
        }

        public override void AddRecipes() {
			CreateRecipe()
                .AddCondition(Language.GetText("Mods.DestroyerTest.RecipeCondition.Charge"), () => Main.LocalPlayer.HasItem(ModContent.ItemType<Husk_RiftPhasesaber>()))
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
        private const int RequiredBatteries = 2;

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
            Item.NewItem(player.GetSource_FromThis(), player.position, ModContent.ItemType<Husk_RiftPhasesaber>());
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