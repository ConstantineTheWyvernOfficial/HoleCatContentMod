using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.Achievements;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyerTest.Common
{
    public class Achievement
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; }
        public Texture2D Icon { get; set; }
    }
    public class AchievementManager : ModSystem
    {

        public static Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

        public override void Load()
        {
            // Initialize achievements here
            achievements.Add("WhackAMoleMaster", new Achievement
            {
                Name = "Whack-A-Mole Master",
                Description = "Hit 3 or more enemies with a single scepter throw.",
                IsUnlocked = false,
                Icon = ModContent.Request<Texture2D>("DestroyerTest/Content/Achievement/Achievement_WhackAMoleMaster!").Value
            });
        }

        public static List<Achievement> GetAchievements()
        {
            return new List<Achievement>(achievements.Values);
        }

        public static void UnlockAchievement(string achievementName)
        {
            if (achievements.ContainsKey(achievementName))
            {
                achievements[achievementName].IsUnlocked = true;
            }
        }
    }
}
