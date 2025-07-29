using System;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Dusts;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.SummonItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
    public class PossessedDartRifle : ModProjectile
    {
        private NPC HomingTarget {
			get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];
			set {
				Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
			}
		}

        public override void SetDefaults()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.width = 64;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
            Projectile.minion = true;
            Projectile.minionSlots = 10;
        }

        public int ShootTimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = Projectile.direction;
            Projectile.Center = player.Center;
            float radius = 80f; // Distance from center
            float speed = 0.05f; // Rotation speed (radians per tick)
            float angle = Main.GameUpdateCount * speed; // Angle increases over time

            // Count how many PossessedDartRifle projectiles the player owns
            int total = 0;
            int index = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && proj.type == Projectile.type)
                {
                    if (proj.whoAmI == Projectile.whoAmI)
                        index = total;
                    total++;
                }
            }

            // Calculate spacing angle
            float spacing = MathHelper.TwoPi / (total == 0 ? 1 : total);
            float myAngle = angle + index * spacing;

            Vector2 center = player.Center; // The point to orbit around
            Vector2 offset = new Vector2(MathF.Cos(myAngle), MathF.Sin(myAngle)) * radius;
            Projectile.position = center + offset - new Vector2(Projectile.width / 2, Projectile.height / 2);


            Projectile.rotation = angle * Projectile.direction;




            if (player.HasBuff<DartRifleMinionBuff>())
            {
                Projectile.timeLeft = 60;
            }


            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }

            float maxDetectRadius = 1200f; // The maximum radius at which a projectile can detect a target

            // First, we find a homing target if we don't have one
            if (HomingTarget == null)
            {
                HomingTarget = FindClosestNPC(maxDetectRadius);
            }

            // If we have a homing target, make sure it is still valid. If the NPC dies or moves away, we'll want to find a new target
            if (HomingTarget != null && !IsValidTarget(HomingTarget))
            {
                HomingTarget = null;
            }

            // If we don't have a target, don't adjust trajectory
            if (HomingTarget == null)
                return;

            // If found, we rotate the projectile velocity in the direction of the target.
            // We only rotate by 3 degrees an update to give it a smooth trajectory. Increase the rotation speed here to make tighter turns
            float length = Projectile.velocity.Length();
            Projectile.rotation = Projectile.AngleTo(HomingTarget.Center) * Projectile.direction;
            Vector2 ToEnemy = HomingTarget.Center - Projectile.Center;
            ShootTimer++;
            if (ShootTimer >= 30)
            {
                SoundEngine.PlaySound(SoundID.Item99, Projectile.Center);
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, ToEnemy, ProjectileID.CursedDart, 25, 3);

                ShootTimer = 0;
            }
            Projectile.spriteDirection = Projectile.direction;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            SoundEngine.PlaySound(SoundID.Item25, Projectile.position);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        
        // Finding the closest NPC to attack within maxDetectDistance range
		// If not found then returns null
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
       
	}
}