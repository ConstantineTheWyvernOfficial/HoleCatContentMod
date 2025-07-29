using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.SummonItems;
using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Common;
using DestroyerTest.Content.MetallurgySeries;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class AntiqueCrown : ModItem
	{


		public override void SetStaticDefaults() {
			// If your head equipment should draw hair while drawn, use one of the following:
            //ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			//ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;
		}

		public override void SetDefaults() {
			Item.width = 22; // Width of the item
			Item.height = 12; // Height of the item
			Item.value = Item.sellPrice(gold: 8); // How many coins the item is worth
			Item.rare = ModContent.RarityType<ScepterArmorPHMRarity>(); // The rarity of the item
			Item.defense = 3; // The amount of defense the item will give when equipped
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<FleeceRobe>();
		}



        // UpdateArmorSet allows you to give set bonuses to the armor.
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "4% Increased Scepter Damage and an Additional 20 copper on enemy kills.";
            player.GetDamage(ModContent.GetInstance<ScepterClass>()) *= 1.04f;
            player.AddCoinLuck(player.Center, 20);
        }

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<ManganeseBronze>(5)
				.AddTile(TileID.Anvils)
				.Register();
            
		}
	}
}