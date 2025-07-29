using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DestroyerTest.Common;
using DestroyerTest.Content.MeleeWeapons;

namespace DestroyerTest.Content.Projectiles
{
    public class HeliciteShankSlice : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 28; // This projectile has 4 frames.
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 20; // persistent
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

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Check if the player is holding the item and channeled
            if (player.HeldItem.type == ModContent.ItemType<HeliciteShank>() && player.itemTime > 0)
            {

                AnimateProjectile();

                // Lock the projectile's position relative to the player
                float holdDistance = 30f;
                Vector2 mountedCenter = player.MountedCenter;
                Vector2 toCursor = Main.MouseWorld - mountedCenter;
                toCursor.Normalize();
                Vector2 desiredPos = mountedCenter + toCursor * holdDistance;

                Projectile.Center = desiredPos;

                // Rotate to face the cursor
                Projectile.rotation = toCursor.ToRotation() + MathHelper.PiOver2;

                // Constantly face the direction it's pointing
                Projectile.direction = toCursor.X > 0 ? 1 : -1;

                // Shoot dust particles in a line from the tip
                Vector2 dustDirection = toCursor;
                Vector2 dustSpawn = Projectile.Center + dustDirection * Projectile.width * 0.5f;

                Vector2 randomSpawn = Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height));
                int dustIndex = Dust.NewDust(randomSpawn, 0, 0, DustID.TintableDustLighted, dustDirection.X * 4f, dustDirection.Y * 4f, 100, ColorLib.Rift, 1.2f);
                Main.dust[dustIndex].noGravity = true;

            }
            else
            {
                // Kill the projectile if the item is not being held
                Projectile.Kill();
            }
        }

    }
}