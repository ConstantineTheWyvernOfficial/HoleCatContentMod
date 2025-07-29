using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using DestroyerTest.Common;
using Terraria.ID;

namespace DestroyerTest.Content.Achievement
{
    public class AchievementUndoer : ModItem
    {
        public override void SetDefaults() {
			Item.width = 98;
            Item.height = 90;
            Item.UseSound = SoundID.Item116;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 1);
		}

        public override bool? UseItem(Player player)
        {
            AchievementManager.achievements["WhackAMoleMaster"].IsUnlocked = false;
            return true; // Indicate that the item was used successfully
        }
    }
}
