using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using DestroyerTest.Content.Resources.Cloths;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Rarity;
using System.Collections.Generic;
using DestroyerTest.Content.Equips;
using DestroyerTest.Content.RiftArsenal;
using Terraria.GameContent.ItemDropRules;
using System.Linq;
using DestroyerTest.Content.Magic.ScepterSubclass;

namespace DestroyerTest.Common
{
    public class HeldItemEffects : GlobalItem
    {
        public void ShimmeringItems()
        {
            Player player = Main.LocalPlayer; // Gets the client-side player
            Item heldItem = player.HeldItem;
            if (player.HeldItem.type == ModContent.ItemType<Tenebris>() || player.HeldItem.type == ModContent.ItemType<ShimmeringSludge>())
            {
                player.AddBuff(ModContent.BuffType<ShimmeringFlames>(), 120); // Applies Poisoned for at least 2 ticks (keeps refreshing)
            }
        }
    }


    public class TooltipColors : GlobalItem
    {
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (item.rare == ModContent.RarityType<MetallurgyRarity>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(193, 89, 0);
                Color strokeColor2 = new Color(101, 47, 0);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Main text color
                Color textColor = new Color(39, 39, 39);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }

            if (item.rare == ModContent.RarityType<ShimmeringRarity>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(0, 55, 255);
                Color strokeColor2 = new Color(180, 0, 255);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Main text color
                Color textColor = new Color(8, 8, 8);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }
            if (item.rare == ModContent.RarityType<RiftRarity1>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(0, 0, 0);
                Color strokeColor2 = new Color(255, 155, 0);

                Color textColor1 = new Color(255, 155, 0);
                Color textColor2 = new Color(0, 0, 0);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount2 = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount2);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }
            if (item.rare == ModContent.RarityType<RiftRarity2>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(255, 155, 0);
                Color strokeColor2 = new Color(48, 29, 0);

                Color textColor1 = new Color(48, 29, 0);
                Color textColor2 = new Color(255, 155, 0);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount2 = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount2);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }
            if (item.rare == ModContent.RarityType<ContenderRarity>() && line.Name == "ItemName")
            {
                float speed = 0.08f;
                float lerpAmount = 0.5f * (1 + (float)Math.Sin(Main.GlobalTimeWrappedHourly * speed * 2f * Math.PI));

                // Define control points in order: Black → Red → Black → White → Black
                Color[] colors = { Color.Black, Color.Red, Color.Black, Color.White, Color.Black };

                // Map lerpAmount (0 → 1) to a segment of our five-color gradient
                float scaledLerp = lerpAmount * (colors.Length - 1);
                int index = (int)scaledLerp;  // Get the segment index
                float segmentLerp = scaledLerp - index;  // Get the lerp factor within the segment

                // Ensure index stays within bounds
                index = Math.Clamp(index, 0, colors.Length - 2);

                // Lerp between the two selected colors
                Color strokeColor = Color.Lerp(colors[index], colors[index + 1], segmentLerp);



                // Main text color
                Color textColor = new Color(0, 0, 0);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);



                return false; // Prevents Terraria from drawing the default text
            }
            if (item.rare == ModContent.RarityType<CrimsonSpecialRarity>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(255, 0, 0);
                Color strokeColor2 = new Color(100, 0, 0);

                Color textColor1 = new Color(40, 0, 0);
                Color textColor2 = new Color(80, 0, 0);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount2 = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount2);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }
            if (item.rare == ModContent.RarityType<CorruptionSpecialRarity>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(120, 0, 169);
                Color strokeColor2 = new Color(68, 0, 95);

                Color textColor1 = new Color(37, 11, 48);
                Color textColor2 = new Color(61, 23, 78);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount2 = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount2);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }
            if (item.rare == ModContent.RarityType<HallowedSpecialRarity>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(0, 210, 255);
                Color strokeColor2 = new Color(0, 93, 112);

                Color textColor1 = new Color(10, 55, 65);
                Color textColor2 = new Color(65, 54, 10);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount2 = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount2);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }

            if (item.rare == ModContent.RarityType<TestRarity>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(150, 150, 150);
                Color strokeColor2 = new Color(255, 255, 255);

                Color textColor1 = new Color(80, 0, 0);
                Color textColor2 = new Color(0, 80, 80);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount2 = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount2);

                Vector2 YOffset = new Vector2(0, Main.rand.NextBool(10) ? Main.rand.Next(3, 6) : 0);
                Vector2 XOffset = new Vector2(Main.rand.NextBool(10) ? Main.rand.Next(3, 6) : 0, 0);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j) + YOffset + XOffset, strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }

            if (item.DamageType == ModContent.GetInstance<ScepterClass>() && line.Name == "Damage")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(255, 255, 255);
                Color strokeColor2 = new Color(141, 242, 222);

                Color textColor1 = new Color(40, 40, 40);
                Color textColor2 = new Color(24, 48, 43);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount2 = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount2);


                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }
            if (item.rare == ModContent.RarityType<LifeEchoRarity>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(255, 255, 255);
                Color strokeColor2 = new Color(184, 228, 242);

                Color textColor1 = new Color(0, 0, 0);
                Color textColor2 = new Color(3, 24, 30);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount2 = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount2);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }
            if (item.rare == ModContent.RarityType<EndemyRarity>() && line.Name == "ItemName")
            {
                // Define two colors to cycle between for the stroke
                Color strokeColor1 = new Color(132, 8, 172);
                Color strokeColor2 = new Color(218, 191, 28);

                // Use a sine wave to smoothly transition between the two colors
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);

                // Main text color
                Color textColor = new Color(34, 32, 52);

                // Extract the correct font reference
                DynamicSpriteFont font = FontAssets.MouseText.Value;

                // Draw the outline first by offsetting in all directions
                Vector2 position = new Vector2(line.X, line.Y);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position + new Vector2(i, j), strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                return false; // Prevents Terraria from drawing the default text
            }
            return true; // Default behavior for other rarities 
        }
    }

    public class DevTooltip : GlobalItem
    {
        // Define stroke and text colors
        static Color strokeColor1 = new Color(255, 255, 255); // White
        static Color strokeColor2 = new Color(255, 0, 0);     // Red

        static Color textColor1 = new Color(1, 240, 176);     // Light teal
        static Color textColor2 = new Color(0, 0, 0);         // Black

        // Set global behavior to affect all items
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return true; // Applies to all items
        }

        // Modify tooltips to add the custom developer line
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            // Define the custom tooltip text
            string customText = "\nᛞ Developer Item ᛞ";

            // Create a new TooltipLine with a custom color
            TooltipLine line = new TooltipLine(Mod, "CustomTooltip", customText)
            {
                OverrideColor = Color.Purple // Optional: Base color
            };

            // Add the custom tooltip to the end of the list
            if (item.TryGetGlobalItem(out DevItems globalItem) && globalItem.isDevItem)
            {
                tooltips.Add(line);
            }
        }

        // PreDrawTooltipLine - Draw the text and stroke manually
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            // Check if it's our custom tooltip
            if (line.Name == "CustomTooltip" && line.Mod == Mod.Name && item.TryGetGlobalItem(out DevItems globalItem) && globalItem.isDevItem)
            {
                // Smoothly interpolate between stroke and text colors using sine wave
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount);

                // Define the font and position
                DynamicSpriteFont font = FontAssets.MouseText.Value;
                Vector2 position = new Vector2(line.X, line.Y);

                // Draw the stroke by offsetting text in all directions
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        Vector2 offsetPosition = position + new Vector2(i, j);
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, line.Text, offsetPosition, strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top with the smooth color transition
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                // Return false to prevent default drawing since we manually drew it
                return false;
            }

            // Allow other tooltips to draw normally
            return true;
        }
    }

    public class InspiroItem : GlobalItem
    {
        // Define stroke and text colors
        static Color strokeColor1 = new Color(134, 53, 112);
        static Color strokeColor2 = new Color(108, 42, 90);

        static Color textColor1 = new Color(151, 114, 147);
        static Color textColor2 = new Color(112, 82, 109);

        // Set global behavior to affect all items
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return true; // Applies to all items
        }

        // Modify tooltips to add the custom developer line
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            // Define the custom tooltip text
            string customText = $"\n{CrazyText.scrambledString} \nInspiro Design \n{CrazyText.scrambledString}";

            // Create a new TooltipLine with a custom color
            TooltipLine line = new TooltipLine(Mod, "CustomTooltip", customText)
            {
                OverrideColor = Color.Purple // Optional: Base color
            };

            // Add the custom tooltip to the end of the list
            if (item.TryGetGlobalItem(out InspiroItemList globalItem) && globalItem.isInspiro)
            {
                tooltips.Add(line);
            }
        }

        // PreDrawTooltipLine - Draw the text and stroke manually
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            // Check if it's our custom tooltip
            if (line.Name == "CustomTooltip" && line.Mod == Mod.Name && item.TryGetGlobalItem(out InspiroItemList globalItem) && globalItem.isInspiro)
            {
                // Smoothly interpolate between stroke and text colors using sine wave
                float lerpAmount = (float)(0.5 * (1 + Math.Sin(Main.GlobalTimeWrappedHourly * 2f * Math.PI)));
                Color strokeColor = Color.Lerp(strokeColor1, strokeColor2, lerpAmount);
                Color textColor = Color.Lerp(textColor1, textColor2, lerpAmount);

                // Define the font and position
                DynamicSpriteFont font = FontAssets.MouseText.Value;
                Vector2 position = new Vector2(line.X, line.Y);

                // Draw the stroke by offsetting text in all directions
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue; // Skip center (main text)
                        Vector2 offsetPosition = position + new Vector2(i, j);
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, font, line.Text, offsetPosition, strokeColor, 0f, Vector2.Zero, Vector2.One);
                    }
                }

                // Draw the actual text on top with the smooth color transition
                ChatManager.DrawColorCodedString(Main.spriteBatch, font, line.Text, position, textColor, 0f, Vector2.Zero, Vector2.One);

                // Return false to prevent default drawing since we manually drew it
                return false;
            }

            // Allow other tooltips to draw normally
            return true;
        }
    }


    public class ScepterClassFamily : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.DamageType == ModContent.GetInstance<ScepterClass>();
        }

        public override void SetDefaults(Item item)
        {
            if (item.DamageType == ModContent.GetInstance<ScepterClass>())
            {
                item.GetGlobalItem<ScepterClassFamily>().isScepter = true;
            }
        }

        public bool isScepter = false;
    }


    public class RiftArsenalGlobal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            // Apply this GlobalItem to specific items based on criteria
            return entity.type == ModContent.ItemType<RiftBroadsword>() ||
            entity.type == ModContent.ItemType<RiftChakram>() ||
            entity.type == ModContent.ItemType<RiftClaymore>() ||
            entity.type == ModContent.ItemType<RiftGreatsword>() ||
            entity.type == ModContent.ItemType<RiftPhasesaber>() ||
            entity.type == ModContent.ItemType<RiftRevolver>() ||
            entity.type == ModContent.ItemType<RiftScabbard>() ||
            entity.type == ModContent.ItemType<RiftScythe>() ||
            entity.type == ModContent.ItemType<RiftStaff>() ||
            entity.type == ModContent.ItemType<RiftThrowingKnife>() ||
            entity.type == ModContent.ItemType<RiftZapinator>();
        }

        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<RiftBroadsword>() ||
            item.type == ModContent.ItemType<RiftChakram>() ||
            item.type == ModContent.ItemType<RiftClaymore>() ||
            item.type == ModContent.ItemType<RiftGreatsword>() ||
            item.type == ModContent.ItemType<RiftPhasesaber>() ||
            item.type == ModContent.ItemType<RiftRevolver>() ||
            item.type == ModContent.ItemType<RiftScabbard>() ||
            item.type == ModContent.ItemType<RiftScythe>() ||
            item.type == ModContent.ItemType<RiftStaff>() ||
            item.type == ModContent.ItemType<RiftThrowingKnife>() ||
            item.type == ModContent.ItemType<RiftZapinator>())
            {
                //item.GetGlobalItem<RiftArsenalGlobal>().isRiftArsenal = true;
                this.isRiftArsenal = true; // Use 'this' to refer to the current instance
            }
        }
        public bool isRiftArsenal = false;
    }
    public class DevItems : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            // Apply this GlobalItem to specific items based on criteria
            return entity.type == ModContent.ItemType<ConstantineScythe>() ||
            entity.type == ModContent.ItemType<ConstantineMask>() ||
            entity.type == ModContent.ItemType<CoatStantine>() ||
            entity.type == ModContent.ItemType<ConstanJeans>();
        }

        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<ConstantineScythe>() ||
            item.type == ModContent.ItemType<ConstantineMask>() ||
            item.type == ModContent.ItemType<CoatStantine>() ||
            item.type == ModContent.ItemType<ConstanJeans>())
            {
                item.GetGlobalItem<DevItems>().isDevItem = true;
            }
        }
        public bool isDevItem = false;
    }

    public class InspiroItemList : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (ModLoader.TryGetMod("FranciumCalamityWeapons", out Mod FCW))
            {
                return entity.type == FCW.Find<ModItem>("Overlord").Type || entity.type == ModContent.ItemType<SparkFrostCleaver>();
            }
            else
            {
                // Apply this GlobalItem to specific items based on criteria
                return entity.type == ModContent.ItemType<SparkFrostCleaver>();
            }
        }

        public override void SetDefaults(Item item)
        {
            if (ModLoader.TryGetMod("FranciumCalamityWeapons", out Mod FCW))
            {
                if (item.type == ModContent.ItemType<SparkFrostCleaver>() || item.type == FCW.Find<ModItem>("Overlord").Type)
                {
                    item.GetGlobalItem<InspiroItemList>().isInspiro = true;
                }
            }
            else if (item.type == ModContent.ItemType<SparkFrostCleaver>())
            {
                item.GetGlobalItem<InspiroItemList>().isInspiro = true;
            }
        }
        public bool isInspiro = false;
    }
    public class NonWhiteCloth : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            // Apply this GlobalItem to specific items based on criteria
            return entity.type == ModContent.ItemType<RedCloth>() ||
            entity.type == ModContent.ItemType<OrangeCloth>() ||
            entity.type == ModContent.ItemType<YellowCloth>() ||
            entity.type == ModContent.ItemType<LimeCloth>() ||
            entity.type == ModContent.ItemType<GreenCloth>() ||
            entity.type == ModContent.ItemType<TealCloth>() ||
            entity.type == ModContent.ItemType<CyanCloth>() ||
            entity.type == ModContent.ItemType<SkyBlueCloth>() ||
            entity.type == ModContent.ItemType<BlueCloth>() ||
            entity.type == ModContent.ItemType<PurpleCloth>() ||
            entity.type == ModContent.ItemType<VioletCloth>() ||
            entity.type == ModContent.ItemType<PinkCloth>() ||
            entity.type == ModContent.ItemType<BlackCloth>() ||
            entity.type == ModContent.ItemType<BrownCloth>();
        }

        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<OrangeCloth>() ||
            item.type == ModContent.ItemType<YellowCloth>() ||
            item.type == ModContent.ItemType<LimeCloth>() ||
            item.type == ModContent.ItemType<GreenCloth>() ||
            item.type == ModContent.ItemType<TealCloth>() ||
            item.type == ModContent.ItemType<CyanCloth>() ||
            item.type == ModContent.ItemType<SkyBlueCloth>() ||
            item.type == ModContent.ItemType<BlueCloth>() ||
            item.type == ModContent.ItemType<PurpleCloth>() ||
            item.type == ModContent.ItemType<VioletCloth>() ||
            item.type == ModContent.ItemType<PinkCloth>() ||
            item.type == ModContent.ItemType<BlackCloth>() ||
            item.type == ModContent.ItemType<BrownCloth>())
            {
                item.GetGlobalItem<NonWhiteCloth>().isNonWhiteCloth = true;
            }
        }
        public bool isNonWhiteCloth = false;
    }

    public class AllCloth : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            // Apply this GlobalItem to specific items based on criteria
            return entity.type == ModContent.ItemType<RedCloth>() ||
            entity.type == ModContent.ItemType<OrangeCloth>() ||
            entity.type == ModContent.ItemType<YellowCloth>() ||
            entity.type == ModContent.ItemType<LimeCloth>() ||
            entity.type == ModContent.ItemType<GreenCloth>() ||
            entity.type == ModContent.ItemType<TealCloth>() ||
            entity.type == ModContent.ItemType<CyanCloth>() ||
            entity.type == ModContent.ItemType<SkyBlueCloth>() ||
            entity.type == ModContent.ItemType<BlueCloth>() ||
            entity.type == ModContent.ItemType<PurpleCloth>() ||
            entity.type == ModContent.ItemType<VioletCloth>() ||
            entity.type == ModContent.ItemType<PinkCloth>() ||
            entity.type == ModContent.ItemType<BlackCloth>() ||
            entity.type == ModContent.ItemType<BrownCloth>() ||
            entity.type == ModContent.ItemType<WhiteCloth>();
        }

        public override void SetDefaults(Item item)
        {
        }
    }

    public class LockBoxLoot : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.ObsidianLockbox)
            {
                foreach (IItemDropRule item4 in itemLoot.Get(false))
                {
                    OneFromRulesRule val = (OneFromRulesRule)(object)(item4 is OneFromRulesRule ? item4 : null);
                    if (val != null && CheckIfAtleastOneWithin(val.options, 274, 683, 220, 218, 3019))
                    {
                        HashSet<IItemDropRule> hashSet = new HashSet<IItemDropRule>(val.options);
                        hashSet.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<ShadowScepter>(), 3, 1, 1));
                        val.options = hashSet.ToArray();
                    }
                }
            }
        }

        private static bool CheckIfAtleastOneWithin(IItemDropRule[] rules, params int[] items)
        {
            foreach (IItemDropRule val in rules)
            {
                CommonDropNotScalingWithLuck val2 = (CommonDropNotScalingWithLuck)(object)(val is CommonDropNotScalingWithLuck ? val : null);
                if (val2 != null && items.Contains(val2.itemId))
                {
                    return true;
                }
                ItemDropWithConditionRule val3 = (ItemDropWithConditionRule)(object)(val is ItemDropWithConditionRule ? val : null);
                if (val3 != null && items.Contains(val3.itemId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}