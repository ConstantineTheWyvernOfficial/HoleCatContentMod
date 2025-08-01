  
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;
using Terraria.Audio;
using DestroyerTest.Common;
using DestroyerTest.Content.RiftArsenal;
using DestroyerTest.Content.Tools;
using System.Collections.Generic;

namespace DestroyerTest.Content.RiftArsenalNoCharge
{

	public class Husk_RiftZapinator : ModItem
	{
		public override void SetDefaults() {

			Item.width = 64;
			Item.height = 64;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ModContent.RarityType<RiftRarity1>();

			
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.damage = 0;
		}

        public override bool CanUseItem(Player player)
        {
            return false;
        }

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_Riftplate>(15)
				.AddTile<Tile_RiftCrucible>()
				.Register();
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
            Item.NewItem(player.GetSource_OpenItem(Type), player.position, ModContent.ItemType<RiftZapinator>());
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