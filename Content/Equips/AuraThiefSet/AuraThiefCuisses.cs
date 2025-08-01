using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Resources;
using DestroyerTest.Rarity;
using System.Numerics;
using DestroyerTest.Content.Dusts;
using DestroyerTest.Content.Resources.Cloths;
using System.Drawing;

namespace DestroyerTest.Content.Equips.AuraThiefSet
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Legs value here will result in TML expecting a X_Legs.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Legs)]
	public class AuraThiefCuisses : ModItem
	{
		public override void SetDefaults() {
			Item.width = 22; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<LifeEchoRarity>(); // The rarity of the item
			Item.defense = 4; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
		if (player.velocity.Length() > 0)
        {
            Dust.NewDustDirect(player.Bottom, 2, 1, ModContent.DustType<SoulDust>(), 0, 0.02f, 100, new Microsoft.Xna.Framework.Color(184, 228, 242), 1);
        }
        }


		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<LifeEcho>(15)
                .AddIngredient<BlackCloth>(15)
                .AddIngredient(ItemID.Wood, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}