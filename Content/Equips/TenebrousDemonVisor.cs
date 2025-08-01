using DestroyerTest.Content.Resources;
using DestroyerTest.Content.SummonItems;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class TenebrousDemonVisor : ModItem
	{


		public override void SetStaticDefaults() {
			// If your head equipment should draw hair while drawn, use one of the following:

		}

		public override void SetDefaults() {
			Item.width = 26; // Width of the item
			Item.height = 27; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<ShimmeringRarity>(); // The rarity of the item
			Item.defense = 10; // The amount of defense the item will give when equipped
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<TenebrousDemonChestplate>() && legs.type == ModContent.ItemType<TenebrousDemonChausses>();
		}

        // UpdateArmorSet allows you to give set bonuses to the armor.
        public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Ranged) *= 1.35f;
            
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<RiftGuardVisor>(1)
                .AddIngredient<Tenebris>(6)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}