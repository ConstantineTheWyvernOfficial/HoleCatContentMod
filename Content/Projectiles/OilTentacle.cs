using System;
using DestroyerTest.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
    // Copied from Calamity Mod's Eldtritch Tentacle Code, as I am not sure how to acquire the vanilla shadowflame hex doll code.
    public class OilTentacle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.MaxUpdates = 3;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.velocity.X != Projectile.velocity.X)
            {
                if (Math.Abs(Projectile.velocity.X) < 1f)
                {
                    Projectile.velocity.X = -Projectile.velocity.X;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            if (Projectile.velocity.Y != Projectile.velocity.Y)
            {
                if (Math.Abs(Projectile.velocity.Y) < 1f)
                {
                    Projectile.velocity.Y = -Projectile.velocity.Y;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Vector2 projCenter = Projectile.Center;
            Projectile.scale = 1f - Projectile.localAI[0];
            Projectile.width = (int)(20f * Projectile.scale);
            Projectile.height = Projectile.width;
            Projectile.position.X = projCenter.X - (float)(Projectile.width / 2);
            Projectile.position.Y = projCenter.Y - (float)(Projectile.height / 2);
            if ((double)Projectile.localAI[0] < 0.1)
            {
                Projectile.localAI[0] += 0.01f;
            }
            else
            {
                Projectile.localAI[0] += 0.025f;
            }
            if (Projectile.localAI[0] >= 0.95f)
            {
                Projectile.Kill();
            }
            Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[0] * 1.5f;
            Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[1] * 1.5f;
            if (Projectile.velocity.Length() > 16f)
            {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 16f;
            }
            Projectile.ai[0] *= 1.05f;
            Projectile.ai[1] *= 1.05f;
            if (Projectile.scale < 1f)
            {
                int scaleLoopCheck = 0;
                while ((float)scaleLoopCheck < Projectile.scale * 10f)
                {
                    Color[] Colors = new Color[]
                    {
                    new Color(10, 10, 10),
                    new Color(45, 45, 45),
                    new Color(100, 100, 100),
                    new Color(128, 128, 128),
                    new Color(200, 200, 200),
                    };
                    int OilDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<BlackDust>(), Projectile.velocity.X, Projectile.velocity.Y, 100, Colors[Main.rand.Next(Colors.Length)], 1.1f);
                    Main.dust[OilDust].position = (Main.dust[OilDust].position + Projectile.Center) / 2f;
                    Main.dust[OilDust].noGravity = true;
                    Main.dust[OilDust].velocity *= 0.1f;
                    Main.dust[OilDust].velocity -= Projectile.velocity * (1.3f - Projectile.scale);
                    Main.dust[OilDust].fadeIn = (float)(100 + Projectile.owner);
                    Main.dust[OilDust].scale += Projectile.scale * 0.75f;
                    scaleLoopCheck++;
                }
            }
        }
    }
}