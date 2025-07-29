
using DestroyerTest.Content.Magic.ScepterSubclass;
using DestroyerTest.Content.Tiles.AchievementPaintingTiles;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Consumables
{
	public class ScepterAchievementBag : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.maxStack = 1;
			Item.consumable = true;
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Purple;
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
			// We have to replicate the expert drops from MinionBossBody here

			itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<MageGlove>(), 1, 1, 1));
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<WhackAMole_Master_Painting>(), 1, 1, 1));
			itemLoot.Add(ItemDropRule.Coins(150, true));
		}
	}
}