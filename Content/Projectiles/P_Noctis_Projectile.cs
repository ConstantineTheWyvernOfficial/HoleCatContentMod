using DestroyerTest.Content.RogueItems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
	// This projectile showcases advanced AI code. Of particular note is a showcase on how projectiles can stick to NPCs in a manner similar to the behavior of vanilla weapons such as Bone Javelin, Daybreak, Blood Butcherer, Stardust Cell Minion, and Tentacle Spike. This code is modeled closely after Bone Javelin.
	public class P_Noctis_Projectile : ModProjectile
	{
		

		public override void SetStaticDefaults() {
			ProjectileID.Sets.DontAttachHideToAlpha[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 132; // The width of projectile hitbox
			Projectile.height = 132; // The height of projectile hitbox
			Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Ranged; // Makes the projectile deal ranged damage. You can set in to DamageClass.Throwing, but that is not used by any vanilla items
			Projectile.penetrate = 4; // How many monsters the projectile can penetrate.
			Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			
			Projectile.light = 0.5f; // How much light emit around the projectile
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.tileCollide = true; // Can the projectile collide with tiles?
			
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}

		private const int GravityDelay = 45;

		public override void AI() {
			UpdateAlpha();
			NormalAI();
			
		}

		private void NormalAI() {
			// Offset the rotation by 90 degrees because the sprite is oriented vertically.
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);

            // Increase the frequency of particle generation
            for (int i = 0; i < 8; i++) { // Generate 3 particles per frame
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.WaterCandle, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 1, Scale: 3.3f);
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
                dust.noGravity = true;
            }
			// Spawn some random dusts as the javelin travels
			if (Main.rand.NextBool(3)) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.WaterCandle, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 1, Scale: 3.3f);
				dust.velocity += Projectile.velocity * 0.3f;
				dust.velocity *= 0.2f;
                dust.noGravity = true;
			}
			if (Main.rand.NextBool(4)) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.WaterCandle,
					0, 0, 254, Scale: 0.3f);
				dust.velocity += Projectile.velocity * 0.5f;
				dust.velocity *= 0.5f;
                dust.noGravity = true;
			}
		}

	

		public override void OnKill(int timeLeft) {
			Vector2 usePos = Projectile.position; // Position to use for dusts

			// Offset the rotation by 90 degrees because the sprite is oriented vertically.
			Vector2 rotationVector = (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2(); // rotation vector to use for dust velocity
			usePos += rotationVector * 16f;

			// Spawn some dusts upon javelin death
			for (int i = 0; i < 20; i++) {
				// Create a new dust
				Dust dust = Dust.NewDustDirect(usePos, Projectile.width, Projectile.height, DustID.Bone);
				dust.position = (dust.position + Projectile.Center) / 2f;
				dust.velocity += rotationVector * 2f;
				dust.velocity *= 0.5f;
				dust.noGravity = true;
				usePos -= rotationVector * 0f;
			}

			// Make sure to only spawn items if you are the projectile owner.
			// This is an important check as Kill() is called on clients, and you only want the item to drop once
			
		}

		private const int MaxStickingJavelin = 6; // This is the max amount of javelins able to be attached to a single NPC
		private readonly Point[] stickingJavelins = new Point[MaxStickingJavelin]; // The point array holding for sticking javelins

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Projectile.velocity = (target.Center - Projectile.Center) *
				0.75f; // Change velocity based on delta center of targets (difference between entity centers)
			Projectile.netUpdate = true; // netUpdate this javelin
			Projectile.damage = 0; // Makes sure the sticking javelins do not deal damage anymore
        // KillOldestJavelin will kill the oldest projectile stuck to the specified npc.
                    // It only works if ai[0] is 1 when sticking and ai[1] is the target npc index, which is what IsStickingToTarget and TargetWhoAmI correspond to.
                    Projectile.KillOldestJavelin(Projectile.whoAmI, Type, target.whoAmI, stickingJavelins);	
			Player player1 = Main.player[Projectile.owner];
			player1.AddBuff(BuffID.Slow, 120);
			player1.AddBuff(BuffID.MoonLeech, 120);
		}

        // ExampleJavelinBuff handles the damage over time (DoT)
			

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			// For going through platforms and such, javelins use a tad smaller size
			width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
			return true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// By shrinking target hitboxes by a small amount, this projectile only hits if it more directly hits the target.
			// This helps the javelin stick in a visually appealing place within the target sprite.
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8) {
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			// Return if the hitboxes intersects, which means the javelin collides or not
			return projHitbox.Intersects(targetHitbox);
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			
			// Since we aren't attached, add to this list
			behindNPCsAndTiles.Add(index);
		}

		// Change this number if you want to alter how the alpha changes
		private const int AlphaFadeInSpeed = 25;

		private void UpdateAlpha() {
			// Slowly remove alpha as it is present
			if (Projectile.alpha > 0) {
				Projectile.alpha -= AlphaFadeInSpeed;
			}

			// If alpha gets lower than 0, set it to 0
			if (Projectile.alpha < 0) {
				Projectile.alpha = 0;
			}
		}
	}
}