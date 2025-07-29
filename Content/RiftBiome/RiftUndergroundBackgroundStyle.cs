using Terraria.ModLoader;

namespace DetroyerTest.Content.RiftBiome
{
    public class RiftUndergroundBackgroundStyle : ModUndergroundBackgroundStyle
        {
            public override void FillTextureArray(int[] textureSlots)
            {
                textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/RiftUndergroundFar");
                textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/RiftUndergroundMid");
            }
        }
}