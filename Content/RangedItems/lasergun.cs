/*
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace DestroyerTest.Content.RangedItems
{
	// Token: 0x0200023E RID: 574
	public class Lasergun : ModItem
	{
		// Token: 0x06000FBB RID: 4027 RVA: 0x0006E8A0 File Offset: 0x0006CAA0
		public override void SetDefaults() {
			Item.width = 56;
			Item.height = 26;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.buyPrice(0, 22, 10, 0);
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.channel = true;
			Item.autoReuse = true;
			Item.UseSound = new SoundStyle?(SoundID.Item13);
			Item.DamageType = DamageClass.Magic;
			Item.mana = 4;
			Item.damage = 105;
			Item.crit = 2;
			Item.knockBack = 1f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<LasergunLaser>();
			Item.shootSpeed = 14f;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0006E108 File Offset: 0x0006C308
		public override Vector2? HoldoutOffset() {
			return new Vector2?(new Vector2(-13f, 0f));
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0006E9AD File Offset: 0x0006CBAD
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.DirtBlock, 4)
				.Register();
		}
	}
}
*/
