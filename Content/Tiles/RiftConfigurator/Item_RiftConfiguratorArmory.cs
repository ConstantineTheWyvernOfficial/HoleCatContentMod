using DestroyerTest.Content.Resources;
using DestroyerTest.Common;
using DestroyerTest.Rarity;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.RiftConfigurator
{
	public class Item_RiftConfiguratorArmory : ModItem
	{
		public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tile_RiftConfiguratorArmory>());
            Item.height = 44;
            Item.width = 38;
            Item.value = 6000;
            Item.rare = ModContent.RarityType<RiftRarity1>();
		}

            public override bool CanRightClick() {
                return true;
            }

            public override void RightClick(Player player)
            {
                  SoundStyle InsertDrive = new SoundStyle("DestroyerTest/Assets/Audio/InsertDrive");
                  if (player.HeldItem.IsAir)
                  {
                  Item.TurnToAir();
                  Item.NewItem(player.GetSource_OpenItem(Type), player.position, ModContent.ItemType<Item_RiftConfiguratorCore>());
                  Item.NewItem(player.GetSource_OpenItem(Type), player.position, ModContent.ItemType<ArmoryDrive>());
                  SoundEngine.PlaySound(InsertDrive, player.position);
                  Main.NewText("Drive Uninstalled", ColorLib.DarkRift2);
                  }
            }
	}
}
