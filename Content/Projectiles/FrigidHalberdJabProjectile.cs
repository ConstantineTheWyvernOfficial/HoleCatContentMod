using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Content.MeleeWeapons;

namespace DestroyerTest.Content.Projectiles
{
	public class FrigidHalberdJabProjectile : ModProjectile
	{
		
		// Define the range of the Spear Projectile. These are overridable properties, in case you'll want to make a class inheriting from this one.
		protected virtual float HoldoutRangeMin => 24f;
		protected virtual float HoldoutRangeMax => 96f;

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.Spear); // Clone the default values for a vanilla spear. Spear specific values set for width, height, aiStyle, friendly, penetrate, tileCollide, scale, hide, ownerHitCheck, and melee.
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner]; // Since we access the owner player instance so much, it's useful to create a helper local variable for this
			int duration = player.itemAnimationMax; // Define the duration the projectile will exist in frames

			player.heldProj = Projectile.whoAmI; // Update the player's held projectile id

			// Reset projectile time left if necessary
			if (Projectile.timeLeft > duration) {
				Projectile.timeLeft = duration;
			}

			//Projectile.velocity = Vector2.Normalize(Projectile.velocity); // Velocity isn't used in this spear implementation, but we use the field to store the spear's attack direction.

			float halfDuration = duration * 0.5f;
			float progress;

			// Here 'progress' is set to a value that goes from 0.0 to 1.0 and back during the item use animation.
			if (Projectile.timeLeft < halfDuration) {
				progress = Projectile.timeLeft / halfDuration;
			}
			else {
				progress = (duration - Projectile.timeLeft) / halfDuration;
			}

			// Move the projectile from the HoldoutRangeMin to the HoldoutRangeMax and back, using SmoothStep for easing the movement
			Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
			
			// Apply proper rotation to the sprite.
			if (Projectile.spriteDirection == -1) {
				// If sprite is facing left, rotate 45 degrees
				Projectile.rotation += MathHelper.PiOver2;
			}
			else {
				// If sprite is facing right, rotate 135 degrees
				Projectile.rotation += MathHelper.PiOver2;
			}
			

			//Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			// Avoid spawning dusts on dedicated servers
			if (!Main.dedServ) {
				if (Main.rand.NextBool(3)) {
					Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, Alpha: 255, Scale: 1.2f);
				}

				if (Main.rand.NextBool(4)) {
					Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Alpha: 255, Scale: 0.3f);
				}
			}
				return false; // Don't execute vanilla AI.
		}

			public override bool OnTileCollide(Vector2 oldVelocity)
			{
				// Example: Release ice shards into the air from the point of impact
				for (int i = 0; i < 5; i++) // Spawn 5 shards
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), 
											Projectile.position, 
											new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-5f, -2f)), 
											ProjectileID.FrostShard, // Replace with your custom shard type
											Projectile.damage / 2, 
											Projectile.knockBack, 
											Projectile.owner);
				}

				// Optionally bounce or stop here
				Projectile.velocity = Vector2.Zero; // Stop the projectile
				return true; // Return true to destroy the projectile, or false to keep it alive
			}


		
	}
}