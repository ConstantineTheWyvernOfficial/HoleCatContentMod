using DestroyerTest.Common;
using DestroyerTest.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
	// This example is similar to the Wooden Arrow projectile
	public class CuproNickelJavelinThrown : ModProjectile
	{
        public override string Texture => "DestroyerTest/Content/RogueItems/CopperTier/CuproNickelJavelin";
		public override void SetStaticDefaults()
		{
			// If this arrow would have strong effects (like Holy Arrow pierce), we can make it fire fewer projectiles from Daedalus Stormbow for game balance considerations like this:
			//ProjectileID.Sets.FiresFewerFromDaedalusStormbow[Type] = true;
			ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
		}

		public override void SetDefaults()
		{
			Projectile.width = 52; // The width of projectile hitbox
			Projectile.height = 52; // The height of projectile hitbox
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = 6;
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}

        public int trailLength = 60;
		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = ColorLib.JavelinEnergy;

			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;

			// --- Draw the main projectile ---
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Main.EntitySpriteDraw(
				projectileTexture,
				Projectile.Center - Main.screenPosition,
				null,
				Color.White,
				Projectile.rotation,
				projectileTexture.Size() / 2,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			// --- Draw glow + trail ---
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/JavelinHeadAura").Value;
			//Vector2 offset = new Vector2(10.0f, 10.0f).RotatedBy(Projectile.rotation);
            //Vector2 offsetpos = Projectile.Center + offset - Main.screenPosition;
       
            Vector2 offset = new Vector2(3.0f, 3.0f).RotatedBy(Projectile.rotation);
            Vector2 offsetpos = Projectile.Center + offset - Main.screenPosition;


            Main.EntitySpriteDraw(
                glowTexture,
                offsetpos,
                null,
                lightColor,
                Projectile.rotation,
                glowTexture.Size() / 2,
                0.2f * Projectile.scale,
                SpriteEffects.None,
                0
            );

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			Texture2D longtrailTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/SimpleParticle").Value;
			Vector2 trailOrigin = new(longtrailTexture.Width / 2f, longtrailTexture.Height / 2f);

			for (int i = 0; i < trailLength && i < Projectile.oldPos.Length; i++)
			{
				float fade = (float)(trailLength - i) / trailLength;
				Color trailColor = lightColor * fade;
				trailColor.A = (byte)(fade * 100);

				Vector2 drawPosition = Projectile.oldPos[i] + (Projectile.Size / 4f) - Main.screenPosition;
				float scaleFactor = 0.1f;
				Main.EntitySpriteDraw(longtrailTexture, drawPosition, null, trailColor, Projectile.velocity.ToRotation() + MathHelper.PiOver2, trailOrigin, (Projectile.scale * fade) * scaleFactor, SpriteEffects.None, 0);
			}

			// Restore normal batch
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}

		public override void AI() {
			// The code below was adapted from the ProjAIStyleID.Arrow behavior. Rather than copy an existing aiStyle using Projectile.aiStyle and AIType,
			// like some examples do, this example has custom AI code that is better suited for modifying directly.
			// See https://github.com/tModLoader/tModLoader/wiki/Basic-Projectile#what-is-ai for more information on custom projectile AI.

            // The projectile is rotated to face the direction of travel
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

		}

		public override void OnKill(int timeLeft) {
			
			
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			SoundEngine.PlaySound(SoundID.Item65, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
            Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<HitAnimParticle_Javelin>(), 0, 0, Projectile.owner);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position); // Plays the basic sound most projectiles make when hitting blocks.
            Projectile.NewProjectile(Projectile.GetSource_TileInteraction((int)(Projectile.position.X / 16), (int)(Projectile.position.Y / 16)), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TileDeathAnimParticle>(), 0, 0, Projectile.owner);
			return true;
		}
	}
}