
using System;
using DestroyerTest.Common;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Magic.ScepterSubclass
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.HandsOn)]
	public class ElementalAlkahest : ModItem
	{
		
		public override void SetDefaults() {
			Item.width = 22; // Width of the item
			Item.height = 28; // Height of the item
			Item.value = Item.sellPrice(gold: 86); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
            Item.vanity = false;
            Item.accessory = true;
		}

		public override void UpdateEquip(Player player) {
			player.GetAttackSpeed<ScepterClass>() += 0.05f;
            player.GetDamage<ScepterClass>() += 0.10f;
            ScepterClassStats.Range += 50;
            int[] buffs = new int[]
			{
				BuffID.WeaponImbueCursedFlames,
				BuffID.WeaponImbueFire,
				BuffID.WeaponImbueGold,
				BuffID.WeaponImbueIchor,
				BuffID.WeaponImbueNanites,
				BuffID.WeaponImbuePoison,
				BuffID.WeaponImbueVenom
			};

			player.AddBuff(buffs[Main.rand.Next(buffs.Length)], 2);

		}

       public class EA_DROP_NPC : GlobalNPC
		{
			public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {

				if (npc.type == NPCID.MoonLordCore) {
					npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<ElementalAlkahest>()));
				}
			}
		}
	}
}
