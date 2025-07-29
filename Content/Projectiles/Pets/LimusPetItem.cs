using DestroyerTest.Content.Projectiles.Pets;
using DestroyerTest.Content.Resources.Cloths;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles.Pets
{
	public class LimusPetItem : ModItem
	{
		// Names and descriptions of all ExamplePetX classes are defined using .hjson files in the Localization folder
		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish); // Copy the Defaults of the Zephyr Fish Item.

			Item.shoot = ModContent.ProjectileType<LimusPet>(); // "Shoot" your pet projectile.
			Item.buffType = ModContent.BuffType<LimusBuff>(); // Apply buff upon usage of the Item.
		}

        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer) {
				player.AddBuff(Item.buffType, 3600);
			}
   			return true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<BlueCloth>(8)
                .AddIngredient(ItemID.Gel, 12)
				.AddTile(TileID.Solidifier)
				.Register();
		}
	}
}