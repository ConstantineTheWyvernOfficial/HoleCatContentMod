using DestroyerTest.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles.ConstitutionBoss
{
public class TrailBlazer : ModProjectile
		{
			// Store the target NPC using Projectile.ai[0]
			public Player HomingTarget {
				get => Projectile.ai[0] == 0 ? null : Main.player[(int)Projectile.ai[0] - 1];
				set {
					Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
				}
			}

			public ref float DelayTimer => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            Main.projFrames[Type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = 68; // The width of projectile hitbox
			Projectile.height = 72; // The height of projectile hitbox

			Projectile.DamageType = DamageClass.Generic;
			Projectile.friendly = false; // Can the projectile deal damage to enemies?
			Projectile.hostile = true; // Can the projectile deal damage to the player?
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.light = 1f; // How much light emit around the projectile
			Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.tileCollide = false;
			Projectile.damage += 15;
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
			}

            private void AnimateProjectile() {
            // Loop through the frames, assuming each frame lasts 5 ticks
            if (++Projectile.frameCounter >= 5) {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type]) {
                    Projectile.frame = 0;
                }
            }
        }

			public int trailLength = 20;
			public override bool PreDraw(ref Color lightColor)
			{
                
				lightColor = ColorLib.StellarColor;

				SpriteBatch spriteBatch = Main.spriteBatch;
				Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;

				// --- Draw the main projectile ---
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				int frameHeight = projectileTexture.Height / Main.projFrames[Projectile.type];
                Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, projectileTexture.Width, frameHeight);

                Main.EntitySpriteDraw(
                    projectileTexture,
                    Projectile.Center - Main.screenPosition,
                    sourceRect, // Use the correct frame
                    lightColor,
                    Projectile.rotation,
                    new Vector2(projectileTexture.Width / 2f, frameHeight / 2f),
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );

				// --- Draw glow + trail ---
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

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


				

				// Restore normal batch
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				return false;
			}

        // Custom AI
        public override void AI()
        {
            AnimateProjectile();
            Projectile.rotation = Projectile.velocity.ToRotation();
            float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target

            // First, we find a homing target if we don't have one
            if (HomingTarget == null)
            {
                HomingTarget = FindPlayer(maxDetectRadius);
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
            float targetAngle = Projectile.AngleTo(HomingTarget.Center);
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(30)).ToRotationVector2() * length;
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, Projectile.velocity * 0.2f, 100, ColorLib.StellarColor, 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }
			}

			// Finding the closest NPC to attack within maxDetectDistance range
			// If not found then returns null
			public Player FindPlayer(float maxDetectDistance) {
				Player ClosestTarget = null;

				// Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
				float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

				// Loop through all NPCs
				foreach (var targetplayer in Main.player) {
					// Check if NPC able to be targeted. 
					if (IsValidTarget(targetplayer)) {
						// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
						float sqrDistanceToTarget = Vector2.DistanceSquared(targetplayer.Center, Projectile.Center);

						// Check if it is within the radius
						if (sqrDistanceToTarget < sqrMaxDetectDistance) {
							sqrMaxDetectDistance = sqrDistanceToTarget;
							ClosestTarget = targetplayer;
						}
					}
				}

				return ClosestTarget;
			}

			public bool IsValidTarget(Player target) {
				// This method checks that the NPC is:
				// 1. active (alive)
				// 2. chaseable (e.g. not a cultist archer)
				// 3. max life bigger than 5 (e.g. not a critter)
				// 4. can take damage (e.g. moonlord core after all it's parts are downed)
				// 5. hostile (!friendly)
				// 6. not immortal (e.g. not a target dummy)
				// 7. doesn't have solid tiles blocking a line of sight between the projectile and NPC
				return target.active == true && target.statLife > 5 && target.MountedCenter.DistanceSQ(Projectile.Center) < 25000;
			}

       
    }
}