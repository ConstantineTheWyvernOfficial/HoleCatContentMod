using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;


namespace DestroyerTest.Content.Magic
{
    	public class CombatRoulette : ModItem
	{
        public override void SetDefaults() {
            Item.UseSound = new SoundStyle ("DestroyerTest/Assets/Audio/PartySword");
			Item.width = 48; // The item texture's width.
			Item.height = 48; // The item texture's height.

			Item.useStyle = ItemUseStyleID.HoldUp; // The useStyle of the Item.
			Item.useTime = 1200; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = false; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Generic; // Whether your item is part of the melee class.
			Item.damage = 0; // The damage your item deals.
			Item.knockBack = 0; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 0; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 1); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<GoliathRarity>(); // Give this item our custom rarity.
		}

        public void Calculations()
        {
        }



        

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
	}
}