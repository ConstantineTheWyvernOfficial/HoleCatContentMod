using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using DestroyerTest.Common;

namespace DestroyerTest.Content.Achievement
{
    public class AchievementBook : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            // Add other properties as needed
        }

        public override bool? UseItem(Player player)
        {
            // Open the Achievement Book UI
            AchievementBookUI achievementBookUI = new AchievementBookUI();
            return true; // Indicate that the item was used successfully
        }
    }
}
