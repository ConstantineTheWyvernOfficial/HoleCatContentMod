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

        public override void AI()
        {

            // Create a dust perimeter (circle) around the projectile
            int dustAmount = 6;
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
                float timeOffset = Main.GameUpdateCount * 0.6f; // Adjust speed as needed
                float dynamicAngle = angle + timeOffset;
                Vector2 dustPos = Projectile.Center + Radius * new Vector2((float)Math.Cos(dynamicAngle), (float)Math.Sin(dynamicAngle));
                PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], dustPos, Vector2.Zero, ColorLib.CursedFlames, 1.0f);
            }

            float radiusSq = Radius * Radius;
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && Vector2.DistanceSquared(player.Center, Projectile.Center) <= radiusSq)
                {
                    MiniRoseLifeRegenPlayer modPlayer = player.GetModPlayer<MiniRoseLifeRegenPlayer>();
                    modPlayer.Active = true;
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

                Tile tile = Main.tile[tileX, tileY];
                if (tile != null && tile.HasTile)
                {
                    bool isSolid = WorldGen.SolidTile(tile);
                    bool isPlatform = TileID.Sets.Platforms[tile.TileType] && !tile.IsHalfBlock && tile.Slope == 0;

                    if (isSolid || isPlatform)
                    {
                        Projectile.velocity.Y = 0f;
                        Projectile.position.Y = tileY * 16 - Projectile.height;
                    }
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

    public class MiniRoseLifeRegenPlayer : ModPlayer
    {
        public bool Active = false;
        public override void UpdateLifeRegen()
        {
            if (Active == true)
            {
                Player.lifeRegen += 20;
                Player.manaRegen += 20;
            }
        }

    }
}