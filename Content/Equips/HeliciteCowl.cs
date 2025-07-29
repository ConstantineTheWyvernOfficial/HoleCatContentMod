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

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class HeliciteCowl : ModItem
	{


		public override void SetStaticDefaults() {
			// If your head equipment should draw hair while drawn, use one of the following:

		}

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<RiftRarity2>(); // The rarity of the item
			Item.defense = 20; // The amount of defense the item will give when equipped
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<HeliciteRobe>() && legs.type == ModContent.ItemType<HeliciteChausses>();
		}

		

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
			if (IsArmorSet(player.armor[0], player.armor[1], player.armor[2])) {
			player.GetDamage<RangedDamageClass>() *= 3.0f;
			player.AddBuff(ModContent.BuffType<RiftBallBuff>(), 3600); // Adds the RiftBallBuff for 60 seconds (3600 ticks)
			player.AddBuff(ModContent.BuffType<AirSeal>(), 3600); // Adds the RiftBallBuff for 60 seconds (3600 ticks)
			}
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Living_Shadow>(5)
                .AddIngredient<Item_HeliciteCrystal>(20)
                .AddIngredient(ItemID.Silk, 15)
				.AddTile<Tile_RiftCrucible>()
                .AddCondition(Condition.DownedGolem)
				.Register();
            
		}
	}
}