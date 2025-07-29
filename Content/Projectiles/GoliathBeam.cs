using DestroyerTest.Common;
using DestroyerTest.Content.Particles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
    public class GoliathBeam : ModProjectile
		{
			// Store the target NPC using Projectile.ai[0]
			private NPC HomingTarget {
				get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];
				set {
					Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
				}
			}

			public ref float DelayTimer => ref Projectile.ai[1];

			public override void SetStaticDefaults() {
				ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
				ProjectileID.Sets.TrailingMode[Type] = 2;
            	ProjectileID.Sets.TrailCacheLength[Type] = 12;
			}

		public override void SetDefaults()
		{
			Projectile.width = 38; // The width of projectile hitbox
			Projectile.height = 38; // The height of projectile hitbox
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.light = 1f; // How much light emit around the projectile
			Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
			}

			public override bool PreDraw(ref Color lightColor) {
            // Draws an afterimage trail. See https://github.com/tModLoader/tModLoader/wiki/Basic-Projectile#afterimage-trail for more information.
                Color BeamColor = new Color(Main.DiscoR / 2, (byte)(Main.DiscoG / 1.25f), (byte)(Main.DiscoB / 1.5f));
                lightColor = BeamColor;

                Texture2D texture = TextureAssets.Projectile[Type].Value;

                Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
                for (int k = Projectile.oldPos.Length - 1; k > 0; k--) {
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }

                // Apply custom color to the projectile
                

                // Draw the projectile with the custom color
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, BeamColor, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0);

                return false;
            }

			// Custom AI
			public override void AI() {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

                
				float maxDetectRadius = 800f; // The maximum radius at which a projectile can detect a target
                if (DelayTimer < 32 && DelayTimer > 2)
                {
                    NPC previewTarget = FindClosestNPC(maxDetectRadius);
                    if (previewTarget != null)
                    {
                        float t = (DelayTimer - 2f) / 6f;
                        float targetAngleprev = Projectile.AngleTo(previewTarget.Center);
                        Projectile.rotation = MathHelper.SmoothStep(Projectile.velocity.ToRotation() + MathHelper.PiOver4, targetAngleprev + MathHelper.PiOver4, t);
                    }
                }
                if (DelayTimer < 32 && DelayTimer >= 31)
                {
                    SoundEngine.PlaySound(SoundID.Item60, Projectile.Center);
                    PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp2>(), Projectile.Center, Vector2.Zero, new Color(Main.DiscoR / 2, (byte)(Main.DiscoG / 1.25f), (byte)(Main.DiscoB / 1.5f)), 0.2f);
                }
                if (DelayTimer < 32)
                {
                    Projectile.velocity *= 0.9f;
                    DelayTimer++;
                    return;
                }
                
                

				// First, we find a homing target if we don't have one
                if (HomingTarget == null)
                {
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
                float targetAngle = Projectile.AngleTo(HomingTarget.Center);
				float length = Projectile.velocity.Length();
				Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(30)).ToRotationVector2() * (length + 5);
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
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
				return target.CanBeChasedBy();
			}
			
    }
}