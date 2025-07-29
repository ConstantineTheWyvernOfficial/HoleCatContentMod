using rail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;

namespace DestroyerTest.Content.RiftBiome.RiftDesertResources
{
	public class Item_RiftSilt : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 10;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_RiftSilt>());
			Item.width = 12;
			Item.height = 12;
			Item.ammo = AmmoID.Sand;
			// Item.shoot and Item.damage are not used for sand ammo by convention. They would result in undesireable item tooltips.
			// ItemID.Sets.SandgunAmmoProjectileData is used instead.
			Item.notAmmo = true;
		}

		public override void AddRecipes() {
			CreateRecipe(10)
				.AddIngredient<Living_Shadow>()
                .AddIngredient(TileID.Silt)
				.AddTile(TileID.Blendomatic)
				.Register();
		}
	}
}