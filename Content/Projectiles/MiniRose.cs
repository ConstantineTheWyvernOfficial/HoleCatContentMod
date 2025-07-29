using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace DestroyerTest.Content.Projectiles
{
    public class MiniRose : ModProjectile
    {


        public override void SetDefaults()
        {
            Projectile.width = 27;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Radius = 120;
            if (Main.expertMode)
            {
                Radius = 180;
            }
            if (Main.masterMode)
            {
                Radius = 240;
            }
        }

        public int Radius;

        public int DustInterval = 30;

        public override void AI()
        {
            DustInterval++;
            // Create a dust perimeter (circle) around the projectile
            int dustAmount = 24; // down from 52
            if (DustInterval >= 30)
            {
                for (int d = 0; d < dustAmount; d++)
                {
                    float angle = MathHelper.TwoPi * d / dustAmount;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Radius;
                    Vector2 dustPosition = Projectile.Center + offset;
                    int dustIndex = Dust.NewDust(dustPosition, 0, 0, DustID.CursedTorch, 0f, 0f, 100, default, 0.8f);
                    Main.dust[dustIndex].noGravity = false; // let gravity clean it up
                    Main.dust[dustIndex].velocity *= 0.3f;
                }
                DustInterval = 0;
            }

            // Check if any player is within the circular area
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && !player.dead)
                    {
                        float distance = Vector2.Distance(player.Center, Projectile.Center);
                        if (distance <= Radius)
                        {
                            // Good: applies every frame but doesn't multiply infinitely
                            player.manaRegenBonus += 2;
                            player.lifeRegen += 2; // this is already a strong regen

                            // Avoid *= inside AI. You can give a timed buff instead or use a ModPlayer flag.
                            player.GetDamage(DamageClass.Generic) += 0.1f;
                        }

                    }
                }
            // Gravity
            if (Projectile.velocity.Y < 10f)
                Projectile.velocity.Y += 0.4f;

            // Stay on ground if colliding with tiles below
            if (Projectile.velocity.Y > 0f)
            {
                int tileX = (int)((Projectile.position.X + Projectile.width / 2) / 16f);
                int tileY = (int)((Projectile.position.Y + Projectile.height) / 16f);
                if (WorldGen.SolidTile(Main.tile[tileX, tileY]))
                {
                    Projectile.velocity.Y = 0f;
                    Projectile.position.Y = tileY * 16 - Projectile.height;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Stop vertical movement on ground
            if (oldVelocity.Y > 0f)
            {
                Projectile.velocity.Y = 0f;
            }
            return false;
        }
    }
}