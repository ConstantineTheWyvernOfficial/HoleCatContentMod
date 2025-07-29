using DestroyerTest.Content.Entity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

public class WorldFlags : ModSystem
{
    public static bool ScholarSpawned;

    public override void OnWorldLoad()
    {
        ScholarSpawned = false;
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag["ScholarSpawned"] = ScholarSpawned;
    }

    public override void LoadWorldData(TagCompound tag)
    {
        ScholarSpawned = tag.GetBool("ScholarSpawned");
    }
    
    public override void PostUpdateWorld()
    {
        if (!WorldFlags.ScholarSpawned && Main.dayTime && Main.time > Main.rand.Next(3200, 4800)) // 10 seconds into day
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active)
                {
                    int npcIndex = NPC.NewNPC(player.GetSource_Misc("ScholarArrival"),
                        (int)(Main.spawnTileX + 100), // Just off-screen
                        (int)(Main.spawnTileY - 50),  // Slightly above
                        ModContent.NPCType<Scholar>());
                    Main.npc[npcIndex].homeTileX = (int)player.Center.X / 16;
                    Main.npc[npcIndex].homeTileY = (int)player.Center.Y / 16;
                    Main.NewText("Someone has appeared near spawn...");
                    WorldFlags.ScholarSpawned = true;

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);

                    break;
                }
            }
        }
    }

}
