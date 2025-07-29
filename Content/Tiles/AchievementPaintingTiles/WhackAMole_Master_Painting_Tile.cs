using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DestroyerTest.Content.Tiles.AchievementPaintingTiles
{
	// This class shows off many things common to Lamp tiles in Terraria. The process for creating this example is detailed in: https://github.com/tModLoader/tModLoader/wiki/Advanced-Vanilla-Code-Adaption#examplelamp-tile
	// If you can't figure out how to recreate a vanilla tile, see that guide for instructions on how to figure it out yourself.
	internal class WhackAMole_Master_Painting_Tile : ModTile
	{
		public override void SetStaticDefaults() {
			// Properties
            Main.tileShine[Type] = 500;
			Main.tileLighted[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileWaterDeath[Type] = true;
			Main.tileLavaDeath[Type] = true;
			// Main.tileFlame[Type] = true; // Main.tileFlame is only useful for vanilla tiles. Modded tiles can manually draw flames in PostDraw.


            TileObjectData.newTile = new TileObjectData(copyFrom:TileObjectData.Style1x1); // Create a new instance

			TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16];
			TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true; // Ensure no default placement rules interfere
            TileObjectData.newTile.AnchorValidTiles = new int[] { }; // Empty array, meaning no specific floor needed
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.addTile(Type);

			// Etc
			AddMapEntry(new Color(57, 39, 29), Language.GetText("Achievement Painting"));
		}

	}
}