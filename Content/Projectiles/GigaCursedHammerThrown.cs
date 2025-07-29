using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using Terraria.DataStructures;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Buffs;
using System.IO;
using InnoVault.PRT;

namespace DestroyerTest.Content.Projectiles
{
    public class GigaCursedHammerThrown : ModProjectile
    {
        private bool returning = false;
        private int flightTime = 0;

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
            Projectile.width = 164;
            Projectile.height = 164;
            Projectile.friendly = true;
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.Generic;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(returning);
            writer.Write(flightTime);
            writer.Write(soundCooldown);
            writer.Write(existenceTimer);
            writer.Write(TileCollisions);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            returning = reader.ReadBoolean();
            flightTime = reader.ReadInt32();
          
            soundCooldown = reader.ReadInt32();
            existenceTimer = reader.ReadInt32();
            TileCollisions = reader.ReadInt32();
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
                    if (proj.ModProjectile is GigaCursedHammerThrown otherProj)
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
                SoundEngine.PlaySound(SoundID.Item169);
                soundCooldown = 30; // Reset the cooldown to 30 ticks
            }
            

            
            Player player = Main.player[Projectile.owner];

            // Always spinning
            Projectile.rotation += 0.6f * Projectile.direction;

            
            int[] types = new int[]
			{
				PRTLoader.GetParticleID<ColoredFire1>(),
				PRTLoader.GetParticleID<ColoredFire2>(),
				PRTLoader.GetParticleID<ColoredFire3>(),
				PRTLoader.GetParticleID<ColoredFire4>(),
				PRTLoader.GetParticleID<ColoredFire5>(),
				PRTLoader.GetParticleID<ColoredFire6>(),
				PRTLoader.GetParticleID<ColoredFire7>()
			};

            PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], Projectile.Center + new Vector2(82, -82).RotatedBy(Projectile.rotation), Vector2.Zero, ColorLib.CursedFlames, 0.4f);
            

            ArmCatchAnimate(player);

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
                if (returnDirection.LengthSquared() < 45f) // 4 pixels radius
                {
                    
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
            SoundStyle Hit = new SoundStyle("DestroyerTest/Assets/Audio/EndemyImpact") with
            {
            PitchVariance = 0.5f
            };
            Player player = Main.player[Main.myPlayer];  // Accessing the current player
            target.StrikeNPC(hit); // This bypasses i-frames
            target.AddBuff(BuffID.CursedInferno, 120);
            SoundEngine.PlaySound(Hit, Projectile.position);
            returning = true; // Immediately start returning when hitting something
        }

  

        public override bool OnTileCollide(Vector2 oldVelocity) {
            SoundStyle Break = new SoundStyle("DestroyerTest/Assets/Audio/TO_Break") with
            {
            PitchVariance = 0.5f
            };
            // Play impact sound and spawn tile hit effects
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Tink, Projectile.position);
            TileCollisions += 1;
            if (TileCollisions > 5)
            {
                SoundEngine.PlaySound(Break);
                Projectile.Kill();
              
                existenceTimer = 0;
                TileCollisions = 0;
            }

            // Create a burst of dust on impact
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.position, DustID.CursedTorch, oldVelocity.RotatedByRandom(MathHelper.PiOver4) * 0.5f, 150, default, 1.5f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            // Activate return phase
            returning = true;

            return false; // Prevents the projectile from being destroyed on collision
        }

    }
}

