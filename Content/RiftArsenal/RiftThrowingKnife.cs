using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.RogueItems;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.RiftArsenalNoCharge;
using Terraria.Localization;
using Terraria.Audio;
using DestroyerTest.Common;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DestroyerTest.Content.Tools;

namespace DestroyerTest.Content.RiftArsenal
{
	public class RiftThrowingKnife : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.ItemsThatCountAsBombsForDemolitionistToSpawn[Type] = true;
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 30f;
			Item.shoot = ModContent.ProjectileType<Projectiles.RiftThrowingKnifeProjectile>();
			Item.width = 14;
			Item.height = 34;
			Item.maxStack = 1;
			Item.consumable = false;
			Item.UseSound = SoundID.Item71;
			Item.useAnimation = 8;
			Item.useTime = 8;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = Item.buyPrice(0, 0, 20, 0);
			Item.rare = ModContent.RarityType<RiftRarity2>();
			Item.damage = 60;
			Item.autoReuse = true;
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
                .AddCondition(Language.GetText("Mods.DestroyerTest.RecipeCondition.Charge"), () => Main.LocalPlayer.HasItem(ModContent.ItemType<Husk_RiftThrowingKnife>()))
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
            Item.NewItem(player.GetSource_FromThis(), player.position, ModContent.ItemType<Husk_RiftThrowingKnife>());
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