using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using Terraria.Social.Steam;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyerTest.Content.Resources;

    
	public class Shadowflame : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 5;
		}

		public override void SetDefaults() {
			Item.width = 16; // The item texture's width
			Item.height = 20; // The item texture's height

			Item.maxStack = Item.CommonMaxStack; // The item's max stack value
			Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 0.6f; // Disables gravity for this item
        }


    public class SF_DROP_NPC : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			if (
                npc.type == NPCID.GoblinSorcerer ||
                npc.type == NPCID.ShadowFlameApparition ||
                npc.type == NPCID.GoblinSummoner) {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Shadowflame>(), 3, 5, 13));
			}

		}
	}
}

