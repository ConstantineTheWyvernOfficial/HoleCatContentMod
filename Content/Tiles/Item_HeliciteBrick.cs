using DestroyerTest.Content.Tiles.RiftConfigurator;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles
{
	public class Item_HeliciteBrick: ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 100;
			ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
			
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tile_HeliciteBrick>());
			Item.width = 12;
			Item.height = 12;
			Item.value = 3000;
		}

        public override void AddRecipes()
        {
            CreateRecipe(16)
                .AddIngredient<Item_HeliciteCrystal>(4)
                .AddTile<Tile_RiftConfiguratorCore>()
                .Register();
        }
    }
}