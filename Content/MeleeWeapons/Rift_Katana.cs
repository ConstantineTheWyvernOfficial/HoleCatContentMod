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
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Resources.Blueprints;
using DestroyerTest.Content.Tiles.RiftConfigurator;

namespace DestroyerTest.Content.MeleeWeapons
{
	public class Rift_Katana : ModItem
	{
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults() {
			Item.damage = 140;
			Item.DamageType = DamageClass.MeleeNoSpeed; // Deals melee damage
			Item.width = 74;
			Item.height = 122;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.channel = true; 
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0;
			Item.value = 10000;
			Item.rare = ModContent.RarityType<RiftRarity1>();
			Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/TenebrousKatana/Slice", 3) with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Rift_Katana_Projectile>();
			Item.shootSpeed = 5f;
            Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand
		}

		

        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_Riftplate>(10)
				.AddIngredient<BroadswordData>(1)
				.AddIngredient<ShadowCircuitry>(3)
				.AddIngredient(ItemID.Katana, 1)
				.AddTile<Tile_RiftConfiguratorWeaponry>()
				.Register();
		}

	}
}
