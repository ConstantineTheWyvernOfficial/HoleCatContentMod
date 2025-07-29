
using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Common
{
    public class DTUtils
    {
        public static bool PromiseEquipped = false;
        public static bool StellarGogglesEquipped = false;

        
        
    }

    public class DTWorldUpdating : ModSystem
    {
        public override void PostUpdatePlayers()
        {
            DTUtils.StellarGogglesEquipped = false;
        }
    }
    /// <summary>
    /// A static class containing a library of colors used in the mod in order to avoid having to manually enter the RGB values for them when drawing.
    /// </summary>
    public static class ColorLib
    {
        /// <summary>
        /// The deepest color used in Living Shadows and other sprites using glow from Living Shadows.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color DarkRift1 = new Color(51, 31, 0);
        /// <summary>
        /// The 2nd deepest color used in Living Shadows and other sprites using glow from Living Shadows.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color DarkRift2 = new Color(102, 61, 0);
        /// <summary>
        /// The 3rd deepest color used in Living Shadows and other sprites using glow from Living Shadows.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color DarkRift3 = new Color(153, 92, 0);
        /// <summary>
        /// The 4th deepest color used in Living Shadows and other sprites using glow from Living Shadows.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color DarkRift4 = new Color(204, 122, 0);
        /// <summary>
        /// The main color used in Living Shadows and other sprites using glow from Living Shadows. All other Rift Glow colors derive from this.
        /// </summary>
        public static Color Rift = new Color(255, 153, 0);
        /// <summary>
        /// The 4th brightest color used in Living Shadows and other sprites using glow from Living Shadows.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color LightRift1 = new Color(255, 173, 51);
        /// <summary>
        /// The 3rd brightest color used in Living Shadows and other sprites using glow from Living Shadows.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color LightRift2 = new Color(255, 194, 102);
        /// <summary>
        /// The 2nd brightest color used in Living Shadows and other sprites using glow from Living Shadows.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color LightRift3 = new Color(255, 214, 153);
        /// <summary>
        /// The brightest color used in Living Shadows and other sprites using glow from Living Shadows, aside from White.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color LightRift4 = new Color(255, 235, 204);

        /// <summary>
        /// The brightest possible color used in Living Shadows and other sprites using glow from Living Shadows. This is already an available color as White in the XNA framwwork, but ColorLib is a mirror of the entirety of every palette in the mod, so if a palette has white on it, it will end up here.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color RiftWhite = new Color(255, 255, 255);

        /// <summary>
        /// The standard beige in the tenebrous palette.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color TenebrisBeige = new Color(216, 185, 133);

        /// <summary>
        /// The standard magenta in the tenebrous palette.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color TenebrisMagenta = new Color(202, 40, 212);

        /// <summary>
        /// The standard blue in the tenebrous palette.
        /// <para/> ColorLib colors are numbered from darkest to lightest in a series.
        /// </summary>
        public static Color TenebrisBlue = new Color(87, 99, 186);

        /// <summary>
        /// An average color you will see in a cursed flame projectile.
        /// <para/> Note: This color is one of four and is the second lightest. 
        /// </summary>
        public static Color CursedFlames = new Color(179, 252, 0);

        /// <summary>
        /// An average color you will see in an Ichor projectile.
        /// <para/> Note: This color is one of five and is the third lightest. 
        /// </summary>
        public static Color Ichor = new Color(254, 202, 80);

        /// <summary>
        /// An All-Purpose Neon Gradient cycling through all the colors of the rainbow.
        /// </summary>
        public static Color RainbowGradient => new Color(Main.DiscoR / 2, (byte)(Main.DiscoG / 1.25f), (byte)(Main.DiscoB / 1.5f));


        /// <summary>
        /// The color used for drawing the aura and hit effects of the Metallurgy System Javelins.
        /// </summary>
        public static Color JavelinEnergy
        {
            get
            {
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                return Color.Lerp(Color.Gray, new Color(246, 192, 116), lerpAmount);
            }
        }

        public static Color StellarColor
        {
            get
            {
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                return Color.Lerp(new Color(247, 233, 141), new Color(143, 39, 120), lerpAmount);
            }
        }

        public static Color TenebrisGradient
        {
            get
            {
                float time = (Main.GlobalTimeWrappedHourly % 3f);

                if (time < 1f)
                    return Color.Lerp(TenebrisBeige, TenebrisMagenta, time);
                else if (time < 2f)
                    return Color.Lerp(TenebrisMagenta, TenebrisBlue, time - 1f);
                else
                    return Color.Lerp(TenebrisBlue, TenebrisBeige, time - 2f);
            }
        }

        public static Color CelestialGradient
        {
            get
            {
                float time = (Main.GlobalTimeWrappedHourly % 3f);

                if (time < 1f)
                    return Color.Lerp(new Color(0, 174, 238), new Color(0, 242, 170), time);
                else if (time < 2f)
                    return Color.Lerp(new Color(0, 242, 170), new Color(254, 158, 35), time - 1f);
                else if (time < 3f)
                    return Color.Lerp(new Color(254, 158, 35), new Color(190, 30, 209), time - 2f);
                else
                    return Color.Lerp(new Color(190, 30, 209), new Color(0, 174, 238), time - 3f);
            }
        }
    }
}