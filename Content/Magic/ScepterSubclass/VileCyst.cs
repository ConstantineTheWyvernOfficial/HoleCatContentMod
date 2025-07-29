
using System;
using DestroyerTest.Common;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Magic.ScepterSubclass
{
	public class VileCyst : ModItem
	{

		public override void SetDefaults()
		{
			Item.width = 22; // Width of the item
			Item.height = 28; // Height of the item
			Item.value = Item.sellPrice(gold: 86); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.vanity = false;
			Item.accessory = true;

		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage<ScepterClass>() *= 1.02f;
			ScepterClassStats.VileCystItem = true;
		}

		public override void UpdateInventory(Player player)
		{
			if (!Array.Exists(player.armor, armorItem => armorItem == Item))
			{
				ScepterClassStats.VileCystItem = false;
			}

		}
		
		public override void AddRecipes()
        {
			CreateRecipe()
				.AddIngredient(ItemID.RottenChunk, 16)
				.AddTile(TileID.DemonAltar)
				.Register();
        }
	}
}
