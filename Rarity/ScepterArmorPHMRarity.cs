using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace DestroyerTest.Rarity
{
	public class ScepterArmorPHMRarity : ModRarity
    {
        public override Color RarityColor => new Color(53, 84, 95); // Change color as needed

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset > 0) 
            {
                return ItemRarityID.Green; // Upgrade to next rarity if necessary
            }

            return ItemRarityID.White; // No lower tier, so return itself
        }
    }
	
}