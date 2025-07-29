using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles.Riftplate;

namespace DestroyerTest.Content.RiftBiomeSpread
{
	public class RiftWretcher : ModItem
	{
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults() {
			Item.width = 93;
			Item.height = 26;
			Item.autoReuse = true;
			Item.damage = 1280;
			Item.DamageType = DamageClass.Default;
			Item.knockBack = 0f;
			Item.rare = ItemRarityID.LightRed; // Use a custom rarity if you have one, or use an existing one like ItemRarityID.LightRed
			Item.shootSpeed = 10f;
			Item.useAnimation = 5;
			Item.useTime = 5;
			Item.UseSound = new Terraria.Audio.SoundStyle ("FranciumCalamityWeapons/Audio/NightFlameShoot") with
			{
				Volume = 0.9f,
				PitchVariance = 0.2f,
				MaxInstances = 10,
			};
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.buyPrice(gold: 1);
			Item.shoot = ProjectileID.Flames;
			Item.useAmmo = ItemID.Gel;
		}

		public bool Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Spawn the projectile
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Flames, damage, knockback, player.whoAmI);
			return false; // Return false to prevent the default projectile from being fired
		}


		
	}
}
