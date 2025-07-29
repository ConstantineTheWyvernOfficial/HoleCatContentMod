using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using DestroyerTest.Common;
using SteelSeries.GameSense;
using ReLogic.Content;

public class ColorGradientOverlaySystem : ModSystem
{
    public static float ColorVisibility = 0f; // Controls how bright the flash is

    public override void ModifyTransformMatrix(ref SpriteViewMatrix transform)
    {
        if (ColorVisibility > 0f)
        {
            /*
            ColorVisibility -= 0.05f;
            if (ColorVisibility < 0f)
                ColorVisibility = 0f;
            */
        }
    }

    public override void PreUpdatePlayers()
    {
        ManageStellarGoggleEquips();
    }

    public override void PostDrawInterface(SpriteBatch spriteBatch)
    {
        if (ColorVisibility > 0f)
        {
            Color colorTop = new Color(143, 39, 120) * ColorVisibility;
            Color colorBottom = new Color(247, 233, 141) * ColorVisibility;

            Texture2D pixel = TextureAssets.MagicPixel.Value;
            int screenWidth = Main.screenWidth;
            int screenHeight = Main.screenHeight;
            //Effect shader = ModContent.Request<Effect>("DestroyerTest/Assets/HSHLShaders/SlashTrans", AssetRequestMode.ImmediateLoad).Value;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            for (int y = 0; y < screenHeight; y++)
            {
                float t = (float)y / (screenHeight - 1);
                Color lerpedColor = Color.Lerp(colorBottom, colorTop, t);
                spriteBatch.Draw(pixel, new Rectangle(0, y, screenWidth, 1), lerpedColor);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
        }
    }


    public static void FadeInHaze(float MaxAmount)
    {
        if (ColorVisibility < MaxAmount)
        {
            ColorVisibility += 0.02f; // Adjust this value for speed
            if (ColorVisibility > MaxAmount)
                ColorVisibility = MaxAmount;
        }

        if (MaxAmount > 1.0f || MaxAmount < 0f)
            throw new System.Exception("MaxAmount must be between 0.0f and 1.0f");
    }

    public static void FadeOutHaze(float MinAmount)
    {
        if (ColorVisibility > MinAmount)
        {
            ColorVisibility -= 0.02f; // Adjust this value for speed
            if (ColorVisibility < MinAmount)
                ColorVisibility = MinAmount;
        }

        if (MinAmount < 0f || MinAmount > 1.0f)
            throw new System.Exception("MinAmount must be between 0.0f and 1.0f");
    }

    public void ManageStellarGoggleEquips()
    {
        if (DTUtils.StellarGogglesEquipped == true && ColorVisibility < 0.7f)
        {
            FadeInHaze(0.7f);
        }
        if (DTUtils.StellarGogglesEquipped == false && ColorVisibility > 0.0f)
        {
           FadeOutHaze(0.0f);
        }
    }
}
