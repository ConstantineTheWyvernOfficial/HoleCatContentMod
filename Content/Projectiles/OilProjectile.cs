using System;
using System.Runtime.CompilerServices;
using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Particles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
    /// <summary>
    /// Expermiental Class for an All-Purpose Homing Projectile.
    /// <para/> Projectile ai slots 0 and 1 should not be set to anything when spawning, as they store NPC and Player values respectively.
    /// <para/> Projectile ai slot 2 controls whether the projectile is friendly or harmful.
    /// </summary>
	public class OilProjectile : ModProjectile
    {
        private NPC NPCTarget
        {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];
            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
            }
        }

        private Player PLRTarget
        {
            get => Projectile.ai[1] == 0 ? null : Main.player[(int)Projectile.ai[1] - 1];
            set
            {
                Projectile.ai[1] = value == null ? 0 : value.whoAmI + 1;
            }
        }

        public float DelayTimer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;

            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.light = 1f;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
            Projectile.extraUpdates = 3;
        }

        /// <summary>
        /// Controls whether the Projectile is Hostile or Friendly.
        /// <para/> 1 = Friendly, 2 = Hostile
        /// <para/> Attempting to return an invalid value will kill the projectile.
        /// </summary>
        public int Mode;

        public override void AI()
        {
            DelayTimer++;
            Mode = (int)Projectile.ai[2];

            if (Mode > 2 || Mode <= 0)
            {
                Projectile.Kill();
                //throw new Exception("Non-Fatal Error in Oil Projectile Targeting. Value must be 1 or 2.");
                Mod.Logger.Warn("OilProjectile: Invalid Mode in ai[2]. Expected 1 or 2.");
            }

            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.0f);



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

            if (Main.rand.NextBool(3))
            {
                PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], Projectile.Center, Vector2.Zero, ColorLib.RainbowGradient, 0.5f);
            }

            int[] types2 = new int[]
                {
                PRTLoader.GetParticleID<BlackFire1>(),
                PRTLoader.GetParticleID<BlackFire2>(),
                PRTLoader.GetParticleID<BlackFire3>(),
                PRTLoader.GetParticleID<BlackFire4>(),
                PRTLoader.GetParticleID<BlackFire5>(),
                PRTLoader.GetParticleID<BlackFire6>(),
                PRTLoader.GetParticleID<BlackFire7>()
                };

            Color[] BackColors = new Color[]
                {
                new Color(13, 2, 2),
                new Color(4, 4, 4),
                new Color(10, 13, 12),
                new Color(18, 10, 22),
                new Color(86, 65, 82),
                };

            if (Main.rand.NextBool(2))
            {
                PRTLoader.NewParticle(types2[Main.rand.Next(types2.Length)], Projectile.Center, Vector2.Zero, BackColors[Main.rand.Next(BackColors.Length)], 1.0f);
            }


            if (DelayTimer < 10)
                {
                    DelayTimer += 1;
                    return;
                }

            float maxDetectRadius = 1400f;

            if (Mode == 1)
            {
                Projectile.friendly = true;
                Projectile.hostile = false;

                if (NPCTarget == null)
                {
                    NPCTarget = FindClosestNPC(maxDetectRadius);
                }


                if (NPCTarget != null && !IsValidNPC(NPCTarget))
                {
                    NPCTarget = null;
                }


                if (NPCTarget == null)
                    return;

                float length = Projectile.velocity.Length();
                float targetAngle = Projectile.AngleTo(NPCTarget.Center);
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(15)).ToRotationVector2() * length;
            }
            if (Mode == 2)
            {
                Projectile.friendly = false;
                Projectile.hostile = true;

                if (PLRTarget == null)
                {
                    PLRTarget = FindClosestPlayer(maxDetectRadius);
                }


                if (PLRTarget != null && !IsValidPlayer(PLRTarget))
                {
                    PLRTarget = null;
                }

                if (PLRTarget == null)
                    return;

                float length = Projectile.velocity.Length();
                float targetAngle = Projectile.AngleTo(PLRTarget.Center);
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(15)).ToRotationVector2() * length;
            }
        }
        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            foreach (var target in Main.ActiveNPCs)
            {
                if (IsValidNPC(target))
                {

                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }

        public bool IsValidNPC(NPC target)
        {
            return target.CanBeChasedBy();
        }

        public Player FindClosestPlayer(float maxDetectDistance)
        {
            Player closestPlayer = null;

            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            foreach (var target in Main.player)
            {
                if (IsValidPlayer(target))
                {
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestPlayer = target;
                    }
                }
            }

            return closestPlayer;
        }

        public bool IsValidPlayer(Player target)
        {
            return (target.active == true && target.statLife > 1);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            PRTLoader.NewParticle(PRTLoader.GetParticleID<Boom1>(), target.Center, Vector2.Zero, ColorLib.RainbowGradient, 0.05f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            PRTLoader.NewParticle(PRTLoader.GetParticleID<Boom1>(), target.Center, Vector2.Zero, ColorLib.RainbowGradient, 0.05f);
        }
    }
}