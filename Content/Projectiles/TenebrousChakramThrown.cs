using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using System.IO;
using System;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Dusts;

namespace DestroyerTest.Content.Projectiles
{
    public class TenebrousChakramThrown : ModProjectile
    {
        SoundStyle EnemySlice = new SoundStyle($"DestroyerTest/Assets/Audio/TenebrousKatana/GoreSlice", 2) with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
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
            Projectile.width = 54;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.penetrate = 4; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.Ranged;
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
                    if (proj.ModProjectile is TenebrousChakramThrown otherProj)
                    {
                        if (otherProj.existenceTimer < lowestExistenceTime)
                        {
                            lowestExistenceTime = otherProj.existenceTimer;
                            youngestProjectile = proj;
                        }
                    }
                }
            }

            if (Projectile.penetrate <= 1)
            {
                returning = true;
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
                    ModContent.ProjectileType<TenebrisStar>(),
                    Projectile.damage,
                    0,
                    player.whoAmI
                );

            }
            
            

            
           
            // Always spinning
            Projectile.rotation += 0.6f * Projectile.direction;
            

            if (!returning)
            {
                // OutPhase: Count time before returning
                flightTime++;
                if (flightTime >= 60 + ScepterClassStats.Range)
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
            target.AddBuff(ModContent.BuffType<FriendlyShimmeringFlames>(), 120);
            SoundEngine.PlaySound(EnemySlice, Projectile.position);
            HitCount += 1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            
            // Play impact sound and spawn tile hit effects
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item178, Projectile.Center);
            TileCollisions += 1;
            if (TileCollisions > 5)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                Projectile.Kill();
                HitCount = 0;
                existenceTimer = 0;
                TileCollisions = 0;
            }

            // Create a burst of dust on impact
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.position, ModContent.DustType<TenebrisMetalDust>(), oldVelocity.RotatedByRandom(MathHelper.PiOver4) * 0.5f, 150, default, 1.5f);
                dust.noGravity = false;
                dust.fadeIn = 1.5f;
            }

            // Activate return phase
            returning = true;

            return false; // Prevents the projectile from being destroyed on collision
        }

    }
}

