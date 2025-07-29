using DestroyerTest.Content.RiftBiome;
using DestroyerTest.Content.RiftBiome.RiftDesertResources;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RiftBiome.RiftSurfaceResources
{
	public class Tile_RiftDirt : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
			Main.tileMerge[ModContent.TileType<Tile_RiftStone>()][Type] = true;
			Main.tileMerge[ModContent.TileType<Tile_RiftSilt>()][Type] = true;
			
			Main.tileBlockLight[Type] = true;

			DustType = DustID.Wraith;

			AddMapEntry(new Color(15, 15, 15));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight) {
			//We use this method to set the merge values of the adjacent tiles to -2 if the tile nearby is a snow block
			//-2 is what terraria uses to designate the tiles that will merge with ours using the custom frames
			WorldGen.TileMergeAttempt(-2, ModContent.TileType<Tile_RiftStone>(), ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
			WorldGen.TileMergeAttempt(-2, ModContent.TileType<Tile_RiftSilt>(), ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
		}

		public override void ChangeWaterfallStyle(ref int style) {
			style = ModContent.GetInstance<RiftWaterfallStyle>().Slot;
		}
	}
}