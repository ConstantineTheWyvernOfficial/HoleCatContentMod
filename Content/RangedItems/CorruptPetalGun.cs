using System;
using System.Collections.Generic;
using System.Linq;
using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Projectiles.AmmoProjectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RangedItems
{
	public class CorruptPetalGun : ModItem
	{
		public override void SetDefaults() {
			// Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

			// Common Properties
			Item.width = 70; // Hitbox width of the item.
			Item.height = 30; // Hitbox height of the item.
			Item.rare = ItemRarityID.Green; // The color that the item's name will be in-game.

			// Use Properties
			Item.useTime = 10; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 10; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.

            // The sound that this item plays when used.
            Item.UseSound = SoundID.Item11;

			// Weapon Properties
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 46; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 5f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.

			// Gun Properties
			Item.shoot = ModContent.ProjectileType<CorruptPetal>(); // For some reason, all the guns in the vanilla source have this.
			Item.shootSpeed = 15f; // The speed of the projectile (measured in pixels per frame.) This value equivalent to Handgun
		}


		// This method lets you adjust position of the gun in the player's hands. Play with these values until it looks good with your graphics.
		public override Vector2? HoldoutOffset() {
			return new Vector2(2f, -2f);
		}

        public override bool CanUseItem(Player player)
        {
            return HasAmmo(player, ItemID.Vine);
        }

        public override bool? UseItem(Player player)
        {
            ConsumeAmmo(player, ItemID.Vine);
            return true;
        }


        private bool HasAmmo(Player player, int ammoType)
        {
            return GetAmmoItem(player, ammoType) != null;
        }

        private void ConsumeAmmo(Player player, int ammoType)
        {
            Item ammoItem = GetAmmoItem(player, ammoType);
            if (ammoItem != null)
            {
                ammoItem.stack--;
                if (ammoItem.stack <= 0)
                    ammoItem.TurnToAir();
            }
        }

        private Item GetAmmoItem(Player player, int ammoType)
        {
            List<Item[]> sources = new()
            {
                player.inventory,
                player.bank?.item,
                player.bank2?.item,
                player.bank3?.item,
                player.bank4?.item
            };

            foreach (var source in sources)
            {
                if (source == null) continue;
                Item item = source.FirstOrDefault(i => i.type == ammoType && i.stack > 0);
                if (item != null)
                    return item;
            }

            return null;
        }




		//TODO: Move this to a more specifically named example. Say, a paint gun?
        //public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
        // Every projectile shot from this gun has a 1/3 chance of being an ExampleInstancedProjectile
        //if (Main.rand.NextBool(3)) {
        //type = ProjectileID.GoldenShowerFriendly;
        //}
        //}

        /*
		* Feel free to uncomment any of the examples below to see what they do
		*/

        // What if I wanted it to work like Uzi, replacing regular bullets with High Velocity Bullets?
        // Uzi/Molten Fury style: Replace normal Bullets with High Velocity
        /*public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			if (type == ProjectileID.Bullet) { // or ProjectileID.WoodenArrowFriendly
				type = ProjectileID.BulletHighVelocity; // or ProjectileID.FireArrow;
			}
		}*/

        // What if I wanted multiple projectiles in a even spread? (Vampire Knives)
        // Even Arc style: Multiple Projectile, Even Spread
        /*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			float numberProjectiles = 3 + Main.rand.Next(3); // 3, 4, or 5 shots
			float rotation = MathHelper.ToRadians(45);

			position += Vector2.Normalize(velocity) * 45f;
			velocity *= 0.2f; // Slow the projectile down to 1/5th speed so we can see it. This is only here because this example shares ModItem.SetDefaults code with other examples. If you are making your own weapon just change Item.shootSpeed as normal.

			for (int i = 0; i < numberProjectiles; i++) {
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}

			return false; // return false to stop vanilla from calling Projectile.NewProjectile.
		}*/

        // How can I make the shots appear out of the muzzle exactly?
        // Also, when I do this, how do I prevent shooting through tiles?
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }

		// How can I get a "Clockwork Assault Rifle" effect?
		// 3 round burst, only consume 1 ammo for burst. Delay between bursts, use reuseDelay
		// Make the following changes to SetDefaults():
		/*
			item.useAnimation = 12;
			item.useTime = 4; // one third of useAnimation
			item.reuseDelay = 14;
			item.consumeAmmoOnLastShotOnly = true;
		*/

		// How can I shoot 2 different projectiles at the same time?
		/*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			// Here we manually spawn the 2nd projectile, manually specifying the projectile type that we wish to shoot.
			Projectile.NewProjectile(source, position, velocity, ProjectileID.GrenadeI, damage, knockback, player.whoAmI);

			// By returning true, the vanilla behavior will take place, which will shoot the 1st projectile, the one determined by the ammo.
			return true;
		}*/

		// How can I choose between several projectiles randomly?
		/*public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			// Here we randomly set type to either the original (as defined by the ammo), a vanilla projectile, or a mod projectile.
			type = Main.rand.Next(new int[] { type, ProjectileID.GoldenBullet, ModContent.ProjectileType<Projectiles.ExampleBullet>() });
		}*/
	}
}