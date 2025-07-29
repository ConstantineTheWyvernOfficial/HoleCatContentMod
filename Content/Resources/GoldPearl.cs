using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Resources
{
	public class GoldPearl : ModItem
	{
		public override void SetStaticDefaults() {

			Item.ResearchUnlockCount = 5;
		}

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 1000;
			Item.rare = ItemRarityID.Purple;
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, Color.Gold.ToVector3() * 0.55f * Main.essScale);
		}
	}

	public class GP_DROP_NPC : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {

			if (npc.type == NPCID.QueenSlimeBoss) {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GoldPearl>(), 1, 1, 2));
			}
		}
	}
}
