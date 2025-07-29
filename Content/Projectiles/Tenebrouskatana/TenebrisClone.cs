using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;
using System;

namespace DestroyerTest.Content.Projectiles.Tenebrouskatana
{
    public class TenebrisClone : ModProjectile
    {

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 240; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.Generic;
            Projectile.tileCollide = true;
            Projectile.alpha = 80; // Start fully transparent
        }
    

        public override void AI()
        {
            Projectile.velocity *= 0.83f;
            Player player = Main.player[Projectile.owner];


              // Generate flying dust effect
            if (Main.rand.NextBool(3)) // 33% chance per tick
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, Projectile.velocity * 0.2f, 100, ColorLib.TenebrisGradient, 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            if (DestroyerTestMod.TenebrisTeleportKeybind.JustPressed)
				{
                    Projectile.timeLeft = 0;
                }
        }
    }

}

