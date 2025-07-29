using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace DestroyerTest.Content.Projectiles
{
    public class IceRope : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10; // Width of the projectile
            Projectile.height = 10; // Height of the projectile
            Projectile.friendly = true; // Can hit enemies
            Projectile.penetrate = -1; // Infinite penetration
            Projectile.tileCollide = false; // Doesn't collide with tiles
            Projectile.ignoreWater = true; // Ignores water slowdown
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner]; // Get the owner of the projectile

            // Ensure the projectile follows the player's position
            Vector2 playerCenter = player.MountedCenter;
            Vector2 directionToPlayer = playerCenter - Projectile.Center;
            float distanceToPlayer = directionToPlayer.Length();

            if (distanceToPlayer > 400f) // Max distance before returning
            {
                Projectile.Kill();
            }

            if (!player.channel) // If the player releases the attack button
            {
                Projectile.Kill(); // End the projectile
            }

            directionToPlayer.Normalize(); // Make the direction vector a unit vector
            Projectile.velocity = directionToPlayer * 10f; // Move the projectile back to the player

            // Create the chain effect between the player and the projectile
            if (Main.myPlayer == Projectile.owner)
            {
                float chainSegmentLength = 20f;
                Vector2 chainPosition = Projectile.Center;

                for (float i = chainSegmentLength; i < distanceToPlayer; i += chainSegmentLength)
                {
                    chainPosition = Projectile.Center + directionToPlayer * i;
                    Dust.NewDustPerfect(chainPosition, DustID.Iron, Vector2.Zero, 100, Color.Gray, 1.2f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 playerCenter = player.MountedCenter;
            Vector2 directionToPlayer = playerCenter - Projectile.Center;
            float distanceToPlayer = directionToPlayer.Length();
            directionToPlayer.Normalize();

            // Load your custom chain texture
            Texture2D chainTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Projectiles/IceRope").Value;

            Vector2 chainPosition = Projectile.Center;
            for (float i = 0; i < distanceToPlayer; i += chainTexture.Height)
            {
                chainPosition = Projectile.Center + directionToPlayer * i;

                Main.EntitySpriteDraw(
                    chainTexture, // Use your custom texture
                    chainPosition - Main.screenPosition, // Adjust for screen position
                    null, // Use the full texture
                    lightColor, // Apply light color
                    0f, // No rotation (optional: calculate rotation based on direction)
                    new Vector2(chainTexture.Width / 2, chainTexture.Height / 2), // Origin (centered)
                    1f, // Scale
                    SpriteEffects.None, // No flipping
                    0f // Layer depth
                );
            }

            return true;
        }
    }
}
