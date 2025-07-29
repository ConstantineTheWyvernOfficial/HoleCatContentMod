using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using Terraria.GameContent.Drawing;
using DestroyerTest.Content.Dusts;
using DestroyerTest.Content.Buffs;
using System.IO;

namespace DestroyerTest.Content.Projectiles
{
    public class RiftPhasesaberThrown : ModProjectile
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
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(returning);
            writer.Write(flightTime);
            writer.Write(HitCount);
            writer.Write(soundCooldown);
            writer.Write(existenceTimer);
            writer.Write(TileCollisions);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            returning = reader.ReadBoolean();
            flightTime = reader.ReadInt32();
            HitCount = reader.ReadInt32();
            soundCooldown = reader.ReadInt32();
            existenceTimer = reader.ReadInt32();
            TileCollisions = reader.ReadInt32();
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

				// Finalize drawing
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				return false; // Prevents default projectile rendering
			}

        public override void AI()
        {
            existenceTimer++;

            // Check for duplicate projectiles
            Projectile youngestProjectile = null;
            int lowestExistenceTime = int.MaxValue;
            int count = 0;

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == Projectile.type && proj.owner == Projectile.owner)
                {
                    count++;
                    if (proj.ModProjectile is ShadowScepterThrown otherProj)
                    {
                        if (otherProj.existenceTimer < lowestExistenceTime)
                        {
                            lowestExistenceTime = otherProj.existenceTimer;
                            youngestProjectile = proj;
                        }
                    }
                }
            }

            // If more than one exists, kill the youngest (lowest existenceTimer)
            if (count > 1 && youngestProjectile != null && youngestProjectile == Projectile)
            {
                Projectile.Kill();
                return; // Exit early to prevent further execution
            }



            // Decrease the cooldown timer on each tick
            if (soundCooldown > 0)
            {
                soundCooldown--;
            }

            // Play the sound every 30 ticks
            if (soundCooldown <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item15);
                soundCooldown = 30; // Reset the cooldown to 30 ticks
            }
            
            Player player = Main.player[Projectile.owner];

            // Always spinning
            Projectile.rotation += 0.4f * Projectile.direction;

              // Generate flying dust effect
            if (Main.rand.NextBool(3)) // 33% chance per tick
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<RiftDust>(), Projectile.velocity * 0.2f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }


            if (!returning)
            {
                // OutPhase: Count time before returning
                flightTime++;
                if (flightTime >= 60)
                {
                    returning = true;
                }
            }

            if (returning)
            {
                ArmCatchAnimate(player);
                // InPhase: Smooth return using Lerp
                Vector2 returnDirection = player.Center - Projectile.Center;
                float speed = MathHelper.Lerp(Projectile.velocity.Length(), 15f, 0.08f); // Smooth acceleration
                Projectile.velocity = returnDirection.SafeNormalize(Vector2.Zero) * speed;

                // If close enough, remove the projectile
                if (returnDirection.LengthSquared() < 20f) // 4 pixels radius
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
            SoundEngine.PlaySound(SoundID.Item175, Projectile.position);
           
            returning = true; // Immediately start returning when hitting something
        }


        public override bool OnTileCollide(Vector2 oldVelocity) {
            
            // Play impact sound and spawn tile hit effects
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            
    

            // Create a burst of dust on impact
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.position, ModContent.DustType<RiftDust>(), oldVelocity.RotatedByRandom(MathHelper.PiOver4) * 0.5f, 150, default, 1.5f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            // Activate return phase
            returning = true;

            return false; // Prevents the projectile from being destroyed on collision
        }

    }

}

