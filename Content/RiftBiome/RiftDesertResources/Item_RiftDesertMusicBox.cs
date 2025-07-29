using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.RiftBiome.RiftDesertResources;

namespace DestroyerTest.Content.RiftBiome.RiftDesertResources
{
	public class Item_RiftDesertMusicBox : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false; // music boxes can't get prefixes in vanilla
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox; // recorded music boxes transform into the basic form in shimmer

			// The following code links the music box's item and tile with a music track:
			//   When music with the given ID is playing, equipped music boxes have a chance to change their id to the given item type.
			//   When an item with the given item type is equipped, it will play the music that has musicSlot as its ID.
			//   When a tile with the given type and Y-frame is nearby, if its X-frame is >= 36, it will play the music that has musicSlot as its ID.
			// When getting the music slot, you should not add the file extensions!
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/V1_Title"), ModContent.ItemType<Item_RiftDesertMusicBox>(), ModContent.TileType<Tile_RiftDesertMusicBox>());
		}

		public override void SetDefaults() {
			Item.DefaultToMusicBox(ModContent.TileType<Tile_RiftDesertMusicBox>(), 0);
		}

        public override void AddRecipes()
        {
            CreateRecipe()
				.AddIngredient(ItemID.MusicBox)
				.AddIngredient<Item_RiftSilt>(10)
                .AddIngredient(ItemID.SoulofNight, 5)
				.AddTile<Tile_RiftConfiguratorCore>()
				.Register();
        }
    }
}