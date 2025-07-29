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

    
	public class Motherboard : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 15;
		}

		public override void SetDefaults() {
			Item.width = 28; // The item texture's width
			Item.height = 18; // The item texture's height

			Item.maxStack = Item.CommonMaxStack; // The item's max stack value
			Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
        }
        
        public class MB_DROP_NPC : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			if (
                npc.type == NPCID.DeadlySphere ||
                npc.type == NPCID.Cyborg ||
                npc.type == NPCID.TheDestroyer ||
                npc.type == NPCID.Retinazer ||
                npc.type == NPCID.Spazmatism ||
                npc.type == NPCID.SkeletronPrime) {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Motherboard>(), 3, 10, 17));
			}
            if (
                npc.type == NPCID.TheDestroyerBody ||
                npc.type == NPCID.TheDestroyerTail ||
                npc.type == NPCID.Probe ||
                npc.type == NPCID.PrimeLaser ||
                npc.type == NPCID.PrimeSaw ||
                npc.type == NPCID.PrimeVice ||
                npc.type == NPCID.PrimeCannon) {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Motherboard>(), 3, 1, 4));
			}

		}
	}
}

