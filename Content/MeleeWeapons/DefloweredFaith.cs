  
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Equips;



namespace DestroyerTest.Content.MeleeWeapons
{
	public class DefloweredFaith : ModItem
	{
		public override void SetDefaults() {
			Item.width = 96; // The item texture's width.
			Item.height = 97; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 28; // The damage your item deals.
			Item.knockBack = 12; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 6; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 16); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<CorruptionSpecialRarity>();
			Item.UseSound = Item.UseSound = SoundID.Item71;
		}

		//public override void MeleeEffects(Player player, Rectangle hitbox) {
			//if (Main.rand.NextBool(3)) {
				// Emit dusts when the sword is swung
				//Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.Sparkle>());
			//}
		//}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            player.AddBuff(BuffID.Lifeforce, 360);
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ItemID.DemoniteBar, 8)
			.AddIngredient<GildedCross>()
        	.AddTile(TileID.Anvils)
        	.Register();
		}		
	}
}
