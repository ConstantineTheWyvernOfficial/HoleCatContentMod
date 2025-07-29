using DestroyerTest.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Tiles
{
    public class Item_NightmareRoseTrophy : ModItem
    {
        public override void SetDefaults()
        {
            // Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle as well as setting a few values that are common across all placeable items
            Item.DefaultToPlaceableTile(ModContent.TileType<Tile_NightmareRoseTrophy>());

            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 1);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.CorruptSeeds, 4)
                .AddIngredient(ItemID.DemoniteBar, 12)
                .AddCondition(Terraria.Localization.Language.GetOrRegister("Mods.DestroyerTest.RecipeCondition.DefeatNightmareRose", () => "Defeat Nightmare Rose"), () => DownedBossSystem.downedNightmareRoseBoss)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
	}
}