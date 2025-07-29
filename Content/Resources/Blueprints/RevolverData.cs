using System;
using DestroyerTest.Common;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Resources.Blueprints
{
	public class RevolverData : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 10;
		}

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 5;
			Item.value = 1000;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.HoldUp;
			Item.rare = ModContent.RarityType<RiftRarity1>();
		}

        public override bool? UseItem(Player player) {
            SoundEngine.PlaySound(new SoundStyle("DestroyerTest/Assets/Audio/RadioBeep"), player.position);
            CombatText.NewText(player.Hitbox, ColorLib.Rift, "This a configuration of Data for making Handguns.");
            return true;
        }

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, ColorLib.Rift.ToVector3() * 0.55f * Main.essScale);
		}
        public override void AddRecipes() {
			CreateRecipe(1)
                .AddIngredient<RiftData>(1)
                .AddIngredient<Living_Shadow>(5)
                .AddIngredient(ItemID.FlintlockPistol, 1)
                .AddIngredient(ItemID.PhoenixBlaster, 1)
                .AddTile<Tile_RiftConfiguratorCore>()
            .Register();
	
			CreateRecipe(1)
				.AddIngredient<RiftData>(1)
				.AddIngredient<Living_Shadow>(5)
				.AddIngredient(ItemID.FlintlockPistol, 1)
                .AddIngredient(ItemID.PhoenixBlaster, 1)
				.AddTile<Tile_RiftConfiguratorArmory>()
			.Register();

            CreateRecipe(1)
				.AddIngredient<RiftData>(1)
				.AddIngredient<Living_Shadow>(5)
				.AddIngredient(ItemID.FlintlockPistol, 1)
                .AddIngredient(ItemID.PhoenixBlaster, 1)
				.AddTile<Tile_RiftConfiguratorTools>()
			.Register();

            CreateRecipe(1)
				.AddIngredient<RiftData>(1)
				.AddIngredient<Living_Shadow>(5)
				.AddIngredient(ItemID.FlintlockPistol, 1)
                .AddIngredient(ItemID.PhoenixBlaster, 1)
				.AddTile<Tile_RiftConfiguratorFurniture>()
			.Register();

            CreateRecipe(1)
				.AddIngredient<RiftData>(1)
				.AddIngredient<Living_Shadow>(5)
				.AddIngredient(ItemID.FlintlockPistol, 1)
                .AddIngredient(ItemID.PhoenixBlaster, 1)
				.AddTile<Tile_RiftConfiguratorWeaponry>()
			.Register();
		}
	}
}
