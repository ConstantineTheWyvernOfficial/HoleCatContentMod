/*
using DestroyerTest.SwordLineage;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using DestroyerTest.Projectiles;

namespace DestroyerTest.Tiles
{
    public class DestroyerTestPlayer : ModPlayer
    {
        public int DashResourceCurrent { get; internal set; }

        public void OnKill(Player player, double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            // Original chance is meant to be 1/1000, but as per suggestion, it has been set to 100% chance while still being a random check.
            //if (Main.rand.Next() == 0)
            {
                Main.NewText("Gracestone Replaced");
                // Find the gravestone tile
                int gravestoneType = GetGravestoneType();
                if (gravestoneType != -1)
                {
                    // Replace the gravestone with the custom tile
                    ReplaceGravestoneWithCustomTile(gravestoneType);
                    CheckTilePlacement((int)(Player.position.X / 16f), (int)(Player.position.Y / 16f));
                }
            }
        }

        private int GetGravestoneType()
        {
            // List of gravestone types
            int[] gravestoneTypes = new int[]
            {
                TileID.Tombstones
            };

            // Find the gravestone type at the player's death position
            int x = (int)(Player.position.X / 16f);
            int y = (int)(Player.position.Y / 16f);
            Tile tile = Main.tile[x, y];
            if (tile != null && Array.Exists(gravestoneTypes, type => type == tile.TileType))
            {
                return tile.TileType;
            }

            return -1;
        }

        private void ReplaceGravestoneWithCustomTile(int gravestoneType)
        {
            int x = (int)(Player.position.X / 16f);
            int y = (int)(Player.position.Y / 16f);

            // Replace the gravestone with the custom tile
            Projectile.NewProjectile(x, y, ModContent.ProjectileType<Memoriam_Grave_Projectile>(), true, true);

            // Check if the tile was placed correctly
            CheckTilePlacement(x, y);
        }

        private void CheckTilePlacement(int x, int y)
        {
            // Check if the tile was placed correctly
            if (Main.tile[x, y].TileType != ModContent.TileType<Memoriam_Grave_Placed>())
            {
                Main.NewText("Failed to place Memoriam Gravestone at ("+ x +", "+ y +").");
                // Log a message if the tile was not placed correctly
                ModContent.GetInstance<DestroyerTest>().Logger.Warn("Failed to place Memoriam_Grave_Placed tile at (" + x + ", " + y + ").");
            }
        }
    }

    public class Memoriam_Grave_Placed : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoSunLight[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            RegisterItemDrop(ModContent.ItemType<Memoriam>(), 0);

            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.Grave"));

            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 1);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(0, 2);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 0);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 0);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 1);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Origin = new Point16(1, 2);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.AnchorBottom = new AnchorData(AnchorType.SolidTile, 1, 1);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);
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
            player.cursorItemIconID = ModContent.ItemType<Memoriam>();
        }
    }
}
*/