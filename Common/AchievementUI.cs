using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System.Collections.Generic;

namespace DestroyerTest.Common
{
    public class AchievementUI : ModSystem
    {
        private static int displayTime = 0;
        private static Texture2D unlockedTexture;
        private static Texture2D lockedTexture;

        private static int transitionTime = 0;
        private static bool wasUnlocked = false;


        public override void Load()
        {
            if (!Main.dedServ)
            {
                lockedTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Achievement/Achievement_WhackAMoleMaster!_Locked").Value;
                unlockedTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Achievement/Achievement_WhackAMoleMaster!").Value;
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                lockedTexture = null;
                unlockedTexture = null;
            }
        }

        public static void ShowAchievement()
        {
            displayTime = 180; // Show for 3 seconds
        }

        // Override the ModifyInterfaceLayers method to add your custom UI
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (!Main.dedServ)
            {
                // We need to find the layer just before the mouse and hover text layer
                int interfaceLayerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

                if (interfaceLayerIndex != -1)
                {
                    layers.Insert(interfaceLayerIndex, new LegacyGameInterfaceLayer(
                        "DestroyerTest: Achievement UI",
                        delegate
                        {
                            DrawAchievementUI(Main.spriteBatch);
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }

        public static void ShowAchievement(bool unlocked)
        {
            displayTime = 180; // Show for 3 seconds
            if (unlocked && !wasUnlocked)
            {
                wasUnlocked = true;
                transitionTime = 60; // 1-second transition
            }
        }


        // This method will handle the drawing of the achievement UI
        private void DrawAchievementUI(SpriteBatch spriteBatch)
        {
            if (!Main.dedServ)
            {
                Player player = Main.LocalPlayer;
                Achievement whackAMoleMaster = AchievementManager.achievements["WhackAMoleMaster"];

                if (displayTime > 0)
                {
                    displayTime--;

                    float fade = (displayTime > 150) ? 1f : displayTime / 150f; // General fade-out effect
                    Texture2D texture;

                    // Handle transition
                    if (!wasUnlocked)
                    {
                        texture = lockedTexture;
                    }
                    else if (transitionTime > 0)
                    {
                        transitionTime--;
                        float transitionAlpha = 1f - (transitionTime / 60f); // 1-second transition
                        spriteBatch.Draw(lockedTexture, new Vector2(player.position.X, player.position.Y - 15), Color.White * (1f - transitionAlpha) * fade);
                        spriteBatch.Draw(unlockedTexture, new Vector2(player.position.X, player.position.Y - 15), Color.White * transitionAlpha * fade);
                        return; // Prevent further drawing
                    }
                    else
                    {
                        texture = unlockedTexture;
                    }

                    Vector2 position = new Vector2(player.position.X, player.position.Y - 15);
                    spriteBatch.Draw(texture, position, Color.White * fade);
                }
            }
        }


    }
}
