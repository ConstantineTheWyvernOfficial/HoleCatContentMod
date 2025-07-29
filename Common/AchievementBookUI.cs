using Terraria.UI;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using System.Collections.Generic;
using DestroyerTest.Common.DestroyerTest.Common;

namespace DestroyerTest.Common
{
    public class AchievementBookUI : UIState
    {
        private UIList achievementList;
        private List<Achievement> achievements;

        public override void OnInitialize()
        {
            if (!Main.dedServ)
            {
                achievementList = new UIList();
                achievementList.Width.Set(0, 1f);
                achievementList.Height.Set(0, 1f);
                achievementList.ListPadding = 5f;
                Append(achievementList);

                achievements = AchievementManager.GetAchievements();

                // Add each achievement to the list
                foreach (var achievement in achievements)
                {
                    var item = new UIAchievementItem(achievement);
                    achievementList.Add(item);
                }
            }
        }
    }

namespace DestroyerTest.Common
{
    public class UIAchievementItem : UIPanel
    {
        private Achievement achievement;
        private UIImage iconImage;
        private UIText nameText;
        private UIText descriptionText;

        public UIAchievementItem(Achievement achievement)
        {
            this.achievement = achievement;
        }

        public override void OnInitialize()
        {
            Width.Set(0, 1f);
            Height.Set(50, 0f);

            iconImage = new UIImage(achievement.Icon);
            iconImage.Left.Set(5, 0f);
            iconImage.Top.Set(5, 0f);
            iconImage.Width.Set(40, 0f);
            iconImage.Height.Set(40, 0f);
            Append(iconImage);

            nameText = new UIText(achievement.Name);
            nameText.Left.Set(50, 0f);
            nameText.Top.Set(5, 0f);
            Append(nameText);

            descriptionText = new UIText(achievement.Description);
            descriptionText.Left.Set(50, 0f);
            descriptionText.Top.Set(25, 0f);
            descriptionText.Width.Set(200, 0f);
            Append(descriptionText);

           if (achievement.IsUnlocked)
            {
                nameText.TextColor = Color.Green; // For unlocked achievements
            }
            else
            {
                nameText.TextColor = Color.Gray; // For locked achievements
            }

        }
    }
}

}
