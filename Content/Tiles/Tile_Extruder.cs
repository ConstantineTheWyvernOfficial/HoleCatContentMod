using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Resources;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DestroyerTest.Content.Tiles
{
	public class Tile_Extruder : ModTile
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

			DustType = DustID.Iron;

			// Placement
			TileObjectData.newTile = new TileObjectData(copyFrom:TileObjectData.Style1x1); // Create a new instance
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            // Additional edits here, such as lava immunity, alternate placements, and subtiles
            TileObjectData.addTile(Type);


			// Etc
			AddMapEntry(new Color(70, 31, 31), Language.GetText("Extruder"));
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
                player.cursorItemIconID = ModContent.ItemType<CunifeWire>();
            }
	}
}