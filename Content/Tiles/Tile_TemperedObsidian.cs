using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles
{
	public class Tile_TemperedObsidian : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
            HitSound = new Terraria.Audio.SoundStyle("DestroyerTest/Assets/Audio/TO_Break") with
            {
            PitchVariance = 0.5f
            };
			DustType = DustID.Obsidian;
            MineResist = 3.6f;

			AddMapEntry(new Color(9, 10, 13));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}