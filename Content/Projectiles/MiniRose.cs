using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using InnoVault.PRT;
using DestroyerTest.Content.Particles.TitaniumShard;
using DestroyerTest.Content.Particles;
using DestroyerTest.Common;

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
            int dustAmount = 16; // down from 52
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

                    for (int i = 0; i < dustAmount; i++)
                    {
                        float angle = MathHelper.TwoPi * i / dustAmount;
                        // Offset the angle each tick so the dust rotates around the circle over time
                        float timeOffset = Main.GameUpdateCount * 0.1f; // Adjust speed as needed
                        float dynamicAngle = angle + timeOffset;
                        Vector2 dustPos = Projectile.Center + Radius * new Vector2((float)Math.Cos(dynamicAngle), (float)Math.Sin(dynamicAngle));
                        PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], dustPos, Vector2.Zero, ColorLib.CursedFlames, 1.0f);
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