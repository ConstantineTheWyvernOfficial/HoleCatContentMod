using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using DestroyerTest.Common;
using DestroyerTest.Content.MeleeWeapons;
using Microsoft.Xna.Framework.Graphics;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Projectiles.Tenebrouskatana;

namespace DestroyerTest.Content.Projectiles
{
    
    public class TenebrisJabProjectile : ModProjectile
    {
        SoundStyle EnemySlice = new SoundStyle($"DestroyerTest/Assets/Audio/TenebrousKatana/GoreSlice", 2) with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
        public static int Jab_HitCount { get; set;} = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 12; // This projectile has 4 frames.
        }
        public override void SetDefaults()
        {
            Projectile.width = 162;
            Projectile.height = 410;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 40; // persistent
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.netImportant = true;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        private void AnimateProjectile() {
            // Loop through the frames, assuming each frame lasts 5 ticks
            if (++Projectile.frameCounter >= 2) {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type]) {
                    Projectile.frame = 0;
                }
            }
        }



        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Check if the player is holding the item and channeled
            if (player.HeldItem.type == ModContent.ItemType<Tenebrous_Katana>() && player.itemTime > 0)
            {

                AnimateProjectile();

                // Lock the projectile's position relative to the player
                float holdDistance = 100f;
                Vector2 mountedCenter = player.MountedCenter;
                Vector2 toCursor = Main.MouseWorld - Main.LocalPlayer.MountedCenter;
                toCursor.Normalize();
                Vector2 desiredPos = mountedCenter + toCursor * holdDistance;

                Projectile.Center = desiredPos;

                // Rotate to face the cursor
                Projectile.rotation = toCursor.ToRotation() + MathHelper.PiOver2;
                
                if (player.direction == -1)
                {
                    Projectile.spriteDirection = -1;
                }
                else
                {
                    Projectile.spriteDirection = 1;
                }

                // Constantly face the direction it's pointing
                Projectile.spriteDirection = toCursor.X > 0 ? 1 : -1;


                // Shoot dust particles in a line from the tip
                Vector2 dustDirection = toCursor;
                Vector2 dustSpawn = Projectile.Center + dustDirection * Projectile.width * 0.5f;

                Vector2 randomSpawn = Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height));
                int dustIndex = Dust.NewDust(randomSpawn, 0, 0, DustID.TintableDustLighted, dustDirection.X * 4f, dustDirection.Y * 4f, 100, Color.Magenta, 1.2f);
                Main.dust[dustIndex].noGravity = true;

            }
            else
            {
                // Kill the projectile if the item is not being held
                Projectile.Kill();
            }
        }

		
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            Vector2 toCursor = Main.MouseWorld - Main.LocalPlayer.MountedCenter;
                toCursor.Normalize();
            Jab_HitCount += 1;
			Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<HitAnimParticle_TK>(), 0, 0, Projectile.owner);
            SoundEngine.PlaySound(EnemySlice, Projectile.position);
            Player player = Main.LocalPlayer;
            CombatText.NewText(player.getRect(), ColorLib.TenebrisGradient, $"Combo: {Jab_HitCount}", true, false);
            if (Jab_HitCount % 5 == 0)
            {
            Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, toCursor * 200, ModContent.ProjectileType<TenebrisClone>(), 0, 0, Projectile.owner);
            }
		}

    }
}