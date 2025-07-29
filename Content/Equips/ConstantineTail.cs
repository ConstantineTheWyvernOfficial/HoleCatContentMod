using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Equips
{
	[AutoloadEquip(EquipType.Back)]
	public class ConstantineTail : ModItem
	{
		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			// Set item dimensions (adjust these to your sprite size)
			Item.width = 24;
			Item.height = 10;

			// Set basic properties
			Item.value = Item.buyPrice(gold: 5); // Set price to 5 gold
			Item.rare = ItemRarityID.Blue;       // Set rarity to blue (tier 1)
			Item.accessory = true;              // Mark item as an accessory
		}
	}
}
