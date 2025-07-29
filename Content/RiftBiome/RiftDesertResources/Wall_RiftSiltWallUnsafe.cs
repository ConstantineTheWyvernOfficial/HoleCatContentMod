using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RiftBiome.RiftDesertResources
{
	public class Wall_RiftSiltWallUnsafe : ModWall
	{
		public override void SetStaticDefaults() {
			AddMapEntry(new Color(48, 29, 0));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}