using DestroyerTest.Content.Resources;
using DestroyerTest.Common;
using DestroyerTest.Rarity;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Tools;
using DestroyerTest.Content.RiftArsenal;
using Microsoft.Xna.Framework;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using System.Collections.Generic;

namespace DestroyerTest.Content.Magic.ScepterSubclass
{
	public class Husk_RiftScepter : ModItem
	{
		public override void SetDefaults() {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.damage = 35;
            Item.knockBack = 0;
            Item.crit = 6;
            Item.DamageType = DamageClass.Melee;
            Item.height = 38;
            Item.width = 38;
            Item.value = 600;
            Item.rare = ItemRarityID.Gray; 
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
		}
        
		public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<Living_Shadow>(50)
                .AddIngredient<ShadowCircuitry>(2)
                .AddTile<Tile_RiftConfiguratorWeaponry>()
				.Register();
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
                    TransformToRiftBroadsword(player, zapSound, consumeBatteries: false);
                }
                else if (player.CountItem(ModContent.ItemType<RiftBattery>(), RequiredBatteries) >= RequiredBatteries)
                {
                    ConsumeBatteries(player, RequiredBatteries);
                    TransformToRiftBroadsword(player, zapSound, consumeBatteries: true);
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

        private void TransformToRiftBroadsword(Player player, SoundStyle zapSound, bool consumeBatteries)
        {
            Item.NewItem(player.GetSource_OpenItem(Type), player.position, ModContent.ItemType<RiftScepter>());
            Item.TurnToAir();
            SoundEngine.PlaySound(zapSound, player.position);
            ScreenFlashSystem.FlashIntensity = 0.9f;

            var modPlayer = player.GetModPlayer<LivingShadowPlayer>();
            modPlayer.LivingShadowCurrent = modPlayer.LivingShadowMax2;
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
