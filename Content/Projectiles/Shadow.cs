using System;
using DestroyerTest.Content.Dusts;
using DestroyerTest.Content.MeleeWeapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
	public class Shadow : ModProjectile
	{
        private void AnimateProjectile() {
            // Loop through the frames, assuming each frame lasts 5 ticks
            if (++Projectile.frameCounter >= 5) {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type]) {
                    Projectile.frame = 0;
                }
            }
        }
		public override void SetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 44;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 14; // Set the number of frames in the sprite sheet
        }

        public override void AI() {
            AnimateProjectile();
			Projectile.Center = Main.MouseWorld;
            float radius = 50f; // Distance from center
            float speed = 0.05f; // Rotation speed (radians per tick)
            float angle = Main.GameUpdateCount * speed; // Angle increases over time

            Vector2 center = Projectile.Center; // The point to orbit around
            Vector2 offset = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * radius;
            Projectile.position = center + offset - new Vector2(Projectile.width / 2, Projectile.height / 2);


			if (Main.rand.NextBool(3)) {
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Lava, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}
            if (!Main.dedServ) {
                    Lighting.AddLight(Projectile.Center, Projectile.Opacity * 2.55f, Projectile.Opacity * 1.55f, Projectile.Opacity * 0.0f);
                }
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			else {
				Projectile.ai[0] += 0.1f;
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				Projectile.velocity *= 0.75f;
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			}
			return false;
		}

		public override void OnKill(int timeLeft) {
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<RiftDust>(), Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
			}
			SoundEngine.PlaySound(SoundID.Item25, Projectile.position);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.ChlorophyteLeafCrystalShot,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox), UniqueInfoPiece = 36 },
				Projectile.owner);
        }
	}
}