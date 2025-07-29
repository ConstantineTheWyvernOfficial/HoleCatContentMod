
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Projectiles.AmmoProjectiles;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.Riftplate;


namespace DestroyerTest.Content.RiftArsenal
{
    public class RiftTome : ModItem
	{
        public override void SetStaticDefaults()
        { 
        }
        public override void SetDefaults() {
            Item.UseSound = SoundID.Item105;
			Item.width = 28; // The item texture's width.
			Item.height = 30; // The item texture's height.

			Item.useStyle = ItemUseStyleID.HoldUp; // The useStyle of the Item.
			Item.useTime = 45; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 45; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.
            Item.mana = 15;

			Item.DamageType = DamageClass.Magic; // Whether your item is part of the melee class.
			Item.damage = 60; // The damage your item deals.
			Item.knockBack = 0; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 4; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 1); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<RiftRarity1>(); // Give this item our custom rarity.
            Item.shoot = ModContent.ProjectileType<RiftFlare>();
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			float ceilingLimit = target.Y;
			if (ceilingLimit > player.Center.Y - 200f) {
				ceilingLimit = player.Center.Y - 200f;
			}
			// Loop these functions 3 times.
			for (int i = 0; i < 6; i++) {
				position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
				position.Y -= 100 * i;
				Vector2 heading = target - position;

				if (heading.Y < 0f) {
					heading.Y *= -1f;
				}

				if (heading.Y < 20f) {
					heading.Y = 20f;
				}

				heading.Normalize();
				heading *= velocity.Length();
				heading.Y += Main.rand.Next(-40, 41) * 0.02f;
				Projectile.NewProjectile(source, position, heading, type, damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
			}

			return false;
		}

        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Living_Shadow>(8)
				.AddIngredient<Item_Riftplate>(12)
                .AddIngredient(ItemID.SpellTome)
				.AddTile<Tile_RiftCrucible>()
				.Register();
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
	}
}