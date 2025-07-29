using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Terraria.Audio;
using Terraria.Localization;
using System;
using DestroyerTest.Content.Entity;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Tiles
{
    public class NightmareBouqetTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(60, 10, 80), Language.GetText("Nightmare Bouqet"));
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                ModTileEntity.PlaceEntityNet(i, j, ModContent.TileEntityType<NightmareSproutTE>());
            }
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            // Check if the player is holding the item
            if (player.HeldItem.type == ModContent.ItemType<WitheredBouqet>())
            {
                Point16 tileEntityPos = new(i, j);
                if (TileEntity.ByPosition.TryGetValue(tileEntityPos, out TileEntity te) && te is NightmareSproutTE sprout)
                {
                    // Check if the area is flat
                    if (!NightmareSproutTE.IsAreaFlatEnough(i - 8, j, 16, 4))
                    {
                        Main.NewText("The rose refuses to bloom on uneven ground.", Color.MediumPurple);
                        return false;
                    }

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 spawnPos = sprout.Position.ToWorldCoordinates() + new Vector2(0, -48);
                        NPC.NewNPC(null, (int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<NightmareRoseBoss>());
                    }

                    sprout.Kill(i, j);

                    // Consume the item
                    if (!player.creativeGodMode) // Optional: avoid consumption in Journey Mode
                        player.HeldItem.stack--;

                    // Sync tile kill if needed
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.TileEntitySharing, number: 0, number2: i, number3: j);

                    // Play use sound
                    SoundEngine.PlaySound(SoundID.Item44, player.position);
                }
            }
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Point16 pos = new Point16(i, j);
            if (ModTileEntity.ByPosition.TryGetValue(pos, out TileEntity tileEntity) && tileEntity is NightmareSproutTE sprout)
            {
                sprout.Kill(i, j);
            }
        }
    }

    public class NightmareSproutTE : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            return tile.HasTile && tile.TileType == ModContent.TileType<NightmareBouqetTile>();
        }

        public override void Update()
        {
            // Optional visual or ambient effect code here
        }

        public static bool IsAreaFlatEnough(int startX, int startY, int width = 16, int tolerance = 4)
        {
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + 30; y++)
                {
                    if (Framing.GetTileSafely(x, y).HasTile && Main.tileSolid[Framing.GetTileSafely(x, y).TileType])
                    {
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);
                        break;
                    }
                }
            }

            return (maxY - minY) <= tolerance;
        }
    }
    public class WitheredBouqet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ModContent.RarityType<CorruptionSpecialRarity>();
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
            Item.maxStack = 20;
        }

    }
}
