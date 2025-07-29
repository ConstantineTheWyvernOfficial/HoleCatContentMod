
using DetroyerTest.Content.SHADEMANAGEMENT;
using DestroyerTest.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using DestroyerTest.Common.Systems;

namespace DestroyerTest.Content.SHADEMANAGEMENT
{
    public class ShadeSystem : ModSystem
    {
        public static bool InDarkness => (Main.worldName == "Shade Under The Trees" ||
            Main.worldName == "Tenebrous Trial" ||
            Main.worldName == "Respite from Sun" ||
            Main.worldName == "Twilight At Last" ||
            Main.worldName == "Garden of Eden" ||
            Main.worldName == "Moon" ||
            Main.worldName == "Witching Hour" ||
            Main.worldName == "Tenebris") && DownedBossSystem.downedLunarBoss == false;

        public override void PostUpdateWorld()
        {
            if (InDarkness)
            {
                Darkness();
            }
        }




        public void Darkness()
        {
            Player player = Main.LocalPlayer;
            player.ZoneWaterCandle = true;
            player.name = CrazyText.scrambledString;
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
            {
                if (InDarkness)
                {
                    // Set both tile and background colors to black
                    tileColor = Color.Black;
                    backgroundColor = Color.Black;
                }
            }

        public class ShadeScene : ModSceneEffect
        {
            public override bool IsSceneEffectActive(Player player)
            => InDarkness;

            public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<ShadeStyle>();

            public override ModWaterStyle WaterStyle => ModContent.GetInstance<ShadeWater>();

            public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftEmptiness");
            public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
        }
    }
}
