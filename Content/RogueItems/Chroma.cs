using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RogueItems
{
	public class Chroma : ModItem
	{
		public override void SetDefaults() {
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools

			// Common Properties
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(silver: 5);
			Item.maxStack = 1;

			// Use Properties
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 10;
			Item.useTime = 10;
            Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/Chroma_Throw") {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
			Item.autoReuse = true;
			Item.consumable = false;

			// Weapon Properties			
			Item.damage = 180;
			Item.knockBack = 15f;
			Item.noUseGraphic = true; // The item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item
			Item.DamageType = DamageClass.Ranged;

			// Projectile Properties
			Item.shootSpeed = 25f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Chroma_Projectile>(); // The projectile that will be thrown
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ItemID.Diamond, 8)
                .AddIngredient(ItemID.FragmentStardust, 5)
                .AddIngredient(ItemID.FragmentVortex, 5)
                .AddIngredient(ItemID.SilverBar, 15)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}