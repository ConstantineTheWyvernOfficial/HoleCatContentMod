using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;

namespace DestroyerTest.Content.Projectiles
{
    public class RiftChakramThrown : ModProjectile
    {
        private bool returning = false;
        private int flightTime = 0;

        public int HitCount = 0;

        private int soundCooldown = 0; // Initialize a cooldown timer

        public int existenceTimer = 0;

        public int TileCollisions = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        int trailLength = 10; // Adjust for desired effect
		public override bool PreDraw(ref Color lightColor)
			{
				// Set lightColor to a reddish hue and adjust its transparency based on the projectile's time left
				lightColor = new Color(255, 155, 0);
				if (Projectile.timeLeft < 30)
				{
					lightColor *= ((float)Projectile.timeLeft / 30f); // Fade out glow as projectile nears expiration
				}

				// Prepare for sprite drawing
				SpriteBatch spriteBatch = Main.spriteBatch;
				Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;

				// End previous spriteBatch before starting new ones
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				// Draw the main projectile
				Main.EntitySpriteDraw(projectileTexture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, projectileTexture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

				// End the AlphaBlend draw and start the Additive blend for the glow effect
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				// Draw the large colored glow
				Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/Cyclone1").Value;
                if (returning)
                {
                    Main.EntitySpriteDraw(glowTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, glowTexture.Size() / 2, 0.1f * Projectile.scale, SpriteEffects.None, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(glowTexture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, glowTexture.Size() / 2, 0.1f * Projectile.scale, SpriteEffects.FlipHorizontally, 0);
                }

                spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				// Draw the large colored glow
				Texture2D glowTexture2 = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/BloomRing").Value;
                    
                Main.EntitySpriteDraw(glowTexture2, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, glowTexture2.Size() / 2, 0.4f * Projectile.scale, SpriteEffects.None, 0);

				// Now, render the **low-opacity red TRAIL** (no white trail)
				Texture2D longtrailTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/Trail2").Value; 
				Vector2 trailOrigin = new Vector2(longtrailTexture.Width / 2, longtrailTexture.Height / 2);

				for (int i = 0; i < trailLength && i < Projectile.oldPos.Length; i++)
					{
						float fade = (float)(trailLength - i) / trailLength;

						// Make sure transparency blending is correct
						Color trailColor = lightColor * fade * 0.3f;
						trailColor.A = (byte)(fade * 100); // Instead of setting it to 0

						Vector2 drawPosition = Projectile.oldPos[i] + (Projectile.Size / 4) - Main.screenPosition;
                        float rotationAngle = Projectile.velocity.ToRotation();
						float scaleFactor = 0.3f; // Adjust the factor to make it smaller
						Main.EntitySpriteDraw(longtrailTexture, drawPosition, null, trailColor, rotationAngle, trailOrigin, (Projectile.scale * fade) * scaleFactor, SpriteEffects.None, 0);
					}

				// Finalize drawing
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				return false; // Prevents default projectile rendering
			}

        public override void AI()
        {
            existenceTimer++;
            // Decrease the cooldown timer on each tick
            if (soundCooldown > 0)
            {
                soundCooldown--;
            }

            // Play the sound every 30 ticks
            if (soundCooldown <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item169);
                soundCooldown = 30; // Reset the cooldown to 30 ticks
            }
            
            

            
            Player player = Main.player[Projectile.owner];

            // Always spinning
            Projectile.rotation += 0.4f * Projectile.direction;

              // Generate flying dust effect
            if (Main.rand.NextBool(3)) // 33% chance per tick
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Lava, Projectile.velocity * 0.2f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            ArmCatchAnimate(player);

            if (!returning)
            {
                // OutPhase: Count time before returning
                flightTime++;
                if (flightTime >= 60)
                {
                    SoundStyle RadioIN = new SoundStyle ("DestroyerTest/Assets/Audio/RadioIN");
                    SoundEngine.PlaySound(RadioIN, Projectile.Center); // Play a sound when the flight time exceeds 60 ticks (1 second)
                    returning = true;
                }
            }

            if (returning)
            {
                // InPhase: Smooth return using Lerp
                Vector2 returnDirection = player.Center - Projectile.Center;
                float speed = MathHelper.Lerp(Projectile.velocity.Length(), 15f, 0.08f); // Smooth acceleration
                Projectile.velocity = returnDirection.SafeNormalize(Vector2.Zero) * speed;

                // If close enough, remove the projectile
                if (returnDirection.LengthSquared() < 16f) // 4 pixels radius
                {
                    HitCount = 0;
                    existenceTimer = 0;
                    Projectile.Kill();
                }
            }
        }

        public void ArmCatchAnimate(Player player)
        {
            // Calculate the direction vector from the player to the projectile
            Vector2 directionToProjectile = Projectile.Center - player.Center;

            // Normalize the direction vector to get a unit vector
            directionToProjectile.Normalize();

            // Calculate the angle between the player's direction and the direction to the projectile
            float angleDifference = MathHelper.WrapAngle(directionToProjectile.ToRotation() - player.direction * MathHelper.PiOver2);

            // Adjust arm rotation based on the player's facing direction
            if (player.direction == 1)
            {
                // Player is facing right, so we use the angle difference as is
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, angleDifference);
            }
            else if (player.direction == -1)
            {
                // Player is facing left, so flip the angle by pi (180 degrees) to reach the opposite direction
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, angleDifference + MathHelper.Pi);
            }
        }






        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DaylightOverloadFriendly>(), 120);
            HitCount += 1;
            returning = true; // Immediately start returning when hitting something
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            // Create a burst of dust on impact
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.position, DustID.Lava, oldVelocity.RotatedByRandom(MathHelper.PiOver4) * 0.5f, 150, default, 1.5f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            // Activate return phase
            returning = true;

            return false; // Prevents the projectile from being destroyed on collision
        }

    }

}

