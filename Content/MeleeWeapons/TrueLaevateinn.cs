  
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.MeleeWeapons
{
	// This is a copy of the Excalibur
	public class TrueLaevateinn : ModItem
	{
		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.damage = 180;
			Item.knockBack = 4.5f;
			Item.width = 40;
			Item.height = 40;
			Item.scale = 1f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.buyPrice(gold: 23); // Sell price is 5 times less than the buy price.
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ModContent.ProjectileType<LaevateinnArc>();
			Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
			Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			float adjustedItemScale = player.GetAdjustedItemScale(Item); // Get the melee scale of the player and item.

			// Spawn the regular arc (LaevateinnArc)
			Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ModContent.ProjectileType<LaevateinnArc>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);

			// Spawn the true arc (TrueLaevateinnArc)
			Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), ModContent.ProjectileType<TrueLaevateinnArc>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);

			// Sync the changes in multiplayer
			NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);

			return false; // Prevent vanilla behavior from shooting an additional projectile
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Laevateinn>(1)
                .AddIngredient(ItemID.BrokenHeroSword, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}