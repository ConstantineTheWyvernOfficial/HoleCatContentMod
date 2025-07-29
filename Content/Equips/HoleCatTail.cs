using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Equips
{
	
	public class HoleCatTail : ModItem
	{

		public int equipBack = -1; // It would be best not to tamper with this.
        
        public override void Load() // This fetches the texture we need

        { 
            if (Main.netMode != NetmodeID.Server) {
                equipBack = EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Back}", EquipType.Back, this);
            }
        }


		public override void Unload()
        {
            equipBack = -1;
        }

        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server)
            {
            }
        }

		public override void SetDefaults()
		{
			// Set item dimensions (adjust these to your sprite size)
			Item.width = 16;
			Item.height = 20;
			Item.value = Item.buyPrice(gold: 5); // Set price to 5 gold
			Item.rare = ItemRarityID.Expert;       // Set rarity to blue (tier 1)
			Item.accessory = true;
			Item.vanity = true;              // Mark item as an accessory
		}
	}
}
