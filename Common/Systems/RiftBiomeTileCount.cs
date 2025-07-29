using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.RiftBiome.RiftSurfaceResources;
using DestroyerTest.Content.RiftBiome.RiftDesertResources;
using DestroyerTest.Content.RiftBiome;
using System;
using Terraria.ModLoader;

namespace DestroyerTest.Common.Systems
{
    public class RiftSurfaceTileCount : ModSystem
    {
        public int RiftSurfaceBlockCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
            RiftSurfaceBlockCount =
                tileCounts[ModContent.WallType<Wall_RiftWall>()] +
                tileCounts[ModContent.TileType<Tile_RiftDirt>()] +
                tileCounts[ModContent.TileType<Tile_RiftStone>()];
        }
    }

    public class RiftDesertTileCount : ModSystem
    {
        public int RiftDesertBlockCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
            RiftDesertBlockCount =
                tileCounts[ModContent.TileType<Tile_RiftSiltStone>()] +
                tileCounts[ModContent.WallType<Wall_RiftSiltStoneWall>()] +
                tileCounts[ModContent.TileType<Tile_HardenedRiftSilt>()] +
                tileCounts[ModContent.TileType<Tile_RiftSilt>()];
        }
    }
}
