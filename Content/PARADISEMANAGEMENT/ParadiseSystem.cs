using DetroyerTest.Content.PARADISEMANAGEMENT;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace DestroyerTest.Content.PARADISEMANAGEMENT
{
    public class ParadiseSystem : ModSystem
    {
        public static bool InParadiseWorld => Main.worldName == "Paradise in a Globe";

        public override void PostUpdateWorld()
        {
            if (InParadiseWorld)
            {
                OceanifySurface();
            }
        }

        public void OceanifySurface()
        {
            Player player = Main.LocalPlayer;

            bool isOnSurface = player.ZoneOverworldHeight;

            if (isOnSurface)
            {
                Lighting.AddLight(player.Center, 0.1f, 0.4f, 0.6f); // Oceanic glow

                // Apply ocean biome for surface visuals/music
                player.ZoneBeach = true;

                // Clear other zones to avoid interference
                player.ZoneJungle = false;
                player.ZonePurity = false;
                player.ZoneCorrupt = false;
                player.ZoneCrimson = false;
                player.ZoneSnow = false;
                player.ZoneHallow = false;
                player.ZoneDesert = false;
                player.ZoneGlowshroom = false;
                player.ZoneDungeon = false;
            }
        }

        public class ParadiseScene : ModSceneEffect
        {
            public override bool IsSceneEffectActive(Player player)
                => InParadiseWorld && player.position.Y / 16f < Main.worldSurface;

            public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<ParadiseStyle>();

            public override ModWaterStyle WaterStyle => ModContent.GetInstance<ParadiseWater>();

            public override int Music => MusicID.Ocean;
            public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
        }
    }
}
