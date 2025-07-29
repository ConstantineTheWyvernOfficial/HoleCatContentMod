
using DestroyerTest.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
	public class RiftYoyoT3Projectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			// The following sets are only applicable to yoyo that use aiStyle 99.

			// YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
			// Vanilla values range from 3f (Wood) to 16f (Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 3.5f;

			// YoyosMaximumRange is the maximum distance the yoyo sleep away from the player. 
			// Vanilla values range from 130f (Wood) to 400f (Terrarian), and defaults to 200f.
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;

			// YoyosTopSpeed is top speed of the yoyo Projectile.
			// Vanilla values range from 9f (Wood) to 17.5f (Terrarian), and defaults to 10f.
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;
		}

        public override void SetDefaults()
        {
            Projectile.width = 20; // The width of the projectile's hitbox.
            Projectile.height = 20; // The height of the projectile's hitbox.

            Projectile.aiStyle = ProjAIStyleID.Yoyo; // The projectile's ai style. Yoyos use aiStyle 99 (ProjAIStyleID.Yoyo). A lot of yoyo code checks for this aiStyle to work properly.

            Projectile.friendly = true; // Player shot projectile. Does damage to enemies but not to friendly Town NPCs.
            Projectile.DamageType = DamageClass.MeleeNoSpeed; // Benefits from melee bonuses. MeleeNoSpeed means the item will not scale with attack speed.
            Projectile.penetrate = -1; // All vanilla yoyos have infinite penetration. The number of enemies the yoyo can hit before being pulled back in is based on YoyosLifeTimeMultiplier.
                                       // Projectile.scale = 1f; // The scale of the projectile. Most yoyos are 1f, but a few are larger. The Kraken is the largest at 1.2f
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}

        // notes for aiStyle 99: 
        // localAI[0] is used for timing up to YoyosLifeTimeMultiplier
        // localAI[1] can be used freely by specific types
        // ai[0] and ai[1] usually point towards the x and y world coordinate hover point
        // ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
        // ai[0] being negative makes the yoyo move back towards the player
        // Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.
        private NPC ShotTarget {
            get {
                int targetIndex = (int)Projectile.ai[0] - 1;
                if (targetIndex < 0 || targetIndex >= Main.maxNPCs || !Main.npc[targetIndex].active)
                {
                    return null; // Ensure index is valid and NPC is active
                }
                return Main.npc[targetIndex];
            }
            set {
                Projectile.ai[0] = (value == null) ? 0 : value.whoAmI + 1;
            }
        }
        public override void AI()
        {
           float maxDetectRadius = 200.0f;

            if (ShotTarget == null)
            {
                ShotTarget = FindClosestNPC(maxDetectRadius);
            }

            if (ShotTarget != null && !IsValidTarget(ShotTarget))
            {
                ShotTarget = null;
            }

            if (ShotTarget == null)
                return;

            // Fire a bullet every 30 ticks (0.5 seconds)
            if (Projectile.ai[1] % 15 == 0) 
            {
                SoundEngine.PlaySound(SoundID.Item91, Projectile.position);
                FireBullet();
            }

            Projectile.ai[1]++; // Increment timer
        }

        public NPC FindClosestNPC(float maxDetectDistance) {
				NPC closestNPC = null;

				// Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
				float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

				// Loop through all NPCs
				foreach (var target in Main.ActiveNPCs) {
					// Check if NPC able to be targeted. 
					if (IsValidTarget(target)) {
						// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
						float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

						// Check if it is within the radius
						if (sqrDistanceToTarget < sqrMaxDetectDistance) {
							sqrMaxDetectDistance = sqrDistanceToTarget;
							closestNPC = target;
						}
					}
				}

				return closestNPC;
			}

			public bool IsValidTarget(NPC target) {
				// This method checks that the NPC is:
				// 1. active (alive)
				// 2. chaseable (e.g. not a cultist archer)
				// 3. max life bigger than 5 (e.g. not a critter)
				// 4. can take damage (e.g. moonlord core after all it's parts are downed)
				// 5. hostile (!friendly)
				// 6. not immortal (e.g. not a target dummy)
				// 7. doesn't have solid tiles blocking a line of sight between the projectile and NPC
				return target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, target.position, target.width, target.height);
			}

            private void FireBullet()
            {
                if (Main.myPlayer != Projectile.owner || ShotTarget == null)
                    return; // Only the owner should fire bullets, and we need a valid target.

                Vector2 shootDirection = ShotTarget.Center - Projectile.Center; // Get direction to target
                shootDirection.Normalize(); // Normalize to maintain consistent speed
                shootDirection *= 10f; // Adjust speed as needed

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(), // Projectile source
                    Projectile.Center,               // Starting position
                    shootDirection,                   // Velocity
                    ModContent.ProjectileType<RiftBolt>(),              // Type of projectile (change as needed)
                    Projectile.damage,                // Damage
                    Projectile.knockBack,             // Knockback
                    Main.myPlayer                     // Owner
                );
            }

        public override void PostAI() {
			if (Main.rand.NextBool(5)) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava); // Makes the projectile emit dust.
			}
		}
	}
}