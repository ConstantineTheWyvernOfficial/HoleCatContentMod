using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Equips
{
// This item is meant to mirror the effects of the Hallowed Plate Mail, which equips a Cape without needing a separate cape Item. 

	[AutoloadEquip(EquipType.Body)] // As usual, we must tell the game what part of the body the item will be equipped on.
    	public class TenebrousArchmageCoat : ModItem
		{
		public override void SetDefaults() // Simple item properties. Nothing new here.
		{
			Item.width = 30;
			Item.height = 22; 
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ModContent.RarityType<ShimmeringRarity>(); // The rarity of the item
			Item.defense = 43;
			// Now, in case you might be asking "Why use that special default when you can just copy what the original Hallowed Plate Mail does?"
			// Unfortunately for you, while cloning the defaults does load a cape on the back, it loads the Hallowed Armor cape, and replaces your body armor textures with the Hallowed Plate Mail Textures.
			//Item.CloneDefaults(ItemID.HallowedPlateMail);
		}

        public override void UpdateEquip(Player player) {
            player.moveSpeed *= 0.40f;

		}

		public override void AddRecipes() //Added to make the item obtainable without needing cheat mods, since many swear by never using cheats, ever.
		{
			CreateRecipe()
                .AddIngredient<HeliciteRobe>()
                .AddIngredient<ShimmeringSludge>(8)
				.AddIngredient<Tenebris>(12)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}