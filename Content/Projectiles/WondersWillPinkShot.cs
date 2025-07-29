using DestroyerTest.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
public class WondersWillPinkShot : ModProjectile
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
			Projectile.width = 10; // The width of projectile hitbox
			Projectile.height = 18; // The height of projectile hitbox

			Projectile.DamageType = ModContent.GetInstance<ScepterClass>();
			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.light = 1f; // How much light emit around the projectile
			Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
			}

			int trailLength = 10; // Adjust for desired effect
			public override bool PreDraw(ref Color lightColor)
			{
				lightColor = new Color(156, 0, 63);
				SpriteBatch spriteBatch = Main.spriteBatch;
				Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;

				// Draw the base projectile using the default drawing system (Deferred)
				Main.EntitySpriteDraw(
					projectileTexture,
					Projectile.Center - Main.screenPosition,
					null,
					lightColor, 
					Projectile.rotation,
					projectileTexture.Size() / 2,
					Projectile.scale,
					SpriteEffects.None,
					0
				);

				// Glow effect (Immediate drawing with Additive blending)
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/SimpleParticle").Value;
				Main.EntitySpriteDraw(
					glowTexture,
					Projectile.Center - Main.screenPosition,
					null,
					lightColor,
					Projectile.rotation,
					glowTexture.Size() / 2,
					Projectile.scale,
					SpriteEffects.None,
					0
				);

				// Now, render the **low-opacity red TRAIL** (Immediate drawing)
				Texture2D longtrailTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/Trail2").Value;
				Vector2 trailOrigin = new Vector2(longtrailTexture.Width / 2, longtrailTexture.Height / 2);

				for (int i = 0; i < trailLength && i < Projectile.oldPos.Length; i++)
				{
					float fade = (float)(trailLength - i) / trailLength;
					Color trailColor = lightColor * fade * 0.3f;
					trailColor.A = (byte)(fade * 100); // Transparency blending

					Vector2 drawPosition = Projectile.oldPos[i] + (Projectile.Size / 2f) - Main.screenPosition;
					float scaleFactor = 0.1f; // Adjust size of the trail
					Main.EntitySpriteDraw(longtrailTexture, drawPosition, null, trailColor, Projectile.rotation, trailOrigin, (Projectile.scale * fade) * scaleFactor, SpriteEffects.None, 0);
				}

				// Restore the deferred mode (for the next drawing of things)
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				return false; // Let the default system handle the base projectile drawing
			}


			// Custom AI
			public override void AI() {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
				float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target

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
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
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