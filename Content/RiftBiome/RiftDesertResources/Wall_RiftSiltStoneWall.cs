
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RiftBiome.RiftDesertResources
{
	public class Wall_RiftSiltStoneWall : ModWall
	{
		public override void SetStaticDefaults() {
            HitSound = SoundID.Item178;
			DustType = DustID.Lava;
			Main.wallHouse[Type] = false;

			AddMapEntry(new Color(102, 61, 0));
        }
	}
}
