using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Particles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
	public class ConstantineDollProjectile : ModProjectile
	{

        public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity";

        private Player HomingTarget {
				get => Projectile.ai[0] == 0 ? null : Main.player[(int)Projectile.ai[0] - 1];
				set {
					Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
				}
			}

		public ref float DelayTimer => ref Projectile.ai[1];

        public override void SetDefaults()
        {
            Player player = Main.LocalPlayer;
            Projectile.width = 90; // The width of projectile hitbox
            Projectile.height = 90; // The height of projectile hitbox

            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = true; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 1f; // How much light emit around the projectile
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
		}



		public override void AI()
		{
            Player player = Main.LocalPlayer;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			int[] types = new int[]
                {
                PRTLoader.GetParticleID<BlackFire1>(),
                PRTLoader.GetParticleID<BlackFire2>(),
                PRTLoader.GetParticleID<BlackFire3>(),
                PRTLoader.GetParticleID<BlackFire4>(),
                PRTLoader.GetParticleID<BlackFire5>(),
                PRTLoader.GetParticleID<BlackFire6>(),
                PRTLoader.GetParticleID<BlackFire7>()
                };

			PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], Projectile.Center, Vector2.Zero, default, 1);

            float maxDetectRadius = 4000f; // The maximum radius at which a projectile can detect a target

            // First, we find a homing target if we don't have one
            if (HomingTarget == null)
            {
                HomingTarget = FindClosestPlayer(maxDetectRadius);
            }

            // If we have a homing target, make sure it is still valid. If the NPC dies or moves away, we'll want to find a new target
            if (HomingTarget != null && !IsValidTarget(HomingTarget))
            {
                HomingTarget = null;
            }

            // If we don't have a target, don't adjust trajectory
            if (HomingTarget == null)
                return;

            // If found, we rotate the projectile velocity in the direction of the target.
            // We only rotate by 3 degrees an update to give it a smooth trajectory. Increase the rotation speed here to make tighter turns
            float length = Projectile.velocity.Length();
            float targetAngle = Projectile.AngleTo(HomingTarget.Center);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(300)).ToRotationVector2() * length;
			
		}

        public Player FindClosestPlayer(float maxDetectDistance) {
                        Player closestPlayer = null;

                        // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
                        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

                        // Loop through all NPCs
                        foreach (var target in Main.player) {
                            // Check if NPC able to be targeted. 
                            if (IsValidTarget(target)) {
                                // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                                float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                                // Check if it is within the radius
                                if (sqrDistanceToTarget < sqrMaxDetectDistance) {
                                    sqrMaxDetectDistance = sqrDistanceToTarget;
                                    closestPlayer = target;
                                }
                            }
                        }

                        return closestPlayer;
                    }

                    public bool IsValidTarget(Player target) {
                        
                        return (target.active == true && target.statLife > 1);
                    }
                public override void OnHitPlayer(Player target, Player.HurtInfo info)
                {
                            info.Damage = 270000000;
                            info.Dodgeable = false;
                            target.KillMe(PlayerDeathReason.ByCustomReason(Terraria.Localization.NetworkText.FromLiteral($"{target.name} played stupid games and won stupid prizes.")), info.Damage, 0, false);
                            Projectile.Kill();
                    }
	}

}