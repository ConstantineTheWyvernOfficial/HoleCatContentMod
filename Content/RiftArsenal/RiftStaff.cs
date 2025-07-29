using System.Collections.Generic;
using DestroyerTest.Common;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.RiftArsenalNoCharge;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Tools;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RiftArsenal
{
	// ExampleStaff is a typical staff. Staffs and other shooting weapons are very similar, this example serves mainly to show what makes staffs unique from other items.
	// Staff sprites, by convention, are angled to point up and to the right. "Item.staff[Type] = true;" is essential for correctly drawing staffs.
	// Staffs use mana and shoot a specific projectile instead of using ammo. Item.DefaultToStaff takes care of that.
	public class RiftStaff : ModItem
	{
        public override string Texture => "DestroyerTest/Content/RiftArsenal/RiftStaff";
		public override void SetStaticDefaults() {
		}

		public override void SetDefaults() {
			// DefaultToStaff handles setting various Item values that magic staff weapons use.
			// Hover over DefaultToStaff in Visual Studio to read the documentation!
			Item.shoot = ModContent.ProjectileType<Rift_Bullet>();
            Item.shootSpeed = 16f;
            Item.useTime = 30;
            Item.useAnimation = 30;
			Item.width = 18;
			Item.height = 50;
			Item.autoReuse = true;
			Item.crit = 12;
			Item.rare = ItemRarityID.Green;
			Item.useStyle = ItemUseStyleID.Shoot;

			// Customize the UseSound. DefaultToStaff sets UseSound to SoundID.Item43, but we want SoundID.Item2.
			Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/Rift_Katana_Hold");

			Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.damage = 110;

			// Set rarity and value
			Item.SetShopValues(ItemRarityColor.Green2, 10000);
		}

		public override bool CanUseItem(Player player)
        {
			var modPlayer = Main.LocalPlayer.GetModPlayer<LivingShadowPlayer>();
            if (modPlayer.LivingShadowCurrent == 0)
			{
				return false;
			}
			return base.CanUseItem(player);
        }
		
		public override void AddRecipes() {
			CreateRecipe()
                .AddCondition(Language.GetText("Mods.DestroyerTest.RecipeCondition.Charge"), () => Main.LocalPlayer.HasItem(ModContent.ItemType<Husk_RiftStaff>()))
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
        private const int RequiredBatteries = 6;

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
                    CombatText.NewText(player.Hitbox, ColorLib.Rift, "Six Rift Batteries needed, or, plug the Electrifier into a rift crucible.", true);
                }
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
            Item.NewItem(player.GetSource_FromThis(), player.position, ModContent.ItemType<Husk_RiftStaff>());
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