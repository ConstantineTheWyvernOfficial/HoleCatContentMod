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
    public class StarHammerThrown : ModProjectile
    {
        private bool returning = false;
        private int flightTime = 0;
        private int soundCooldown = 0; // Initialize a cooldown timer

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.penetrate = 2; // Infinite pierce
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
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            returning = reader.ReadBoolean();
            flightTime = reader.ReadInt32();
          
            soundCooldown = reader.ReadInt32();
        }

        public override void AI()
        {

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

            if (Projectile.Center.Distance(player.Center) <= 60f)
            {
                Projectile.tileCollide = false;
            }

            // Always spinning
                Projectile.rotation += 0.6f * Projectile.direction;

            if (!returning)
            {
                // OutPhase: Count time before returning
                flightTime++;
                if (flightTime >= 10)
                {
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
                if (returnDirection.LengthSquared() < 45f) // 4 pixels radius
                {

                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundStyle Hit = new SoundStyle("DestroyerTest/Assets/Audio/EndemyImpact") with
            {
            PitchVariance = 0.5f
            };

            

            Player player = Main.player[Main.myPlayer];  // Accessing the current player
            StarFall(player, player.Center, target.Center - player.Center);
            hit.Knockback = 4f;
            target.StrikeNPC(hit); // This bypasses i-frames
            SoundEngine.PlaySound(Hit, Projectile.position);
            returning = true; // Immediately start returning when hitting something
        }

        public void StarFall(Player player, Vector2 position, Vector2 velocity)
        {
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			float ceilingLimit = target.Y;
			if (ceilingLimit > player.Center.Y - 200f) {
				ceilingLimit = player.Center.Y - 200f;
			}
			// Loop these functions 3 times.
			for (int i = 0; i < 8; i++) {
				position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
				position.Y -= 100 * i;
				Vector2 heading = target - position;

				if (heading.Y < 0f) {
					heading.Y *= -1f;
				}

				if (heading.Y < 20f) {
					heading.Y = 20f;
				}

				heading.Normalize();
				heading *= velocity.Length();
				heading.Y += Main.rand.Next(-40, 41) * 0.02f;
				Projectile.NewProjectile(Entity.GetSource_FromThis(), position, heading, ProjectileID.Starfury, (int)(Projectile.damage * 1.75f), 2f, player.whoAmI, 0f, ceilingLimit);
			}
        }

  

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Tink, Projectile.position);

            // Create a burst of dust on impact
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.position, DustID.YellowTorch, oldVelocity.RotatedByRandom(MathHelper.PiOver4) * 0.5f, 150, default, 1.5f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            // Activate return phase
            returning = true;

            return false; // Prevents the projectile from being destroyed on collision
        }

    }
}

