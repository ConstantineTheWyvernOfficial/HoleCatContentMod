using DestroyerTest.Common;
using DestroyerTest.Content.Particles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
	// ExampleCustomSwingSword is an example of a sword with a custom swing using a held projectile
	// This is great if you want to make melee weapons with complex swing behavior
	// Note that this projectile only covers 2 relatively simple swings, everything else is up to you
	// Aside from the custom animation, the custom collision code in Colliding is very important to this weapon
	public class ConstantineScytheProjectile : ModProjectile
	{
		// We define some constants that determine the swing range of the sword
		// Not that we use multipliers here since that simplifies the amount of tweaks for these interactions
		// You could change the values or even replace them entirely, but they are tweaked with looks in mind
		private const float SWINGRANGE = 1.67f * (float)Math.PI; // The angle a swing attack covers (300 deg)
		private const float FIRSTHALFSWING = 0.45f; // How much of the swing happens before it reaches the target angle (in relation to swingRange)
		private const float SPINRANGE = 3.5f * (float)Math.PI; // The angle a spin attack covers (630 degrees)
		private const float WINDUP = 0.15f; // How far back the player's hand goes when winding their attack (in relation to swingRange)
		private const float UNWIND = 0.4f; // When should the sword start disappearing
		private const float SPINTIME = 2.5f; // How much longer a spin is than a swing

		private enum AttackType // Which attack is being performed
		{
			// Swings are normal sword swings that can be slightly aimed
			// Swings goes through the full cycle of animations
			Swing,

		}

		private enum AttackStage // What stage of the attack is being executed, see functions found in AI for description
		{
			Prepare,
			Execute,
			Unwind
		}

		// These properties wrap the usual ai and localAI arrays for cleaner and easier to understand code.
		private AttackType CurrentAttack
		{
			get => (AttackType)Projectile.ai[0];
			set => Projectile.ai[0] = (float)value;
		}

		private AttackStage CurrentStage
		{
			get => (AttackStage)Projectile.localAI[0];
			set
			{
				Projectile.localAI[0] = (float)value;
				Timer = 0; // reset the timer when the projectile switches states
			}
		}

		// Variables to keep track of during runtime
		private ref float InitialAngle => ref Projectile.ai[1]; // Angle aimed in (with constraints)
		private ref float Timer => ref Projectile.ai[2]; // Timer to keep track of progression of each stage
		private ref float Progress => ref Projectile.localAI[1]; // Position of sword relative to initial angle
		private ref float Size => ref Projectile.localAI[2]; // Size of sword

		// We define timing functions for each stage, taking into account melee attack speed
		// Note that you can change this to suit the need of your projectile
		private float prepTime => 24f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
		private float execTime => 8f / Owner.GetTotalAttackSpeed(Projectile.DamageType);
		private float hideTime => 24f / Owner.GetTotalAttackSpeed(Projectile.DamageType);

		public override string Texture => "DestroyerTest/Content/Projectiles/ConstantineScytheProjectile"; // Use texture of item as projectile texture
		private Player Owner => Main.player[Projectile.owner];

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 94; // Hitbox width of projectile
			Projectile.height = 102; // Hitbox height of projectile
			Projectile.friendly = true; // Projectile hits enemies
			Projectile.timeLeft = 10000; // Time it takes for projectile to expire
			Projectile.penetrate = -1; // Projectile pierces infinitely
			Projectile.tileCollide = false; // Projectile does not collide with tiles
			Projectile.usesLocalNPCImmunity = true; // Uses local immunity frames
			Projectile.localNPCHitCooldown = -1; // We set this to -1 to make sure the projectile doesn't hit twice
			Projectile.ownerHitCheck = true; // Make sure the owner of the projectile has line of sight to the target (aka can't hit things through tile).
			Projectile.DamageType = DamageClass.Melee; // Projectile is a melee projectile
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.spriteDirection = Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;
			float targetAngle = (Main.MouseWorld - Owner.MountedCenter).ToRotation();


			InitialAngle = (float)(-Math.PI / 2 - Math.PI * 1 / 3 * Projectile.spriteDirection); // For the spin, starting angle is designated based on direction of hit

		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			// Projectile.spriteDirection for this projectile is derived from the mouse position of the owner in OnSpawn, as such it needs to be synced. spriteDirection is not one of the fields automatically synced over the network. All Projectile.ai slots are used already, so we will sync it manually. 
			writer.Write((sbyte)Projectile.spriteDirection);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.spriteDirection = reader.ReadSByte();
		}

		public override void AI()
		{
			UpdateDanglingBead(Projectile.Center);
			// Extend use animation until projectile is killed
			Owner.itemAnimation = 2;
			Owner.itemTime = 2;

			// Kill the projectile if the player dies or gets crowd controlled
			if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
			{
				Projectile.Kill();
				return;
			}

			// AI depends on stage and attack
			// Note that these stages are to facilitate the scaling effect at the beginning and end
			// If this is not desirable for you, feel free to simplify
			switch (CurrentStage)
			{
				case AttackStage.Prepare:
					PrepareStrike();
					break;
				case AttackStage.Execute:
					ExecuteStrike();
					break;
				default:
					UnwindStrike();
					break;
			}

			SetSwordPosition();
			Timer++;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			// Calculate origin of sword (hilt) based on orientation and offset sword rotation (as sword is angled in its sprite)
			Vector2 origin;
			float rotationOffset;
			SpriteEffects effects;

			if (Projectile.spriteDirection > 0)
			{
				origin = new Vector2(0, Projectile.height);
				rotationOffset = MathHelper.ToRadians(45f);
				effects = SpriteEffects.None;
			}
			else
			{
				origin = new Vector2(Projectile.width, Projectile.height);
				rotationOffset = MathHelper.ToRadians(135f);
				effects = SpriteEffects.FlipHorizontally;
			}

			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, default, lightColor * Projectile.Opacity, Projectile.rotation + rotationOffset, origin, Projectile.scale, effects, 0);

			// Since we are doing a custom draw, prevent it from normally drawing
			return false;
		}

		// Find the start and end of the sword and use a line collider to check for collision with enemies
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 start = Owner.MountedCenter;
			Vector2 end = start + Projectile.rotation.ToRotationVector2() * ((Projectile.Size.Length()) * Projectile.scale);
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 15f * Projectile.scale, ref collisionPoint);
		}

		// Do a similar collision check for tiles
		public override void CutTiles()
		{
			Vector2 start = Owner.MountedCenter;
			Vector2 end = start + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() * Projectile.scale);
			Utils.PlotTileLine(start, end, 15 * Projectile.scale, DelegateMethods.CutTiles);
		}

		// We make it so that the projectile can only do damage in its release and unwind phases
		public override bool? CanDamage()
		{
			if (CurrentStage == AttackStage.Prepare)
				return false;
			return base.CanDamage();
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			// Make knockback go away from player
			modifiers.HitDirectionOverride = target.position.X > Owner.MountedCenter.X ? 1 : -1;


		}

		// Function to easily set projectile and arm position
		public void SetSwordPosition()
		{
			Projectile.rotation = InitialAngle + Projectile.spriteDirection * Progress; // Set projectile rotation

			// Set composite arm allows you to set the rotation of the arm and stretch of the front and back arms independently
			Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.ToRadians(90f)); // set arm position (90 degree offset since arm starts lowered)
			Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2); // get position of hand

			armPosition.Y += Owner.gfxOffY;
			Projectile.Center = armPosition; // Set projectile to arm position
			Projectile.scale = Size * 1.2f * Owner.GetAdjustedItemScale(Owner.HeldItem); // Slightly scale up the projectile and also take into account melee size modifiers

			Owner.heldProj = Projectile.whoAmI; // set held projectile to this projectile
		}

		// Function facilitating the taking out of the sword
		private void PrepareStrike()
		{
			Progress = WINDUP * SWINGRANGE * (1f - Timer / prepTime); // Calculates rotation from initial angle
			Size = MathHelper.SmoothStep(0, 1, Timer / prepTime); // Make sword slowly increase in size as we prepare to strike until it reaches max

			if (Timer >= prepTime)
			{
				
				CurrentStage = AttackStage.Execute; // If attack is over prep time, we go to next stage
			}
		}



		public static Vector2 CubicBezier(Vector2 start, Vector2 control1, Vector2 control2, Vector2 end, float t)
		{
			float u = 1 - t;
			return (u * u * u * start) + (3 * u * u * t * control1) + (3 * u * t * t * control2) + (t * t * t * end);
		}

		public bool hascloned = false;
		public Vector2 swordTip;

		// Function facilitating the first half of the swing
		private void ExecuteStrike()
		{
			Player player = Main.player[Projectile.owner];
			swordTip = Projectile.Center + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() * Projectile.scale);

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

            
            PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], swordTip, Vector2.Zero, ColorLib.RainbowGradient, 0.5f);
            

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

           
            PRTLoader.NewParticle(types2[Main.rand.Next(types2.Length)], swordTip, Vector2.Zero, BackColors[Main.rand.Next(BackColors.Length)], 1.0f);
            


			Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) * Timer / execTime);

			if (Timer >= execTime)
			{
				CurrentStage = AttackStage.Unwind;
			}


			// Emit light during the swing
			Lighting.AddLight(player.Center, 3.0f, 1.9f, 1.98f); // Adjust the color and intensity as needed

			if (hascloned == false)
			{
				Projectile.NewProjectile(Entity.GetSource_FromThis(), player.Center, new Vector2(player.direction * 8f, 0f), ModContent.ProjectileType<ConstantineScytheClone>(), (int)(Projectile.damage * 0.75f), 3);
				hascloned = true;
			}
		}

		// Function facilitating the latter half of the swing where the sword disappears
		private void UnwindStrike()
		{
			Progress = MathHelper.SmoothStep(0, SWINGRANGE, (1f - UNWIND) + UNWIND * Timer / hideTime);
			Size = 1f - MathHelper.SmoothStep(0, 1, Timer / hideTime); // Make sword slowly decrease in size as we end the swing to make a smooth hiding animation

			Player player = Main.player[Projectile.owner];

		
			if (Timer >= hideTime) {
				Projectile.Kill();
			}



		}

        public override void PostDraw(Color lightColor)
        {
			DrawDanglingBead(Main.spriteBatch);
        }

		const int SegmentCount = 12;
		const float SegmentLength = 2f;
		const float GravityStrength = 0.10f;
		const float SwingResponsiveness = 0.8f;
		const float Damping = 0.9f;

		Vector2[] ropeSegments = new Vector2[SegmentCount];
		Vector2[] ropeVelocities = new Vector2[SegmentCount];
		bool initialized = false;

		void UpdateDanglingBead(Vector2 anchor)
		{
			if (!initialized)
			{
				for (int i = 0; i < SegmentCount; i++)
				{
					ropeSegments[i] = anchor + Vector2.UnitY * SegmentLength * i;
					ropeVelocities[i] = Vector2.Zero;
				}
				initialized = true;
			}

			// Apply physics to each segment
			for (int i = 0; i < SegmentCount; i++)
			{
				// Gravity
				ropeVelocities[i].Y += GravityStrength;

				// Swing from movement of anchor
				if (i == 0)
					ropeVelocities[i] += (anchor - ropeSegments[i]) * SwingResponsiveness;

				// Integrate velocity
				ropeSegments[i] += ropeVelocities[i];

				// Dampen
				ropeVelocities[i] *= Damping;
			}

			// Constraints to keep segments connected
			for (int j = 0; j < 3; j++) // run multiple times for stability
			{
				for (int i = 0; i < SegmentCount - 1; i++)
				{
					Vector2 diff = ropeSegments[i + 1] - ropeSegments[i];
					float dist = diff.Length();
					float error = SegmentLength - dist;
					Vector2 correction = diff.SafeNormalize(Vector2.Zero) * (error * 0.5f);

					ropeSegments[i] -= correction;
					ropeSegments[i + 1] += correction;
				}

				// Anchor top segment to parent
				ropeSegments[0] = anchor;
			}
		}
		
		void DrawDanglingBead(SpriteBatch spriteBatch)
		{
			Texture2D segmentTex = ModContent.Request<Texture2D>("DestroyerTest/Content/Projectiles/ConstantineScytheString").Value;
			Texture2D beadTex = ModContent.Request<Texture2D>("DestroyerTest/Content/Projectiles/ConstantineScytheBead").Value;

			for (int i = 1; i < SegmentCount; i++)
			{
				Vector2 from = ropeSegments[i - 1];
				Vector2 to = ropeSegments[i];
				Vector2 center = (from + to) / 2f;
				float rotation = (to - from).ToRotation();

				spriteBatch.Draw(segmentTex, center - Main.screenPosition, null, Color.White, rotation + MathHelper.PiOver2, new Vector2(segmentTex.Width / 2f, segmentTex.Height / 2f), 1f, SpriteEffects.None, 0f);
			}

			// Draw the bead at the last segment
			spriteBatch.Draw(beadTex, ropeSegments[^1] - Main.screenPosition, null, Color.White, 0f, new Vector2(beadTex.Width / 2f, beadTex.Height / 2f), 1f, SpriteEffects.None, 0f);
		}




	}
}