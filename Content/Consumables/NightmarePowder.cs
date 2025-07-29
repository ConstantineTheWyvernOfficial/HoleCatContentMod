using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using DestroyerTest.Content.Projectiles; // Add this line if CT3_Swing is in the Projectiles namespace
using DestroyerTest.Rarity;
using System.Linq;

namespace DestroyerTest.Content.Consumables
{
	public class NightmarePowder : ModItem
	{

        //Weapon Properties
        public override void SetDefaults()
        {
            // Common Properties
            Item.width = 50;
            Item.height = 50;
            Item.value = Item.sellPrice(gold: 25, silver: 70);
            Item.rare = ModContent.RarityType<CorruptionSpecialRarity>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;  // This makes sure the item does not deal damage from the swinging animation
            Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand

            // Projectile Properties
            Item.shoot = ModContent.ProjectileType<NightmarePowderProjectile>(); // The sword as a projectile
            Item.shootSpeed = 10;
		}
    }
} 