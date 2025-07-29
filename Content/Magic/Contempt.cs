using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using DestroyerTest.Content.Projectiles; // Add this line if CT3_Swing is in the Projectiles namespace
using DestroyerTest.Rarity;
using System.Linq;

namespace DestroyerTest.Content.Magic
{
	public class Contempt : ModItem
	{

		//Weapon Properties
		public override void SetDefaults()
		{
			// Common Properties
			Item.width = 50;
			Item.height = 50;
			Item.value = Item.sellPrice(gold: 25, silver: 70);
			Item.rare = ModContent.RarityType<CorruptionSpecialRarity>();

			// Use Properties
			// Note that useTime and useAnimation for this item don't actually affect the behavior because the held projectile handles that. 
			// Each attack takes a different amount of time to execute
			// Conforming to the item useTime and useAnimation makes it much harder to design
			// It does, however, affect the item tooltip, so don't leave it out.
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;

			// Weapon Properties
			Item.knockBack = 10;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.autoReuse = true; // This determines whether the weapon has autoswing
			Item.damage = 80; // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = DamageClass.Magic; // Deals melee damage\
			Item.channel = true;
			Item.mana = 25;
			Item.crit = 16; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.
			Item.noMelee = true;  // This makes sure the item does not deal damage from the swinging animation
			Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand

			// Projectile Properties
			Item.shoot = ModContent.ProjectileType<ContemptCursorProjectile>(); // The sword as a projectile
		}

		public override void HoldItem(Player player)
		{
			if (player.channel)
			{
				int manaCost = Item.mana;
				if (player.statMana < manaCost)
				{
					player.channel = false; // Stop channeling if not enough mana
				}
			}
		}
		
		public override bool CanUseItem(Player player)
		{
			return !Main.projectile.Any(proj => proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<ContemptCursorProjectile>());
		}


    }
} 