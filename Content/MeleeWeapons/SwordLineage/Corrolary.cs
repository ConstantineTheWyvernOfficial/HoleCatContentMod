using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Content.Projectiles;
  
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;

namespace DestroyerTest.Content.MeleeWeapons.SwordLineage
{
	
	public class Corrolary : ModItem
	{
		public int attackType = 0; // keeps track of which attack it is
		public int comboExpireTimer = 0; // we want the attack pattern to reset if the weapon is not used for certain period of time

		public override void SetDefaults()
		{
			Item.width = 112; // The item texture's width.
			Item.height = 112; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Shoot; // The useStyle of the Item.
			Item.useTime = 30; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 30; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 130; // The damage your item deals.
			Item.knockBack = 4f; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 26; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 16); // The value of the weapon in copper coins.
			Item.rare = ItemRarityID.Blue;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<CorrolarySwing>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			// Using the shoot function, we override the swing projectile to set ai[0] (which attack it is)
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, attackType);
			attackType = (attackType + 1) % 3; // Increment attackType to make sure next swing is different
			comboExpireTimer = 0; // Every time the weapon is used, we reset this so the combo does not expire
			return false; // return false to prevent original projectile from being shot
		}

		public override void UpdateInventory(Player player) {
			if (comboExpireTimer++ >= 120) // after 120 ticks (== 2 seconds) in inventory, reset the attack pattern
				attackType = 0;
		}

		public override bool MeleePrefix() {
			return true; // return true to allow weapon to have melee prefixes (e.g. Legendary)
		}

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LihzahrdBrick, 5)
                .AddIngredient(ModContent.ItemType<Item_HeliciteCrystal>(), 16)
                .AddIngredient(ModContent.ItemType<Living_Shadow>(), 8)
            .AddIngredient(ModContent.ItemType<Continuum>(), 1)
            .AddTile(TileID.LihzahrdAltar) // Use the correct TileID name if 16 is Anvils
            .Register();
            
            CreateRecipe()
				.AddIngredient(ItemID.LihzahrdBrick, 5)
                .AddIngredient(ModContent.ItemType<Item_HeliciteCrystal>(), 16)
				.AddIngredient(ModContent.ItemType<Living_Shadow>(), 8)
			.AddIngredient(ModContent.ItemType<Continuum>(), 1)
        	.AddTile(TileID.LihzahrdAltar) // Use the correct TileID name if 16 is Anvils
        	.Register();
		}	
	}
}
