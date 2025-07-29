using DestroyerTest.Content.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using Terraria;

namespace DestroyerTest.Content.MeleeWeapons
{

	public class Horizon : ModItem
	{
		public override void SetDefaults()
		{
			Item.height = 39;
			Item.width = 39;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = ItemUseStyleID.Shoot;

			Item.shoot = ModContent.ProjectileType<GargantuaProjectile>();
			Item.damage = 20;
			Item.shootSpeed = 10;
			Item.channel = true;
			Item.noUseGraphic = true;
		}

        public override bool CanUseItem(Player player)
        {
			return player.ownedProjectileCounts[Item.shoot] < 1;
        }

	}
}