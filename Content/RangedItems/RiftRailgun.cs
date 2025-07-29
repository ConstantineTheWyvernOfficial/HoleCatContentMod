using System;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace DestroyerTest.Content.RangedItems
{
	// Token: 0x0200023E RID: 574
	public class RiftRailgun : ModItem
	{
        SoundStyle ChargeUp = new SoundStyle("DestroyerTest/Assets/Audio/RailGunCharge");

		// Token: 0x06000FBB RID: 4027 RVA: 0x0006E8A0 File Offset: 0x0006CAA0
		public override void SetDefaults() {
			Item.width = 120;
			Item.height = 26;
			Item.rare = ModContent.RarityType<RiftRarity1>(); // Set the rarity of the item, this will determine the color of the item's name in-game. Use ModContent.RarityType<YourRarityClass>() to set a custom rarity.
			Item.value = Item.buyPrice(0, 22, 10, 0);
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.channel = true;
			Item.autoReuse = true;
			Item.UseSound = ChargeUp;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 4;
			Item.damage = 300;
			Item.crit = 2;
			Item.knockBack = 1f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<RiftRailgunLaser>();
			Item.shootSpeed = 14f;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0006E108 File Offset: 0x0006C308
		//public override Vector2? HoldoutOffset() {
			//return new Vector2?(new Vector2(-100f, 0f));
		//}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0006E9AD File Offset: 0x0006CBAD
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_Riftplate>(12)
				.AddIngredient<Living_Shadow>(8)
				.AddIngredient<ShadowCircuitry>(16)
				.AddIngredient<Motherboard>(2)
				.AddIngredient(ItemID.IllegalGunParts)
				.AddTile<Tile_RiftConfiguratorCore>()
				//.AddCondition(Language.GetText("Mods.DestroyerTest.RecipeCondition.WeaponryDrive"), 
    			//() => Main.LocalPlayer.GetModPlayer<RiftConfiguratorPlayer>().WeaponryDrive)
				.Register();
		}
	}
}
