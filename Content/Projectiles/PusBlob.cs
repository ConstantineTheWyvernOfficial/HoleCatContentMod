using DestroyerTest.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
	// This example is similar to the Wooden Arrow projectile
	public class PusBlob : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// If this arrow would have strong effects (like Holy Arrow pierce), we can make it fire fewer projectiles from Daedalus Stormbow for game balance considerations like this:
			//ProjectileID.Sets.FiresFewerFromDaedalusStormbow[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20; // The width of projectile hitbox
			Projectile.height = 30; // The height of projectile hitbox
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.timeLeft = 1200;
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}

		public override void AI() {
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 5f) {
				Projectile.ai[0] = 5f;
				Projectile.velocity.Y += 0.1f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}

            if (ScepterClassStats.BloodVialItem == false) {
                Projectile.Kill();
            }
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			return true;
        }

		public override void OnKill(int timeLeft) {
			SoundStyle Kill = new SoundStyle("DestroyerTest/Assets/Audio/BloodBlobKill");
			SoundEngine.PlaySound(Kill, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
			for (int i = 0; i < 5; i++) // Creates a splash of dust around the position the projectile dies.
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.ToxicBubble);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 0.9f;
			} 
		}
	}
}