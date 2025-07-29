using DestroyerTest.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
    public class Conclusion_Dash : ModProjectile
    {
        public const int FadeInDuration = 7;
        public const int FadeOutDuration = 4;
        public const int TotalDuration = 20;

        // The "width" of the blade
        public float CollisionWidth => 10f * Projectile.scale;

        public int Timer {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(134); // This sets width and height to the same value (important when projectiles can rotate)
			Projectile.aiStyle = -1; // Use our own AI to customize how it behaves
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = false; // Prevents hits through tiles
			Projectile.extraUpdates = 1; // Update 1+extraUpdates times per tick
			Projectile.timeLeft = 360; // This value does not matter since we manually kill it earlier
			Projectile.hide = false; // Ensure the projectile is visible
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}
		public bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
				float numberProjectiles = 3 + Main.rand.Next(3); // 3, 4, or 5 shots
				float rotation = MathHelper.ToRadians(45);

				position += Vector2.Normalize(velocity) * 45f;
				velocity *= 1.0f; // Slow the projectile down to 1/5th speed so we can see it. This is only here because this example shares ModItem.SetDefaults code with other examples. If you are making your own weapon just change Item.shootSpeed as normal.

				for (int i = 0; i < numberProjectiles; i++) {
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
					Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
				}

				return false; // return false to stop vanilla from calling Projectile.NewProjectile.
			}
		private float hideTime => 12f;

        public override void AI() {
			Player player = Main.player[Projectile.owner];
				// Call the Shoot method
				Vector2 position = player.Center; // Start at the player's center

				// Calculate direction based on the player's facing direction
				Vector2 direction = player.DirectionTo(Main.MouseWorld); // Direction towards the mouse position
				Vector2 velocity = direction * 20f; // Adjust the speed multiplier (20f is an example)

				// Define the projectile type and parameters
				int type = ModContent.ProjectileType<Conclusion_Shot>(); // Your custom projectile
				int damage = 200; // Example damage
				float knockback = 2f; // Example knockback

				// Create the source for the projectile
				EntitySource_ItemUse_WithAmmo source = new EntitySource_ItemUse_WithAmmo(player, null, Type);

				// Call the Shoot function
				Shoot(player, source, position, velocity, type, damage, knockback);
				if (Timer >= hideTime) {
					Projectile.Kill();
				}

			Timer += 1;
			if (Timer >= TotalDuration) {
				// Kill the projectile if it reaches its intended lifetime
				Projectile.Kill();
				return;
			} else {
				// Ensure the sprite draws "in" the player's hand
				player.heldProj = Projectile.whoAmI;
			}

			// Fade in and out
			Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

			// Keep locked onto the player, but extend further based on the given velocity
			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
			Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

			// Set spriteDirection based on moving left or right
			Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();

			// Point towards where it is moving
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;

			// Align the sprite with the hitbox
			SetVisualOffsets();
		}

		//CENTER THE SPRITE IN YOUR HAND
        private void SetVisualOffsets() {
			const int HalfSpriteWidth = 134 / 2;
			int HalfProjWidth = Projectile.width / 2;

			if (Projectile.spriteDirection == 1) {
				DrawOriginOffsetX = -(HalfProjWidth - HalfSpriteWidth);
				DrawOffsetX = (int)-DrawOriginOffsetX * 2;
				DrawOriginOffsetY = 0;
			} else {
				DrawOriginOffsetX = (HalfProjWidth - HalfSpriteWidth);
				DrawOffsetX = 0;
				DrawOriginOffsetY = 0;
			}
		}	

		//SOMETHING FOR CUSTOM ALIGNMENT
        public override bool ShouldUpdatePosition() {
            // Update Projectile.Center manually
            return false;
        }

		//FUCK UP TILES WHEN TOUCHED
        public override void CutTiles() {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
            Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
        }

		//COLLISION HANDLER
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity * 6f;
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
        }
		
    }
}