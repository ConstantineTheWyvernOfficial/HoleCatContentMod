using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.MetallurgySeries; // Add this line if CT3_Swing is in the Projectiles namespace

namespace DestroyerTest.Content.MeleeWeapons
{
	public class Icemourne : ModItem
	{

        //Weapon Properties
        public override void SetDefaults() {
			// Common Properties
			Item.width = 76;
			Item.height = 76;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ItemRarityID.Pink;

			// Use Properties
			// Note that useTime and useAnimation for this item don't actually affect the behavior because the held projectile handles that. 
			// Each attack takes a different amount of time to execute
			// Conforming to the item useTime and useAnimation makes it much harder to design
			// It does, however, affect the item tooltip, so don't leave it out.
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item66;

			// Weapon Properties
			Item.knockBack = 30;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.autoReuse = true; // This determines whether the weapon has autoswing
			Item.damage = 25; // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = DamageClass.Melee; // Deals melee damage
            Item.crit = 16; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			// Projectile Properties
			Item.shoot = ModContent.ProjectileType<IcemourneWave>(); // The sword as a projectile
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			float adjustedItemScale = player.GetAdjustedItemScale(Item); // Get the melee scale of the player and item.
			Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
			NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI); // Sync the changes in multiplayer.

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

		//Hit Inflictions
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
				player.AddBuff(BuffID.Chilled, 60);
				target.AddBuff(BuffID.Frostburn, 600);
			}

	

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.IronBar, 6)
                .AddIngredient(ItemID.IceBlock, 25)
                .AddIngredient<Steel>(4)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
    }
} 