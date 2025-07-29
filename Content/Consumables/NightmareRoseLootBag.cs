
using DestroyerTest.Common;
using DestroyerTest.Content.Entity;
using DestroyerTest.Content.Equips;
using DestroyerTest.Content.Magic;
using DestroyerTest.Content.Magic.ScepterSubclass;
using DestroyerTest.Content.MeleeWeapons.SwordLineage;
using DestroyerTest.Content.RangedItems;
using DestroyerTest.Content.RogueItems;
using DestroyerTest.Content.SummonItems;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.AchievementPaintingTiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Consumables
{
    public class NightmareRoseLootBag : ModItem
    {

        public override void SetStaticDefaults()
        {
            
            ItemID.Sets.BossBag[Type] = true;
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.rare = ModContent.RarityType<CorruptionSpecialRarity>();
        }


        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // We have to replicate the expert drops from MinionBossBody here

            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Contempt>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<GigaCursedHammerWeapon>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<CorruptPetalGun>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<DeadlyBlossom>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<PossessedDartRifleItem>(), 2, 1, 1));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ItemID.CursedFlame, 2, 20, 60));
            itemLoot.Add(ItemDropRule.Coins(1250, true));
        }
    }
    
    public class LootBagDropHandler_NightmareRose : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {

			if (npc.type == ModContent.NPCType<NightmareRoseBoss>()) {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<NightmareRoseLootBag>(), 1, 1, 1));
			}
		}
	}
}