
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Common;

namespace DestroyerTest.Content.Equips
{
	public class StellarTintedGoggles : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 29;
			Item.height = 18;
			Item.maxStack = 1;
			Item.value = 100;
			Item.accessory = true;
		}

		

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			DTUtils.StellarGogglesEquipped = true;
		}
	}
}