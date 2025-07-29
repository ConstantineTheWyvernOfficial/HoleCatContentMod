
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	public class Wall_RiftplateWall : ModWall
	{
		public override void SetStaticDefaults() {
            HitSound = SoundID.Item37;
			DustType = DustID.Wraith;
			Main.wallHouse[Type] = true;

			AddMapEntry(new Color(12, 12, 12));
		}
        public void HitWire(int i, int j) {
        Tile tile = Main.tile[i, j];
            ushort newWallType = (tile.WallType == ModContent.WallType<Wall_RiftplateWall>()) ? (ushort)ModContent.WallType<Wall_RiftplateActiveWall>() : (ushort)ModContent.WallType<Wall_RiftplateWall>();

            // Change the tile type
            tile.TileType = newWallType;

            SoundEngine.PlaySound(SoundID.Item30);

            // Skip further wire processing for this tile
            Wiring.SkipWire(i, j);

            // Avoid trying to send packets in singleplayer
            if (Main.netMode != NetmodeID.SinglePlayer) {
                NetMessage.SendTileSquare(-1, i, j, 1, TileChangeType.None);
            }

            // Update the tile
            WorldGen.SquareTileFrame(i, j, true);
        }
	}
}