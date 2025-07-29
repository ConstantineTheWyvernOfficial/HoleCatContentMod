using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Common;
using Terraria.GameContent.Drawing;

namespace DestroyerTest.Content.Projectiles
{
	// This projectile demonstrates exploding tiles (like a bomb or dynamite), spawning child projectiles, and explosive visual effects.
	public class StellarPipeBomb : ModProjectile
	{
		private const int DefaultWidthHeight = 15;
		private const int ExplosionWidthHeight = 250;

		private bool IsChild {
			get => Projectile.localAI[0] == 1;
			set => Projectile.localAI[0] = value.ToInt();
		}

		public override void SetStaticDefaults() {
			ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true; // Damage dealt to players does not scale with difficulty in vanilla.

			// This set handles some things for us already:
			// Sets the timeLeft to 3 and the projectile direction when colliding with an NPC or player in PVP (so the explosive can detonate).
			// Explosives also bounce off the top of Shimmer, detonate with no blast damage when touching the bottom or sides of Shimmer, and damage other players in For the Worthy worlds.
			ProjectileID.Sets.Explosive[Type] = true;
		}

		public override void SetDefaults() {
			// While the sprite is actually bigger than 15x15, we use 15x15 since it lets the projectile clip into tiles as it bounces. It looks better.
			Projectile.width = DefaultWidthHeight;
			Projectile.height = DefaultWidthHeight;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.netUpdate = true;

			// 5 second fuse.
			Projectile.timeLeft = 100;

			// These help the projectile hitbox be centered on the projectile sprite.
			DrawOffsetX = -2;
			DrawOriginOffsetY = -5;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			// Inflict the OnFire debuff for 10 seconds onto any NPC/Monster that this hits.
			// 600 frames = 10 seconds
			target.AddBuff(BuffID.OnFire, 600);
			// Vanilla explosions do less damage to Eater of Worlds in expert mode, so we will too.
			if (Main.expertMode) {
				if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail) {
					modifiers.FinalDamage /= 5;
				}
			}
		}

		// The projectile is very bouncy, but the spawned children projectiles shouldn't bounce at all.
		public override bool OnTileCollide(Vector2 oldVelocity) {
			// Die immediately if IsChild is true (We set this to true for the 5 extra explosives we spawn in OnKill)
			if (IsChild) {
				// These two are so the bomb will damage the player correctly.
				Projectile.timeLeft = 0;
				Projectile.PrepareBombToBlow();
				return true;
			}
			// OnTileCollide can trigger quite frequently, so using soundDelay helps prevent the sound from overlapping too much.
			if (Projectile.soundDelay == 0) {
				// We adjust Volume since the sound is a bit too loud. PitchVariance gives the sound some random pitch variance.
				SoundStyle impactSound = new SoundStyle("DestroyerTest/Assets/Audio/MetalPipe") with {
					Volume = 0.7f,
					PitchVariance = 0.5f,
				};
				SoundEngine.PlaySound(impactSound);
			}
			Projectile.soundDelay = 10;

			return false;
		}

		public override void AI() {
			// The projectile is in the midst of exploding during the last 3 updates.
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3) {
				Projectile.PrepareBombToBlow(); // Get ready to explode.
			}
			else {
				// Smoke and fuse dust spawn. The position is calculated to spawn the dust directly on the fuse.
				if (Main.rand.NextBool()) {
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f);
					dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
					dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
					dust.noGravity = true;
					dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;

					dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1f);
					dust.scale = 1f + Main.rand.Next(5) * 0.1f;
					dust.noGravity = true;
					dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;
				}
			}
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > 10f) {
				Projectile.ai[0] = 10f;
				// Roll speed dampening. 
				if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f) {
					Projectile.velocity.X = Projectile.velocity.X * 0.96f;

					if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01) {
						Projectile.velocity.X = 0f;
						Projectile.netUpdate = true;
					}
				}
			}
			// Rotation increased by velocity.X 
			Projectile.rotation += Projectile.velocity.X * 0.1f;
		}

		public override void PrepareBombToBlow() {
			Projectile.tileCollide = false; // This is important or the explosion will be in the wrong place if the bomb explodes on slopes.
			Projectile.alpha = 255; // Set to transparent. This projectile technically lives as transparent for about 3 frames

			// Change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
			Projectile.Resize(ExplosionWidthHeight, ExplosionWidthHeight);

			Projectile.damage = 250; // Bomb: 100, Dynamite: 250
			Projectile.knockBack = 10f; // Bomb: 8f, Dynamite: 10f
		}

		public override void OnKill(int timeLeft) {
			    var launchVelocity = new Vector2(-8, 0); // Create a velocity moving the left.
                
                for (int i = 0; i < 8; i++)
                {
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);
                    ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.RainbowRodHit, new ParticleOrchestraSettings { PositionInWorld = Projectile.Center });
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, launchVelocity, ModContent.ProjectileType<GalantineLanceFriendly>(), 35, 1);
                }
            SoundStyle BurstSound = new SoundStyle("DestroyerTest/Assets/Audio/MetalPipe") with {
					Volume = 0.7f,
					PitchVariance = 0.5f,
				};
			SoundEngine.PlaySound(BurstSound, Projectile.position);
			// Smoke Dust spawn
			for (int i = 0; i < 50; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, ColorLib.StellarColor, 2f);
				dust.velocity *= 1.4f;
			}

			// Fire Dust spawn
			for (int i = 0; i < 80; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, ColorLib.StellarColor, 3f);
				dust.noGravity = true;
				dust.velocity *= 5f;
				dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, ColorLib.StellarColor, 2f);
				dust.velocity *= 3f;
			}

			// Large Smoke Gore spawn
			for (int g = 0; g < 2; g++) {
				var goreSpawnPosition = new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f);
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.scale = 1.5f;
				gore.velocity.X += 1.5f;
				gore.velocity.Y += 1.5f;
				gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.scale = 1.5f;
				gore.velocity.X -= 1.5f;
				gore.velocity.Y += 1.5f;
				gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.scale = 1.5f;
				gore.velocity.X += 1.5f;
				gore.velocity.Y -= 1.5f;
				gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.scale = 1.5f;
				gore.velocity.X -= 1.5f;
				gore.velocity.Y -= 1.5f;
			}
			// reset size to normal width and height.
			Projectile.Resize(DefaultWidthHeight, DefaultWidthHeight);

		}

			
		
	}
}