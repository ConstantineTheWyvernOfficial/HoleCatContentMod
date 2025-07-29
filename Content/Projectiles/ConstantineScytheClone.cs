using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using DestroyerTest.Content.Dusts;
using System.IO;
using System;

namespace DestroyerTest.Content.Projectiles
{
    public class ConstantineScytheClone : ModProjectile
    {
        private int soundCooldown = 0; // Initialize a cooldown timer

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
        }
        
        private NPC HomingTarget {
				get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];
				set {
					Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
				}
			}

		public ref float DelayTimer => ref Projectile.ai[1];

        public override void SetDefaults()
        {
            Projectile.width = 94;
            Projectile.height = 102;
            Projectile.friendly = true;
            Projectile.penetrate = 1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120; // 10 seconds max lifespan
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(soundCooldown);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            soundCooldown = reader.ReadInt32();
        }

        
		public override bool PreDraw(ref Color lightColor) {
			// Draws an afterimage trail. See https://github.com/tModLoader/tModLoader/wiki/Basic-Projectile#afterimage-trail for more information.

			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = Projectile.oldPos.Length - 1; k > 0; k--) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Main.rand.NextBool(5)) // 33% chance per tick
            {
                SoundEngine.PlaySound(new SoundStyle("DestroyerTest/Assets/Audio/ConstitutionBoss/ConstitutionBossHit"), Projectile.Center);
                // Spawn a needle projectile in a random direction
                float angle = Main.rand.NextFloat(0, MathHelper.TwoPi);
                float speed = 8f; // Adjust speed as needed
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center + new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3)),
                    velocity,
                    ModContent.ProjectileType<ConstantineScytheNeedle>(),
                    Projectile.damage,
                    0,
                    player.whoAmI
                );
                

            }
            

            


            
            

           
			// Always spinning
			Projectile.rotation += 0.2f * Projectile.direction;

            float maxDetectRadius = 700f; // The maximum radius at which a projectile can detect a target

				// First, we find a homing target if we don't have one
				if (HomingTarget == null) {
					HomingTarget = FindClosestNPC(maxDetectRadius);
				}

				// If we have a homing target, make sure it is still valid. If the NPC dies or moves away, we'll want to find a new target
				if (HomingTarget != null && !IsValidTarget(HomingTarget)) {
					HomingTarget = null;
				}

				// If we don't have a target, don't adjust trajectory
				if (HomingTarget == null)
					return;

				// If found, we rotate the projectile velocity in the direction of the target.
				// We only rotate by 3 degrees an update to give it a smooth trajectory. Increase the rotation speed here to make tighter turns
				float length = Projectile.velocity.Length();
				float targetAngle = Projectile.AngleTo(HomingTarget.Center);
				Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(30)).ToRotationVector2() * length;
        

            

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
				return target.CanBeChasedBy();
			}

        






        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            

        }

        
    }
}

