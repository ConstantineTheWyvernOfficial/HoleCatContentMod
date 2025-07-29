using DestroyerTest.Content.Projectiles;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace DestroyerTest.Content.MeleeWeapons
{
	public class Endemy2Weapon : ModItem
	{
		public int attackType = 0; // keeps track of which attack it is
		public int comboExpireTimer = 0; // we want the attack pattern to reset if the weapon is not used for certain period of time

		// To add a glow texture, override the GetGlowMaskTexture method or handle it in the drawing code.
		// Example for tModLoader 1.4+:
		public override string Texture => "DestroyerTest/Content/MeleeWeapons/Endemy2Weapon";

		
		public override void PostDrawInInventory(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/MeleeWeapons/Endemy2Weapon_Highlight").Value;
			spriteBatch.Draw(glowTexture, position, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
		}
		
		public override void PostDrawInWorld(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/MeleeWeapons/Endemy2Weapon_Highlight").Value;
			spriteBatch.Draw(glowTexture, Item.position - Main.screenPosition, null, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
		}
		public override void SetDefaults()
        {
            Item.width = 140; // The item texture's width.
            Item.height = 142; // The item texture's height.

            Item.useStyle = ItemUseStyleID.Shoot; // The useStyle of the Item.
            Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
            Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
            Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

            Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
            Item.damage = 1200; // The damage your item deals.
            Item.knockBack = 9f; // The force of knockback of the weapon. Maximum is 20
            Item.crit = 66; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

            Item.value = Item.buyPrice(gold: 16); // The value of the weapon in copper coins.
            Item.rare = ModContent.RarityType<EndemyRarity>(); 
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<Endemy2Swing>();
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			// Using the shoot function, we override the swing projectile to set ai[0] (which attack it is)
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, attackType);
			attackType = (attackType + 1) % 2; // Increment attackType to make sure next swing is different
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


		

		
	}
}
