using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Resources;
  
using DestroyerTest.Content.Dusts;
using DestroyerTest.Rarity;
using Terraria.GameInput;
using DestroyerTest.Content.Projectiles.Tenebrouskatana;
using DestroyerTest.Common;
using DestroyerTest.Content.Particles;

namespace DestroyerTest.Content.MeleeWeapons
{
	public class Tenebrous_Katana : ModItem
	{
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults() {
			Item.damage = 200;
			Item.DamageType = DamageClass.MeleeNoSpeed; // Deals melee damage
			Item.width = 88;
			Item.height = 196;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.channel = true; 
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0;
			Item.value = 10000;
			Item.rare = ModContent.RarityType<ShimmeringRarity>();
			Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/TenebrousKatana/Slice", 3) with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Tenebrous_Katana_Projectile>();
			Item.shootSpeed = 5f;
            Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand
		}

		public int ComboExpireTimer = 0;

		public bool HasBursted = false;

		public int ComboPhase = 0; // 0 = Swing, 1 = Jab, 2 = Spin, 3 = Burst

        public override void UpdateInventory(Player player)
        {
			var Config = ModContent.GetInstance<DTConfig>();
			if (Config.UnnerfTenebrousKatana == true) {
				Item.damage = 3680;
			} 
			if (Config.UnnerfTenebrousKatana == false) {
				Item.damage = 200;
			}
			
			ComboExpireTimer++;

			if (ComboExpireTimer >= 500)
			{
				HasBursted = false;
				player.fullRotation = 0f;
				player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2); // Set rotation origin to the center of the player
				ComboExpireTimer = 0;
				ComboPhase = 0;
				Tenebrous_Katana_Projectile.Swing_HitCount = 0;
				TenebrisJabProjectile.Jab_HitCount = 0;
				TenebrousKatanaSpin.Spin_HitCount = 0;
				Item.shoot = ModContent.ProjectileType<Tenebrous_Katana_Projectile>();
				Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/TenebrousKatana/Slice", 3) with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
			}
            else if (ComboPhase == 0 && Tenebrous_Katana_Projectile.Swing_HitCount >= 15)
			{
				ComboExpireTimer = 0;
				ComboPhase = 1;
				Item.shoot = ModContent.ProjectileType<TenebrisJabProjectile>();
				Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/EnergyFlurry") with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
			}
			else if (ComboPhase == 1 && TenebrisJabProjectile.Jab_HitCount >= 15)
			{
				for (float rotation = 0; rotation < 360; rotation += 5f)
				{
					player.fullRotation = MathHelper.ToRadians(rotation);
					player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2); // Set rotation origin to the center of the player
				}
				ComboExpireTimer = 0;
				ComboPhase = 2;
				Item.shoot = ModContent.ProjectileType<TenebrousKatanaSpin>();
				Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/EnergyFlurry") with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
			}
			else if (ComboPhase == 2 && TenebrousKatanaSpin.Spin_HitCount >= 3)
			{
				ComboExpireTimer = 0;
				ComboPhase = 3;
				Item.shoot = ModContent.ProjectileType<Tenebrous_Katana_Projectile>();
				Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/TenebrousKatana/Slice", 3) with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
			}
			else if (ComboPhase == 3)
			{
				ComboExpireTimer = 0;
				TenebrousKatanaSpin.Spin_HitCount = 0;
				ComboPhase = 0;
				if (HasBursted == false)
				{
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<TenebrousBurstProjectile>(), 250, 0f, player.whoAmI);
				SoundEngine.PlaySound(new SoundStyle("DestroyerTest/Assets/Audio/Destitute") with {MaxInstances = 1});
				HasBursted = true;
				}
				Item.useTime = int.MaxValue; // Temporarily disable item usage
			}
			else if (HasBursted == true)
			{
				ComboExpireTimer = 500;
				ComboPhase = 0;
				Item.useTime = 40;
				Item.shoot = ModContent.ProjectileType<Tenebrous_Katana_Projectile>();
			}
        }

        public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<Tenebris>(10)
				.AddIngredient<ShimmeringSludge>(12)
                .AddIngredient<Rift_Katana>(1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}

	}

	public class TKPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
		{
			foreach (Projectile projectile in Main.projectile)
			{
				if (DestroyerTestMod.TenebrisTeleportKeybind.JustPressed && projectile.active && projectile.owner == Player.whoAmI && projectile.type == ModContent.ProjectileType<TenebrisClone>())
				{
					var StarBurstSound = new SoundStyle("DestroyerTest/Assets/Audio/RiftSwordMinionTeleport") with {
						Volume = 1.0f, 
						Pitch = 0.0f, 
						PitchVariance = 0.5f, 
					};
					SoundEngine.PlaySound(StarBurstSound);

					
					if (projectile.active && projectile.owner == Player.whoAmI && projectile.type == ModContent.ProjectileType<TenebrisClone>())
					{
							// Teleport the player to the projectile's center
							Player.position = projectile.Center - new Vector2(Player.width / 2, Player.height / 2);
							break;
					}
				}
			}
		}
    }
}
