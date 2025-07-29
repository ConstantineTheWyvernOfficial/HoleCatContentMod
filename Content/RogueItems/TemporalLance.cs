using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyerTest.Content.RogueItems
{
    public class TemporalLance : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<Projectiles.TemporalLance>();
            Item.width = 18;
            Item.height = 98;
            Item.UseSound = SoundID.Item71;
            Item.useAnimation = 30; // Increased for custom animation timing
            Item.useTime = 30;      // Match useAnimation for proper animation flow
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.damage = 160;
            Item.autoReuse = false; // Prevents animation overlap
        }

		public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofFright, 10)
                .AddIngredient(ItemID.LunarOre, 8)
                .AddIngredient(ModContent.ItemType<GalantineLance>(), 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}