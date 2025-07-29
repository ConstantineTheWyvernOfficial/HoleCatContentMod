using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DestroyerTest.Content.Tiles
{
	public class BarrierLatticeR : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileShine[Type] = 500;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile = new TileObjectData(copyFrom:TileObjectData.Style1x1); // Create a new instance
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16];
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            // Additional edits here, such as lava immunity, alternate placements, and subtiles
            TileObjectData.addTile(Type);


			AddMapEntry(new Color(100, 100, 100), Language.GetText("Barrier")); 
		}
	}
    public class BarrierLatticeL : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileShine[Type] = 500;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile = new TileObjectData(copyFrom:TileObjectData.Style1x1); // Create a new instance
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16];
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            // Additional edits here, such as lava immunity, alternate placements, and subtiles
            TileObjectData.addTile(Type);


			AddMapEntry(new Color(100, 100, 100), Language.GetText("Barrier")); 
		}
	}
}