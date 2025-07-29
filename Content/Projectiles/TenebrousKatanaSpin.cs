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
    public class TenebrousKatanaSpin : ModProjectile
    {
        SoundStyle EnemySlice = new SoundStyle($"DestroyerTest/Assets/Audio/TenebrousKatana/GoreSlice", 2) with {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.5f, 
				}; 
        public static int Spin_HitCount { get; set;} = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2; // This projectile has 4 frames.
        }
        public override void SetDefaults()
        {
            Projectile.width = 392;
            Projectile.height = 392;
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
            Tenebrous_Katana TK = ModContent.GetInstance<Tenebrous_Katana>();

            Vector2 toCursor = Main.MouseWorld - Main.LocalPlayer.MountedCenter;
            toCursor.Normalize();

            // Check if the player is holding the item and channeled
            if (player.HeldItem.type == ModContent.ItemType<Tenebrous_Katana>() && player.itemTime > 0 /*&& TK.ComboPhase == 2*/)
            {
                Projectile.Center = player.MountedCenter;
                player.velocity = toCursor * 35f;
                

                AnimateProjectile();

                if (Spin_HitCount >= 3)
                {
                    // Reset the hit count and kill the projectile
                    //Spin_HitCount = 0;
                    Projectile.Kill();
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
            Player player = Main.LocalPlayer;
            player.velocity = Vector2.Zero;
            Vector2 toCursor = Main.MouseWorld - Main.LocalPlayer.MountedCenter;
            toCursor.Normalize();
            
            player.velocity = toCursor * -player.velocity.Length() * 0.5f; // Apply knockback in the opposite direction of the projectile's velocity
            Spin_HitCount += 1;
			Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<HitAnimParticle_TK>(), 0, 0, Projectile.owner);
            SoundEngine.PlaySound(EnemySlice, Projectile.position);
            CombatText.NewText(player.getRect(), ColorLib.TenebrisGradient, $"Combo: {Spin_HitCount}", true, false);
		}

    }
}