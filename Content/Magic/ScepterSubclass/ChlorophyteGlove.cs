
using DestroyerTest.Common;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Magic.ScepterSubclass
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.HandsOn)]
	public class ChlorophyteGlove : ModItem
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
            ScepterClassStats.Range += 6;
		}

        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.ChlorophyteBar, 12)
				.AddCondition(Condition.InJungle)
                .AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
