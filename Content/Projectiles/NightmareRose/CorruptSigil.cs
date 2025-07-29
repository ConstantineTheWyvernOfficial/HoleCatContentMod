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
using DestroyerTest.Content.Magic;
using InnoVault.PRT;
using DestroyerTest.Content.Particles;
using System.Text;

namespace DestroyerTest.Content.Projectiles.NightmareRose
{
    public class CorruptSigil : ModProjectile
    {

        
			private Player HomingTarget {
				get => Projectile.ai[0] == 0 ? null : Main.player[(int)Projectile.ai[0] - 1];
				set {
					Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
				}
			}

        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.hostile = true;
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
            Projectile.alpha = 255;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            sb.End(); // End vanilla drawing
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, 
                    DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            DrawSigil(sb);

            sb.End(); // End additive
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                    DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public void DrawSigil(SpriteBatch sb)
        {
            Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Extras/CorruptSigil").Value;

            Main.EntitySpriteDraw(
                glowTexture,
                Projectile.Center - Main.screenPosition,
                null,
                ColorLib.CursedFlames,
                Projectile.rotation,
                glowTexture.Size() / 2,
                Projectile.scale * 0.4f,
                SpriteEffects.None,
                0
            );
        }


        public bool HasSpawned = false;

        public Vector2 CurrentCenter;

        public float ProjSpawnTimer = 0;

        public override void AI()
        {
            float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target

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
			
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(300)).ToRotationVector2() * length;


            ProjSpawnTimer++;
            Player player = Main.player[Projectile.owner];
            if (!player.channel || player.dead || player.CCed)
            {
                Projectile.Kill(); // Stop when player stops channeling, dies, or is crowd controlled
                return;
            }

            CurrentCenter = Projectile.Center;

            
          

            if (player.HeldItem.type == ModContent.ItemType<Contempt>() && player.channel)
            {
                Projectile.timeLeft = 120;
                
                if (HasSpawned == false)
                {
                    PRTLoader.NewParticle(PRTLoader.GetParticleID<RuneCircle1>(), Projectile.Center, Projectile.velocity, ColorLib.CursedFlames, 0.4f);
                    HasSpawned = true;
                }




                float rad = 1000;
                Vector2 Spawn = Projectile.Center + Main.rand.NextVector2CircularEdge(rad, rad);
                Vector2 toOrigin = CurrentCenter - Spawn;
                toOrigin = toOrigin.SafeNormalize(Vector2.UnitY); // fallback to downwards if zero

                if (ProjSpawnTimer >= 30)
                {
                    SoundEngine.PlaySound(SoundID.Item20);

                    for (int a = 0; a < 8; a++)
                    {
                        Spawn = Projectile.Center + Main.rand.NextVector2CircularEdge(rad, rad);
                        toOrigin = CurrentCenter - Spawn;
                        toOrigin = toOrigin.SafeNormalize(Vector2.UnitY);
                        Projectile Flames = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), Spawn, toOrigin * 20f, ModContent.ProjectileType<CursedFlameProj>(), 60, 2);
                        if (Flames.Center == Projectile.Center)
                        {
                            Flames.Kill();
                        }
                    }
                    PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp1>(), Projectile.Center, Projectile.velocity, ColorLib.CursedFlames, 0.4f);
                    ProjSpawnTimer = 0;
                }



            }


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
                    target.AddBuff(BuffID.CursedInferno, 120);
					Projectile.Kill();
			}






        

       

    }

}

