
using DestroyerTest.Content.Resources.Cloths;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Equips
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Back)]
	public class BiCape : ModItem
	{

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 10); // How many coins the item is worth
			Item.rare = ModContent.RarityType<EntropyRarity2>(); // The rarity of the item
            Item.vanity = true;
            int realBackSlot = Item.backSlot;
			Item.backSlot = realBackSlot;
			Item.accessory = true;
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PurpleCloth>(7)
                .AddIngredient<VioletCloth>(5)
				.AddIngredient<BlueCloth>(7)
				.AddTile(TileID.Loom)
				.Register();
        }
    }
}
