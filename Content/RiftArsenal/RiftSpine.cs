using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.RogueItems;
using DestroyerTest.Content.Tiles.Riftplate;

namespace DestroyerTest.Content.RiftArsenal
{
	public class RiftSpine : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.ItemsThatCountAsBombsForDemolitionistToSpawn[Type] = true;
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 22f;
			Item.shoot = ModContent.ProjectileType<Projectiles.RiftSpine_Thrown>();
			Item.width = 24;
			Item.height = 96;
			Item.maxStack = 1;
			Item.consumable = false;
			Item.UseSound = SoundID.Item71;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = Item.buyPrice(0, 0, 20, 0);
			Item.rare = ModContent.RarityType<RiftRarity2>();
			Item.damage = 160;
			Item.autoReuse = true;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<RiftMaker>(2)
                .AddIngredient<Item_HeliciteCrystal>(15)
				.AddTile<Tile_RiftCrucible>()
                .AddCondition(Condition.DownedGolem)
				.Register();
		}
	}
}