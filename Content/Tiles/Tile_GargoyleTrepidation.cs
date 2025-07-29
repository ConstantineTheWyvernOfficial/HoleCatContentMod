using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DestroyerTest.Content.Tiles
{
	// ExampleStatue shows off correctly using wiring to spawn items and NPC.
	// See StatueWorldGen to see how ExampleStatue is added as an option for naturally spawning statues during worldgen.
	public class Tile_GargoyleTrepidation : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.addTile(Type);

			DustType = DustID.Stone;

			AddMapEntry(new Color(144, 148, 144), Language.GetText("MapObject.Statue"));
		}
        public bool playerRange() {
            Player player = Main.LocalPlayer;
            Vector2 position = new Vector2(0, 0);
            if (player.Distance(position) < 60) {
				player.AddBuff(BuffID.Dangersense, -1, true);
                return true;
            }
            else {
                player.ClearBuff(BuffID.Dangersense);
            }
            return true;
        }
	}
}