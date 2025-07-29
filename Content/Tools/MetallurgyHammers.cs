using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.MetallurgySeries.TemperedAlloys;
using DestroyerTest.Content.Resources.Cloths;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;



namespace DestroyerTest.Content.Tools
{
	public class SteelHammer : ModItem
	{
		public override void SetDefaults() {
			Item.width = 80; // The item texture's width.
			Item.height = 78; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 15; // The damage your item deals.
			Item.knockBack = 12; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 76; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 16); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<MetallurgyRarity>();
			Item.UseSound = SoundID.Item1;
            Item.hammer = 55;
		}


		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			// Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
			// 60 frames = 1 second
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.FlameWaders,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) });
		}


		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
    	CreateRecipe()
        	.AddIngredient<Steel>(10)
            .AddIngredient(ItemID.Wood, 8)
			.AddTile(TileID.WorkBenches)
        	.Register();
		}		
	}
	public class CuproNickelHammer : ModItem
	{
		public override void SetDefaults() {
			Item.width = 80; // The item texture's width.
			Item.height = 78; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 15; // The damage your item deals.
			Item.knockBack = 12; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 76; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 16); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<MetallurgyRarity>();
			Item.UseSound = SoundID.Item1;
            Item.hammer = 55;
		}


		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			// Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
			// 60 frames = 1 second
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.FlameWaders,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) });
		}


		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
    	CreateRecipe()
        	.AddIngredient<CuproNickel>(10)
            .AddIngredient(ItemID.Wood, 8)
			.AddTile(TileID.WorkBenches)
        	.Register();
		}		
	}
	public class NavalBrassHammer : ModItem
	{
		public override void SetDefaults() {
			Item.width = 80; // The item texture's width.
			Item.height = 78; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 15; // The damage your item deals.
			Item.knockBack = 12; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 76; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 16); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<MetallurgyRarity>();
			Item.UseSound = SoundID.Item1;
            Item.hammer = 55;
		}


		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			// Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
			// 60 frames = 1 second
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.FlameWaders,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) });
		}


		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
    	CreateRecipe()
        	.AddIngredient<NavalBrass>(10)
            .AddIngredient(ItemID.Wood, 8)
			.AddTile(TileID.WorkBenches)
        	.Register();
		}		
	}
	public class PhosphorBronzeHammer : ModItem
	{
		public override void SetDefaults() {
			Item.width = 80; // The item texture's width.
			Item.height = 78; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 15; // The damage your item deals.
			Item.knockBack = 12; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 76; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 16); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<MetallurgyRarity>();
			Item.UseSound = SoundID.Item1;
            Item.hammer = 55;
		}


		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			// Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
			// 60 frames = 1 second
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.FlameWaders,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) });
		}


		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
    	CreateRecipe()
        	.AddIngredient<PhosphorBronze>(10)
            .AddIngredient(ItemID.Wood, 8)
			.AddTile(TileID.WorkBenches)
        	.Register();
		}		
	}
}
