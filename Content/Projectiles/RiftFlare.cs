using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
	// This example is similar to the Wooden Arrow projectile
	public class RiftFlare : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// If this arrow would have strong effects (like Holy Arrow pierce), we can make it fire fewer projectiles from Daedalus Stormbow for game balance considerations like this:
			//ProjectileID.Sets.FiresFewerFromDaedalusStormbow[Type] = true;
            Projectile.CloneDefaults(ProjectileID.Starfury);
		}

		public override void SetDefaults()
		{
			Projectile.width = 10; // The width of projectile hitbox
			Projectile.height = 18; // The height of projectile hitbox
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 240;
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}

		public override void AI() {
			// If this is the first tick, set the initial velocity towards the player's cursor
			if (Projectile.ai[0] == 0) {
				// Get the player's cursor position in world coordinates
				Vector2 cursorPosition = Main.MouseWorld;

				// Calculate the direction vector from the projectile to the cursor
				Vector2 direction = cursorPosition - Projectile.Center;
				direction.Normalize();

				// Set the projectile's velocity
				Projectile.velocity = direction * 10f; // Adjust speed as needed

				// Mark the projectile as initialized
				Projectile.ai[0] = 1;
			}

			// Rotate the projectile to match its velocity direction
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}

		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item88, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
			for (int i = 0; i < 5; i++) // Creates a splash of dust around the position the projectile dies.
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 0.9f;
			} 
		}
	}
}