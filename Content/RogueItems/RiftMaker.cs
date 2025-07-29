using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;

namespace DestroyerTest.Content.RogueItems
{
	public class RiftMaker : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.ItemsThatCountAsBombsForDemolitionistToSpawn[Type] = true;
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<Projectiles.RiftMaker_Thrown>();
			Item.width = 14;
			Item.height = 56;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.UseSound = SoundID.Item71;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = Item.buyPrice(0, 0, 20, 0);
			Item.rare = ModContent.RarityType<RiftRarity1>();
			Item.damage = 160;
			Item.autoReuse = true;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_Riftplate>(5)
                .AddIngredient<Living_Shadow>(5)
                .AddIngredient(ItemID.Javelin)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}