using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Resources.Cloths;

namespace DestroyerTest.Content.Equips
{

	[AutoloadEquip(EquipType.Legs)]
	public class HoleCatSkirt : ModItem
	{
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Expert;
            Item.defense = 12;
		}

        public override void UpdateEquip(Player player)
        {
            
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.LunarBar, 7)
				.AddIngredient(ItemID.GoldBar, 20)
				.AddIngredient(ItemID.Ruby, 1)
                .AddIngredient<WhiteCloth>(17)
                .AddIngredient<Tenebris>(8)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}