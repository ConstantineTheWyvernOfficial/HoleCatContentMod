using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Resources;
using DestroyerTest.Rarity;
using DestroyerTest.Content.MetallurgySeries;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class TenebrousDemonChausses : ModItem
	{

		public override void SetDefaults() {
			Item.width = 22; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 8); // How many coins the item is worth
			Item.rare = ModContent.RarityType<ShimmeringRarity>(); // The rarity of the item
			Item.defense = 12; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
		}



		public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<RiftGuardChausses>(1)
                .AddIngredient<Tenebris>(6)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}