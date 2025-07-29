using DestroyerTest.Content.Resources;
using DestroyerTest.Common;
using DestroyerTest.Rarity;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles.Riftplate;

namespace DestroyerTest.Content.Tiles.RiftConfigurator
{
	public class Item_RiftConfiguratorCore : ModItem
	{
		public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tile_RiftConfiguratorCore>());
            Item.height = 44;
            Item.width = 38;
            Item.value = 6000;
            Item.rare = ModContent.RarityType<RiftRarity1>(); 
		}
        
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Wire, 16)
                .AddIngredient<Motherboard>(6)
                .AddIngredient<ShadowCircuitry>(8)
                .AddIngredient<Item_Riftplate>(24)
				.AddTile(TileID.HeavyWorkBench)
				.Register();
		}

        public override bool CanRightClick() {
                return true;
            }

        public override void RightClick(Player player)
        {
            SoundStyle InsertDrive = new SoundStyle("DestroyerTest/Assets/Audio/InsertDrive");
            if (player.HeldItem.type == ModContent.ItemType<ArmoryDrive>())
            {
                player.HeldItem.TurnToAir();
                Item.NewItem(player.GetSource_OpenItem(Type), player.position, ModContent.ItemType<Item_RiftConfiguratorArmory>());
                SoundEngine.PlaySound(InsertDrive, player.position);
                Main.NewText("Armory Drive Installed", ColorLib.Rift);
            }
            if (player.HeldItem.type == ModContent.ItemType<FurnitureDrive>())
            {
                player.HeldItem.TurnToAir();
                Item.NewItem(player.GetSource_OpenItem(Type), player.position, ModContent.ItemType<Item_RiftConfiguratorFurniture>());
                SoundEngine.PlaySound(InsertDrive, player.position);
                Main.NewText("Furniture Drive Installed", ColorLib.Rift);
            }
            if (player.HeldItem.type == ModContent.ItemType<ToolDrive>())
            {
                player.HeldItem.TurnToAir();
                Item.NewItem(player.GetSource_OpenItem(Type), player.position, ModContent.ItemType<Item_RiftConfiguratorTools>());
                SoundEngine.PlaySound(InsertDrive, player.position);
                Main.NewText("Tools Drive Installed", ColorLib.Rift);
            }
            if (player.HeldItem.type == ModContent.ItemType<WeaponryDrive>())
            {
                player.HeldItem.TurnToAir();
                Item.NewItem(player.GetSource_OpenItem(Type), player.position, ModContent.ItemType<Item_RiftConfiguratorWeaponry>());
                SoundEngine.PlaySound(InsertDrive, player.position);
                Main.NewText("Weaponry Drive Installed", ColorLib.Rift);
            }
        }
	}
}
