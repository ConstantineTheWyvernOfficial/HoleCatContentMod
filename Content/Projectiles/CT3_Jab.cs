using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
    public class CT3_Jab_Projectile : ModProjectile
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

        // Override the Texture property to specify the texture path
        public override string Texture => "DestroyerTest/Content/Projectiles/CT3_Jab";

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(114); // This sets width and height to the same value (important when projectiles can rotate)
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

        public override void AI() {
			Player player = Main.player[Projectile.owner];

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
			const int HalfSpriteWidth = 57 / 2;
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