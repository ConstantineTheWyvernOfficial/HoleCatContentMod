using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using DestroyerTest.Content.Dusts;
using System;
using Terraria.DataStructures;
using System.IO;
using InnoVault.PRT;
using DestroyerTest.Content.Particles.TitaniumShard;
using DestroyerTest.Content.Projectiles.ParentClasses;

namespace DestroyerTest.Content.Projectiles
{
    public class TitanScepterThrown : ThrownScepter
    {
        public override void SetDefaults()
        {
            ThemeColor = Color.White;
            WidthDim = 34;
            HeightDim = 34;
            DustType = DustID.Glass;
            base.SetDefaults();
        }

        public int AreaTimer = 600;
        public bool TriggeredArea = false;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            

            Projectile.NewProjectile(Projectile.GetSource_OnHit(null), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AreaParticle>(), 0, 0, Projectile.owner);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
           

            Projectile.NewProjectile(Projectile.GetSource_OnHit(null), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AreaParticle>(), 0, 0, Projectile.owner);

            base.OnTileCollide(oldVelocity);
            return false; // Prevents the projectile from being destroyed on collision
        }

    }
    public class AreaParticle : ModProjectile
        {
            public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity";

            private int auraTimer = 600;
          

            public override void SetDefaults()
            {
                Projectile.width = 2;
                Projectile.height = 2;
                Projectile.friendly = false;
                Projectile.penetrate = -1;
                Projectile.timeLeft = 1200;
                Projectile.tileCollide = false;
                Projectile.scale = 0.1f;
                Projectile.hide = true; // Optional: hide if just visual
            }

            private float radius = 250f;
            public override void AI()
            {
                auraTimer--;

                // Create the dust ring
                int dustAmount = 4;
                
                    int[] types = new int[]
                    {
                        PRTLoader.GetParticleID<TitaniumShard1>(),
                        PRTLoader.GetParticleID<TitaniumShard2>(),
                        PRTLoader.GetParticleID<TitaniumShard3>(),
                        PRTLoader.GetParticleID<TitaniumShard4>(),
                        PRTLoader.GetParticleID<TitaniumShard5>(),
                        PRTLoader.GetParticleID<TitaniumShard6>(),
                        PRTLoader.GetParticleID<TitaniumShard7>(),
                        PRTLoader.GetParticleID<TitaniumShard8>(),
                        PRTLoader.GetParticleID<TitaniumShard9>(),
                        PRTLoader.GetParticleID<TitaniumShard10>(),
                        PRTLoader.GetParticleID<TitaniumShard11>()
                    };

                    for (int i = 0; i < dustAmount; i++)
                    {
                        float angle = MathHelper.TwoPi * i / dustAmount;
                        // Offset the angle each tick so the dust rotates around the circle over time
                        float timeOffset = Main.GameUpdateCount * 0.1f; // Adjust speed as needed
                        float dynamicAngle = angle + timeOffset;
                        Vector2 dustPos = Projectile.Center + radius * new Vector2((float)Math.Cos(dynamicAngle), (float)Math.Sin(dynamicAngle));
                        PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], dustPos, Vector2.Zero, default, 0.5f);
                    }
                

                // Apply buffs to players inside the radius
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead && Vector2.Distance(player.Center, Projectile.Center) <= radius)
                    {
                        player.AddBuff(BuffID.TitaniumStorm, 300);
                    }
                }

                if (auraTimer <= 0)
                {
                    Projectile.Kill();
                }
            }
        }

}

