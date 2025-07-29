using DestroyerTest.Content.MetallurgySeries;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DestroyerTest.Content.Tiles
{
	public class Tile_TemperingStation : ModTile
	{
		public override void SetStaticDefaults() {
			// Properties
			Main.tileTable[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

			DustType = DustID.WoodFurniture;
			AdjTiles = new int[] { TileID.Anvils};

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16];
			TileObjectData.addTile(Type);


			// Etc
			AddMapEntry(new Color(70, 31, 31), Language.GetText("TemperingStation"));
		}

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
            {
                return true;
            }

            public override void NumDust(int i, int j, bool fail, ref int num)
            {
                num = 1;
            }

            public override void MouseOver(int i, int j)
            {
                Player player = Main.LocalPlayer;
                player.noThrow = 2;
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = ItemID.IronHammer;
            }
	}
}