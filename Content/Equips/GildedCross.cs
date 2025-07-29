
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Equips
{
	public class GildedCross : ModItem
	{
		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = 1;
			Item.value = 100;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
		}
	}
}