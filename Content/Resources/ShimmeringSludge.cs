using System;
using DestroyerTest.Content.Entity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Resources
{
	public class ShimmeringSludge : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 25;
		}

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 1000;
			Item.alpha = 0;
			Item.rare = ModContent.RarityType<ShimmeringRarity>();
		}

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
		}
	}

	public class SS_DROP_NPC : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {

			if (npc.type == ModContent.NPCType<TenebrousSlime>()) {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShimmeringSludge>(), 1, 1, 15));
			}
			else
			{
				Main.NewText("SS drop attempted", Color.Red);
			}
		}
	}
}
