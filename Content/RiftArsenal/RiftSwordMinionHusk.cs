using System.Numerics;
using DestroyerTest.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RiftArsenal
{
	public class RiftSwordMinionHusk : ModProjectile
	{
		private bool hasReplaced = false;
		private int DeathTimer = 0;

		public override void SetStaticDefaults()
		{
			// Placeholder for static defaults if needed later
		}

		public override void SetDefaults()
		{
			Projectile.width = 52;
			Projectile.height = 52;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.timeLeft = 380;
		}

		public override void AI()
		{
			DeathTimer++;

			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] >= 5f)
			{
				Projectile.ai[0] = 5f;
				Projectile.velocity.Y += 0.1f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			if (DeathTimer >= 360)
			{
				Projectile.Kill();
			}
		}

		public override bool OnTileCollide(Microsoft.Xna.Framework.Vector2 oldVelocity)
		{
			Projectile.velocity = Microsoft.Xna.Framework.Vector2.Zero;
            Projectile.rotation = 0 + MathHelper.PiOver2;
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			// Play death sound
			SoundEngine.PlaySound(SoundID.NPCDeath37, Projectile.position);

			// Dust effect
			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.TintableDustLighted);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
				dust.scale *= 0.9f;
				dust.color = ColorLib.Rift;
			}

			// Check for reanimation condition
			if (!hasReplaced)
			{
				Player player = Main.LocalPlayer;
				var modPlayer = player.GetModPlayer<LivingShadowPlayer>();

				if (modPlayer.LivingShadowCurrent > 0)
				{
					hasReplaced = true;
					Projectile.NewProjectileDirect(
						Projectile.GetSource_Death(),
						Projectile.position,
						Projectile.oldVelocity,
						ModContent.ProjectileType<RiftSwordMinion>(),
						0,
						0
					);
				}
			}
		}
	}
}
