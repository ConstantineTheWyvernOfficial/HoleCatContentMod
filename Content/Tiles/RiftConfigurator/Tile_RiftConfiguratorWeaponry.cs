using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DestroyerTest.Content.Tiles.RiftConfigurator
{
	public class Tile_RiftConfiguratorWeaponry : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(255, 155, 0), name);


			AnimationFrameHeight = 54;
		}

        

        public override void AnimateTile(ref int frame, ref int frameCounter) {

            if (++frameCounter >= 8) {
                frameCounter = 0;
                if (++frame >= 4) {
                    frame = 0;
                }
            }
        }

        /*
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {

            Tile tile = Main.tile[i, j];
            frameYOffset = 54; //Keep this to draw correctly!!!
        }
        */

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {
            Tile tile = Main.tile[i, j];
            frameYOffset = (tile.TileFrameY / AnimationFrameHeight) * AnimationFrameHeight;
        }


		public override bool KillSound(int i, int j, bool fail) {
				SoundEngine.PlaySound(SoundID.Item178, new Vector2(i, j).ToWorldCoordinates());
                return true;

		}
	}
}