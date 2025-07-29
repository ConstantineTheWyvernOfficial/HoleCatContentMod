
using DestroyerTest.Common;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tools;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DestroyerTest.Content.Tiles.Riftplate
{
	public class Tile_RiftCrucible : ModTile
	{
		public override void SetStaticDefaults() {
            HitSound = SoundID.Item178;
			// Properties
			Main.tileTable[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

			DustType = DustID.Lava;
			AdjTiles = new int[] { TileID.WorkBenches + TileID.LihzahrdFurnace };

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
			TileObjectData.addTile(Type);

			// Etc
			AddMapEntry(new Color(153, 92, 0), Language.GetText("RiftCrucible"));
		}

		public override void NumDust(int x, int y, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public void ElectrifierUIAura(Player player)
		{
			Projectile.NewProjectileDirect(player.GetSource_FromThis(), new Vector2(80 / 2, 64 / 2), new Vector2(0, 0), ModContent.ProjectileType<ZoneRingDrawEntity>(), 0, 0);
		}

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
			Player player = Main.LocalPlayer;
            if (player.HeldItem.type == ModContent.ItemType<RiftElectrifier>())
			{
				ElectrifierUIAura(player);
			}
        }
				
	}
}
