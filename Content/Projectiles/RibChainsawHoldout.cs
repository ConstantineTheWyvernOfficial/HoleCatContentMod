using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using DestroyerTest.Common;
using DestroyerTest.Content.MeleeWeapons;
using Microsoft.Xna.Framework.Graphics;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Buffs;

namespace DestroyerTest.Content.Projectiles
{
    public class RibChainsawHoldout : ModProjectile
    {
        SoundStyle EnemySlice = new SoundStyle($"DestroyerTest/Assets/Audio/TenebrousKatana/GoreSlice", 2) with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
        
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2; // This projectile has 4 frames.
        }
        public override void SetDefaults()
        {
            Projectile.width = 118;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 40; // persistent
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;

        }

        private void AnimateProjectile() {
            // Loop through the frames, assuming each frame lasts 5 ticks
            if (++Projectile.frameCounter >= 4) {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type]) {
                    Projectile.frame = 0;
                }
            }
        }
        
        public int SoundInterval = 20;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Check if the player is holding the item and channeled
            if (player.HeldItem.type == ModContent.ItemType<RibChainsaw>() && player.itemTime > 0)
            {

                SoundInterval--;
                if (SoundInterval <= 0)
                {
                    SoundEngine.PlaySound(new SoundStyle("DestroyerTest/Assets/Audio/Chainsaw"), Projectile.Center);
                    SoundInterval = 20;
                }

                AnimateProjectile();

                // Lock the projectile's position relative to the player
                float holdDistance = 50f;
                Vector2 mountedCenter = player.MountedCenter;
                Vector2 toCursor = Main.MouseWorld - mountedCenter;
                toCursor.Normalize();
                Vector2 desiredPos = mountedCenter + toCursor * holdDistance;

                Projectile.Center = desiredPos;
                // Add a vibrating effect by jittering the position slightly
                Vector2 vibration = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                Projectile.position += vibration;


                // Rotate to face the cursor
                Projectile.rotation = toCursor.ToRotation();

                

                // Constantly face the direction it's pointing
                Projectile.direction = toCursor.X > 0 ? 1 : -1;

                // Shoot dust particles in a line from the tip
                Vector2 dustDirection = toCursor;
                Vector2 dustSpawn = Projectile.Center + dustDirection * Projectile.width * 0.5f;

                Vector2 randomSpawn = Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height));
                if (Main.rand.NextBool(3)) // Randomly spawn dust every 3 frames
                {
                    Dust.NewDustDirect(randomSpawn, 0, 0, DustID.Blood, dustDirection.X * 4f, dustDirection.Y * 4f, 100, default, 1.2f);
                }


            }
            else
            {
                // Kill the projectile if the item is not being held
                Projectile.Kill();
            }
        }

		
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            target.AddBuff(BuffID.Bleeding, 120);
		}

    }
}