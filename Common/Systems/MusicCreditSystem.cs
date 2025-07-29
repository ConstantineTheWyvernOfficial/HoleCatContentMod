using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

public class MusicCreditSystem : ModSystem
{
    public static bool ShowCredit = false;
    public static string CreditText = "Track: 'Running From Demons' By Waterflame | https://www.youtube.com/channel/UCVuv5iaVR55QXIc_BHQLakA";

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "DestroyerTest: Music Credit",
                delegate
                {
                    if (ShowCredit)
                    {
                        DrawMusicCredit(Main.spriteBatch);
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }

    private void DrawMusicCredit(SpriteBatch spriteBatch)
    {
        string text = CreditText;
        var font = FontAssets.DeathText.Value;
        float scale = 0.5f; // Adjust this to your liking (0.5 is nice and subtle)

        Vector2 size = font.MeasureString(text) * scale;
        Vector2 position = new Vector2((Main.screenWidth - size.X) / 2f, 20f); // Centered horizontally, 20px from top

        Utils.DrawBorderString(spriteBatch, text, position, Color.LightBlue, scale);
    }

}
