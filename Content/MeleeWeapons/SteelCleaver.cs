using DestroyerTest.Content.MetallurgySeries.TemperedAlloys;
using DestroyerTest.Content.Resources.Cloths;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;



namespace DestroyerTest.Content.MeleeWeapons
{
	public class SteelCleaver : ModItem
	{
		public override void SetDefaults() {
			Item.width = 66; // The item texture's width.
			Item.height = 56; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 30; // The damage your item deals.
			Item.knockBack = 12; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 8; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 16); // The value of the weapon in copper coins.
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/ConstitutionSlash") with {
				Volume = 1.0f, 
    			Pitch = 0.0f, 
    			PitchVariance = 0.5f, 
			}; // The sound when the weapon is being used.
		}

		//public override void MeleeEffects(Player player, Rectangle hitbox) {
			//if (Main.rand.NextBool(3)) {
				// Emit dusts when the sword is swung
				//Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.Sparkle>());
			//}
		//}

		//public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			// Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
			// 60 frames = 1 second
			//target.AddBuff(BuffID.OnFire, 60);
		//}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
    	CreateRecipe()
        	.AddIngredient<TemperedSteel>(10)
            .AddIngredient<GreenCloth>(3)
        	.AddTile(TileID.Anvils) // Use the correct TileID name if 16 is Anvils
        	.Register();
		}		
	}
}
