using DestroyerTest.Content.Projectiles;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Magic
{
	public class StellarFlames : ModItem
	{
		public override void SetDefaults() {
            
			// DefaultToStaff handles setting various Item values that magic staff weapons use.
            // Hover over DefaultToStaff in Visual Studio to read the documentation!
            // Shoot a black bolt, also known as the projectile shot from the onyx blaster.
            Item.DefaultToStaff(ModContent.ProjectileType<StellarFlameFriendly>(), 20, 2, 8);
			Item.width = 14;
			Item.height = 15;
			Item.UseSound = new Terraria.Audio.SoundStyle ("DestroyerTest/Assets/Audio/RiftWretcher") with
			{
				Volume = 0.9f,
				PitchVariance = 0.2f,
				MaxInstances = 100,
			};

			// A special method that sets the damage, knockback, and bonus critical strike chance.
			// This weapon has a crit of 32% which is added to the players default crit chance of 4%
			Item.SetWeaponValues(12, 2, 32);

			Item.SetShopValues(ItemRarityColor.LightRed4, 10000);
		}

		
		
		
	}
}