using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DestroyerTest.Common;
using DestroyerTest.Content.MeleeWeapons;
using Terraria.Audio;
using DestroyerTest.Content.Magic;

namespace DestroyerTest.Content.Projectiles
{
    public class TenebrousTradewindsHoldout : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 20; // This projectile has 4 frames.
        }
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 200; // persistent
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        private void AnimateProjectile() {
            // Loop through the frames, assuming each frame lasts 5 ticks
            if (++Projectile.frameCounter >= 1) {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type]) {
                    Projectile.frame = 0;
                }
            }
        }

        public int ShootTimer = 0;

        public override void AI()
        {
            SoundStyle Shoot = new SoundStyle($"DestroyerTest/Assets/Audio/TTUse") with
            {
                //Volume = 0.5f,
                PitchVariance = 1.0f,
            };
            Player player = Main.player[Projectile.owner];

            // Check if the player is holding the item and channeled
            if (player.HeldItem.type == ModContent.ItemType<TenebrousTradewinds>() && player.itemTime > 0)
            {
                ShootTimer++;

                

                AnimateProjectile();

                // Lock the projectile's position relative to the player
                float holdDistance = 15f;
                Vector2 mountedCenter = player.MountedCenter;
                Vector2 toCursor = Main.MouseWorld - mountedCenter;
                toCursor.Normalize();
                Vector2 desiredPos = mountedCenter + toCursor * holdDistance;

                Projectile.Center = desiredPos;

                // Rotate to face the cursor
                Projectile.rotation = toCursor.ToRotation(); //+ MathHelper.PiOver2;

                // Constantly face the direction it's pointing
                Projectile.direction = toCursor.X > 0 ? 1 : -1;

                if (ShootTimer >= 80) // Adjust the value to control the shooting rate
                {
                    ShootTimer = 0;
                    SoundEngine.PlaySound(Shoot);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, toCursor *= 80.0f, ModContent.ProjectileType<TenebrousTradewindsWind>(), 280, 6f, player.whoAmI);
                }
            }
            else
            {
                // Kill the projectile if the item is not being held
                Projectile.Kill();
            }
        }

    }
}