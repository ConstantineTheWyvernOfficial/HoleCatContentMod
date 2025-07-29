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
using System.Collections.Generic;
using System.IO;
using InnoVault.PRT;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Particles.DamageBonusParticles;
using DestroyerTest.Content.Projectiles.ParentClasses;

namespace DestroyerTest.Content.Projectiles
{
    public class CelestialDiscordThrown : ThrownScepter
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
            
            target.AddBuff(BuffID.Confused, 120);
            Projectile.NewProjectile(Projectile.GetSource_OnHit(null), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CDAreaParticle>(), 0, 0, Projectile.owner);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            
            Projectile.NewProjectile(Projectile.GetSource_OnHit(null), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CDAreaParticle>(), 0, 0, Projectile.owner);

            base.OnTileCollide(oldVelocity);
            return false; // Prevents the projectile from being destroyed on collision
        }

    }
    public class CDAreaParticle : ModProjectile
        {
            public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity";

            private int auraTimer = 600;
            private float radius = 0f;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.tileCollide = false;
            Projectile.scale = 0.1f;
            Projectile.hide = true; // Optional: hide if just visual
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
            }

            public override void AI()
            {
                auraTimer--;

                // Interpolate radius from 0 to 300 over the first 30 ticks (auraTimer 600 -> 570)
                if (auraTimer > 570)
                {
                    float t = 1f - (auraTimer - 570) / 30f; // t goes from 0 to 1 as auraTimer goes from 600 to 570
                    radius = MathHelper.SmoothStep(0f, 300f, MathHelper.Clamp(t, 0f, 1f));
                }
                else
                {
                    radius = 300f;
                }

                // Create the dust ring
                int dustAmount = 8; // Number of dust particles in the ring
            for (int i = 0; i < dustAmount; i++)
            {
                float angle = MathHelper.TwoPi * i / dustAmount;
                // Offset the angle each tick so the dust rotates around the circle over time
                float timeOffset = Main.GameUpdateCount * 0.03f; // Adjust speed as needed
                float dynamicAngle = angle + timeOffset;
                Vector2 dustPos = Projectile.Center + radius * new Vector2((float)Math.Cos(dynamicAngle), (float)Math.Sin(dynamicAngle));
                PRTLoader.NewParticle(PRTLoader.GetParticleID<StarParticle>(), dustPos, Vector2.Zero, ColorLib.CelestialGradient, 1);
            }
                    if (auraTimer <= 30)
                {
                    float t = (float)auraTimer / 30f; // t goes from 1 to 0 as auraTimer goes from 30 to 0
                    radius = MathHelper.SmoothStep(0f, 300f, t); // Smoothly shrink from 300 to 0
                }
                
                int[] types = new int[]
                {
                    PRTLoader.GetParticleID<MagicDamageBonus1>(),
                    PRTLoader.GetParticleID<MagicDamageBonus2>(),
                    PRTLoader.GetParticleID<MeleeDamageBonus>(),
                    PRTLoader.GetParticleID<RangerDamageBonus1>(),
                    PRTLoader.GetParticleID<RangerDamageBonus2>(),
                    PRTLoader.GetParticleID<ScepterDamageBonus>(),
                    PRTLoader.GetParticleID<SummonerDamageBonus>()
                };

                Vector2 damagebonusspawnpoint = Main.rand.NextVector2Circular(radius, radius) + Projectile.Center;
            if (Main.rand.NextBool(3))
            {
                PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], damagebonusspawnpoint, new Vector2(0, 0.001f), ColorLib.CelestialGradient, 1);
            }

                // Apply buffs to players inside the radius
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead && Vector2.Distance(player.Center, Projectile.Center) <= radius)
                    {
                        player.GetDamage(DamageClass.Generic) += 0.25f;
                    }
                }

                if (auraTimer <= 0)
                {
                    Projectile.Kill();
                }
            }
        }

}

