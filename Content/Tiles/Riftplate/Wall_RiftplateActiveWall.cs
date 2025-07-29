
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	public class Wall_RiftplateActiveWall : ModWall
	{
		public override void SetStaticDefaults() {
            HitSound = SoundID.Item37;
			DustType = DustID.Lava;
			Main.wallHouse[Type] = true;

			AddMapEntry(new Color(12, 12, 12));
        }
            public override bool Drop(int i, int j, ref int type)
            {
                int itemType = ModContent.ItemType<Item_RiftplateWall>();
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, itemType);
                return true;
            }
	}
}
