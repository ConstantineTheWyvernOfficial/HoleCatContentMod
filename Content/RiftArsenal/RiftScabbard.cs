
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Common;
using DestroyerTest.Content.SummonItems;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.RiftBiome;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Tools;
using DestroyerTest.Content.RiftArsenalNoCharge;
using Terraria.Localization;

namespace DestroyerTest.Content.RiftArsenal
{
	// This file contains all the code necessary for a minion
	// - ModItem - the weapon which you use to summon the minion with
	// - ModBuff - the icon you can click on to despawn the minion
	// - ModProjectile - the minion itself

	// It is not recommended to put all these classes in the same file. For demonstrations sake they are all compacted together so you get a better overview.
	// To get a better understanding of how everything works together, and how to code minion AI, read the guide: https://github.com/tModLoader/tModLoader/wiki/Basic-Minion-Guide
	// This is NOT an in-depth guide to advanced minion AI
	public class RiftSwordBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
		}

		public override void Update(Player player, ref int buffIndex) {
			// If the minions exist reset the buff time, otherwise remove the buff from the player
			if (player.ownedProjectileCounts[ModContent.ProjectileType<RiftSwordMinion>()] > 0) {
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

	[AutoloadEquip(EquipType.Waist)]
	public class RiftScabbard : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f; // The default value is 1, but other values are supported. See the docs for more guidance. 
		}

		public override void SetDefaults() {
			Item.damage = 40;
			Item.knockBack = 0f;
			Item.mana = 40; // mana cost
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.RaiseLamp; // how the player's arm moves when using the item
			Item.value = 18000;
						Item.rare = ModContent.RarityType<RiftRarity1>(); // The rarity of the item
			Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/Rift_Katana_Hold") with {
				Volume = 1.0f, 
    			Pitch = 0.0f, 
    			PitchVariance = 0.5f, 
			}; // The sound when the weapon is being used.
			Item.accessory = true;

			// These below are needed for a minion weapon
			Item.noMelee = true; // this item doesn't do any melee damage
			Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
			Item.buffType = ModContent.BuffType<RiftSwordBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<RiftSwordMinion>(); // This item creates the minion projectile
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
			position = Main.MouseWorld;
		}

       
       // Define minionTypes as a class field so both methods can access it

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Apply the buff to the player to keep the minion alive
			player.AddBuff(Item.buffType, 2);

			// Spawn the minion projectile
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<RiftSwordMinion>(), damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;

			// Prevent the game from spawning another projectile automatically
			return false;
		}

		public int MinionCount = 0;

		public void UpdateEquip(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			player.AddBuff(ModContent.BuffType<RiftSwordBuff>(), 60);

			if (MinionCount <= 3)
			{
			var projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), position, velocity, ModContent.ProjectileType<RiftSwordMinion>(), damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;
			MinionCount += 1;
			}
		}

		public override bool CanUseItem(Player player)
        {
			var modPlayer = Main.LocalPlayer.GetModPlayer<LivingShadowPlayer>();
            if (modPlayer.LivingShadowCurrent == 0)
			{
				return false;
			}
			return base.CanUseItem(player);
        }
		
		public override void AddRecipes() {
			CreateRecipe()
                .AddCondition(Language.GetText("Mods.DestroyerTest.RecipeCondition.Charge"), () => Main.LocalPlayer.HasItem(ModContent.ItemType<Husk_RiftScabbard>()))
				.Register();
		}

		public override void UpdateInventory(Player player)
        {
            NoChargeLeft(player);
        }

		public override bool CanRightClick() {
                return true;
            }

        private const int CrucibleProximityRange = 3;
        private const int RequiredBatteries = 6;

        public override void RightClick(Player player)
        {
            SoundStyle zapSound = new SoundStyle("DestroyerTest/Assets/Audio/RiftCharge");

            if (player.HeldItem != null && player.HeldItem.type == ModContent.ItemType<RiftElectrifier>())
            {
                if (IsNearRiftCrucible(player))
                {
                    ReplenishLivingShadow(player, zapSound, consumeBatteries: false);
                }
                else if (player.CountItem(ModContent.ItemType<RiftBattery>(), RequiredBatteries) >= RequiredBatteries)
                {
                    ConsumeBatteries(player, RequiredBatteries);
                    ReplenishLivingShadow(player, zapSound, consumeBatteries: true);
                }
                else
                {
                    CombatText.NewText(player.Hitbox, ColorLib.Rift, "Six Rift Batteries needed, or, plug the Electrifier into a rift crucible.", true);
                }
            }
        }

        public override bool ConsumeItem(Player player)
        {
            return false; // Prevents the item from being consumed on use
        }

        private bool IsNearRiftCrucible(Player player)
        {
            Point playerTilePosition = player.Center.ToTileCoordinates();
            int riftCrucibleTileType = ModContent.TileType<Tile_RiftCrucible>();

            for (int x = -CrucibleProximityRange; x <= CrucibleProximityRange; x++)
            {
                for (int y = -CrucibleProximityRange; y <= CrucibleProximityRange; y++)
                {
                    Point checkPosition = new Point(playerTilePosition.X + x, playerTilePosition.Y + y);
                    if (Main.tile[checkPosition.X, checkPosition.Y].TileType == riftCrucibleTileType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ConsumeBatteries(Player player, int count)
        {
            int riftBatteryType = ModContent.ItemType<RiftBattery>();
            for (int i = 0; i < count; i++)
            {
                player.ConsumeItem(riftBatteryType);
            }
        }

        private void ReplenishLivingShadow(Player player, SoundStyle zapSound, bool consumeBatteries)
        {
            
            SoundEngine.PlaySound(zapSound, player.position);
            ScreenFlashSystem.FlashIntensity = 0.9f;

            var modPlayer = player.GetModPlayer<LivingShadowPlayer>();
            modPlayer.LivingShadowCurrent = modPlayer.LivingShadowMax2;
        }

		private bool hasReplaced = false;
        private void NoChargeLeft(Player player)
        {
            var modPlayer = player.GetModPlayer<LivingShadowPlayer>();
            if (modPlayer.LivingShadowCurrent == 0 && hasReplaced == false)
			{
            hasReplaced = true;
			Item.NewItem(player.GetSource_FromThis(), player.position, ModContent.ItemType<Husk_RiftScabbard>());
            Item.TurnToAir();
            }
        }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string batteryTooltip = $"Requires {RequiredBatteries} Rift Batteries to recharge.";
			tooltips.Add(new TooltipLine(Mod, "RiftBatteryRequirement", batteryTooltip)
			{
				OverrideColor = ColorLib.Rift // Optional: Set a custom color for the tooltip text
			});
		}

	}

	

	// This minion shows a few mandatory things that make it behave properly.
	// Its attack pattern is simple: If an enemy is in range of 43 tiles, it will fly to it and deal contact damage
	// If the player targets a certain NPC with right-click, it will fly through tiles to it
	// If it isn't attacking, it will float near the player with minimal movement
	public class RiftSwordMinion : ModProjectile
	{
		
        public override string Texture => "DestroyerTest/Content/RiftArsenal/RiftBroadsword";

		public bool PlayedSound = false;
		private void GenerateDust()
		{
			
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.Copper,
					0, 0, 254, Scale: 1.0f);
				dust.velocity += Projectile.velocity * 0.5f;
				dust.velocity *= 0.5f;
				dust.noGravity = true;
		
		}

		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
		}

		public sealed override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 28;
			Projectile.tileCollide = false; // Makes the minion go through tiles freely

			// These below are needed for a minion weapon
			Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.minion = true; // Declares this as a minion (has many effects)
			Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
			Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.netUpdate = true;
		}
		int trailLength = 10; // Adjust for desired effect
		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = ColorLib.Rift;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			// Draw the base projectile using the default drawing system (Deferred)
			Main.EntitySpriteDraw(
				projectileTexture,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor, 
				Projectile.rotation,
				projectileTexture.Size() / 2,
				Projectile.scale,
				SpriteEffects.None,
				0
			);

			// Glow effect (Immediate drawing with Additive blending)
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/RiftSwordGlowmaskColor").Value;
			Main.EntitySpriteDraw(
				glowTexture,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor,
				Projectile.rotation,
				glowTexture.Size() / 2,
				0.1f * Projectile.scale,
				SpriteEffects.None,
				0
			);

			// Restore the deferred mode (for the next drawing of things)
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			return false; // Let the default system handle the base projectile drawing
		}




		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles() {
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage() {
			return true;
		}

		private bool hasReplaced = false;
		public int ChargeCheckTimer = 0;
		public int SearchTimer = 240; // Unrelated to actually searching for targets. 
		private bool EnterOrbitMode(Player owner) {
			return SearchTimer <= 0;
		}

		public override void AI() {
			SearchTimer--;

			ChargeCheckTimer++;

			if (ChargeCheckTimer >= 60)
			{
				ChargeCheckTimer = 0;
				CheckForZeroCharge();
			}


			if (DestroyerTestMod.Config.MinionExtrasToggle == true)
			{
			Commentary(Main.player[Projectile.owner], null, false); // Call commentary for the minion, if needed. Pass in null for NPC to skip it.
			}
			GenerateDust();
			
			Player owner = Main.player[Projectile.owner];

			
			if (!CheckActive(owner)) {
				return;
			}

			GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
			SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
			Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
			Visuals();
		}

		// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
		private bool CheckActive(Player owner) {
			if (owner.dead || !owner.active) {
				owner.ClearBuff(ModContent.BuffType<RiftSwordBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<RiftSwordBuff>())) {
				Projectile.timeLeft = 2;
			}

            
			return true;
		}



		
		private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition) {
			
			GenerateDust();

			Vector2 idlePosition = owner.Center;
			idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

			
			// If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
			// The index is projectile.minionPos
			float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
			idlePosition.X += minionPositionOffsetX; // Go behind the player

			// All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

			// Teleport to player if distance is too big
			vectorToIdlePosition = idlePosition - Projectile.Center;
			distanceToIdlePosition = vectorToIdlePosition.Length();

			if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 1000f) {
				SoundEngine.PlaySound(new SoundStyle ($"DestroyerTest/Assets/Audio/RiftSwordMinionTeleport") with {
				Volume = 1.0f, 
    			Pitch = 0.0f, 
    			PitchVariance = 0.5f, 
			});
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
				// and then set netUpdate to true
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.ChlorophyteLeafCrystalShot,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(Projectile.Hitbox), UniqueInfoPiece = 36 },
				Projectile.owner);
				PlayedSound = false;
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.04f;

			// Fix overlap with other minions
			foreach (var other in Main.ActiveProjectiles) {
				if (other.whoAmI != Projectile.whoAmI && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width) {
					if (Projectile.position.X < other.position.X) {
						Projectile.velocity.X -= overlapVelocity;
					}
					else {
						Projectile.velocity.X += overlapVelocity;
					}

					if (Projectile.position.Y < other.position.Y) {
						Projectile.velocity.Y -= overlapVelocity;
					}
					else {
						Projectile.velocity.Y += overlapVelocity;
					}
				}
			}
		}
		private HashSet<int> soundPlayedForNPCs = new HashSet<int>(); // Track NPCs that triggered the sound

		private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter) {
			// Starting search distance
			distanceFromTarget = 700f;
			targetCenter = Projectile.position;
			foundTarget = false;

			foreach (var npc in Main.npc) {
				if (!npc.active && soundPlayedForNPCs.Contains(npc.whoAmI)) {
					soundPlayedForNPCs.Remove(npc.whoAmI); // Remove NPC from tracking when it dies
				}
			}

			GenerateDust();

			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC) {
				NPC npc = Main.npc[owner.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);

				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f) {
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;

					// Play sound if not already played for this NPC
					if (!soundPlayedForNPCs.Contains(npc.whoAmI)) {
						PlayRadioINSound();
						soundPlayedForNPCs.Add(npc.whoAmI);
					}
				}
			}

			if (!foundTarget) {
				// This code is required either way, used for finding a target
				foreach (var npc in Main.ActiveNPCs) {
					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						bool closeThroughWall = between < 100f;

						if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;

							// Play sound if not already played for this NPC
							if (!soundPlayedForNPCs.Contains(npc.whoAmI)) {
								CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Target Locked!", true);
								PlayRadioINSound();
								soundPlayedForNPCs.Add(npc.whoAmI);
							}
						}
					}
				}
			}

			if (foundTarget) {
				SearchTimer = 240;
			}

			// friendly needs to be set to true so the minion can deal contact damage
			Projectile.friendly = foundTarget;
		}

		private void PlayRadioINSound() {
			SoundStyle RadioIN = new SoundStyle("DestroyerTest/Assets/Audio/RadioIN") {
				Volume = 0.50f
			};
			SoundEngine.PlaySound(RadioIN, Projectile.Center);
		}

		private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition) {
			Player owner = Main.LocalPlayer;

			if (EnterOrbitMode(owner)) {
				int totalMinions = 0;
				int myIndex = 0;

				// Count how many minions of this type are active for this player, and find this one's index
				for (int i = 0; i < Main.maxProjectiles; i++) {
					if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner &&
						Main.projectile[i].type == Projectile.type) {
						if (Main.projectile[i].whoAmI == Projectile.whoAmI) {
							myIndex = totalMinions;
						}
						totalMinions++;
					}
				}

				OrbitPlayer(owner, myIndex, totalMinions);
				return; // Skip regular targeting movement
			}


			float speed = 50f;
			float inertia = 140f;
			
			GenerateDust();

			if (foundTarget) {
				if (distanceFromTarget > 40f) {
					// If not in "strike-through" mode, home in
					if (Projectile.ai[1] == 0) {
						Vector2 direction = targetCenter - Projectile.Center;
						direction.Normalize();
						direction *= speed;

						float targetAngle = Projectile.AngleTo(targetCenter * MathHelper.ToRadians(360));
						Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;

						// If close enough, enter "strike-through" mode
						if (distanceFromTarget < 50f) {
							SoundEngine.PlaySound(SoundID.Item66);
							Projectile.ai[1] = 1; // Enter strike-through phase
							Projectile.ai[0] = 0; // Reset timer
						}
						Projectile.rotation = targetAngle;
					}
				}
			}

			// If in "strike-through" mode, keep moving forward without changing direction
			if (Projectile.ai[1] == 1) {
				Projectile.ai[0]++; // Increment timer

				if (Projectile.ai[0] < 20) {
					// Keep moving in the same direction for a bit
					Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * speed;
				} else {
					// Exit "strike-through" mode after 20 ticks (~1/3 of a second)
					Projectile.ai[1] = 0;
				}
			}

			if (!foundTarget) {
				// Reset "strike-through" state when there's no target
				Projectile.ai[1] = 0;

				if (distanceToIdlePosition > 600f) {
					speed = 12f;
					inertia = 60f;
				}
				else {
					speed = 4f;
					inertia = 80f;
				}

				if (distanceToIdlePosition > 20f) {
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (Projectile.velocity == Vector2.Zero) {
					Projectile.velocity.X = -0.15f;
					Projectile.velocity.Y = -0.05f;
				}
			}
		}


		private void Visuals() {
			// So it will lean slightly towards the direction it's moving
			Projectile.rotation = Projectile.velocity.X * 0.5f;

			GenerateDust();

			// This is a simple "loop through all frames from top to bottom" animation
			//int frameSpeed = 5;

			//Projectile.frameCounter++;

			//if (Projectile.frameCounter >= frameSpeed) {
				//Projectile.frameCounter = 0;
				//Projectile.frame++;

				//if (Projectile.frame >= Main.projFrames[Projectile.type]) {
					//Projectile.frame = 0;
				//}
			//}

			// Some visuals here
			Lighting.AddLight(Projectile.Center, ColorLib.Rift.ToVector3() * 0.78f);

		}


		private int SwitchTimer = 0;

		private void OrbitPlayer(Player owner, int index, int total) {
			float orbitRadius = 120f;
			float orbitSpeed = 0.05f; // Radians per tick
			float angleOffset = MathHelper.TwoPi / total * index;

			// Update the timer and alternate the sword direction every 120 seconds
			SwitchTimer++;
			if (SwitchTimer >= 480) { // 120 seconds = 7200 ticks (60 ticks per second)
				SwitchTimer = 0; // Reset the timer
			}

			// Determine whether the sword faces inward or outward based on SwitchTimer
			bool isFacingInward = (SwitchTimer < 480); // First 60 seconds facing inward, next 60 seconds outward

			float angle = Main.GameUpdateCount * orbitSpeed + angleOffset;
			Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * orbitRadius;

			// Invert the offset direction based on facing mode (inward or outward)
			if (isFacingInward) {
				offset = -offset; // Facing inward: flip the offset direction
			}

			Vector2 desiredPosition = owner.Center + offset;

			Vector2 toPosition = desiredPosition - Projectile.Center;
			float speed = 8f;
			float inertia = 10f;

			Vector2 desiredVelocity = toPosition.SafeNormalize(Vector2.Zero) * speed;
			Projectile.velocity = (Projectile.velocity * (inertia - 1) + desiredVelocity) / inertia;

			// Lean into the movement direction
			//Projectile.rotation = Projectile.velocity.X * 0.5f;
		}


		public int CommentaryTimer = 0;
		public void Commentary(Player player, NPC npc, bool foundtarget)
		{
			CommentaryTimer++;
			
			if (player.HeldItem.type == ModContent.ItemType<RiftBroadsword>() && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Hah! It's me but dumber!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HeldItem.type == ModContent.ItemType<Living_Shadow>() && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Time for a recharge already? I'm bursting with energy though!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HeldItem.type == ModContent.ItemType<Hope_Scabbard>() && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Why use the magic-based shortswords when you can use me, the AI-Powered Broadsword?", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}
			
			if (player.HeldItem.type == ModContent.ItemType<Goliath>() && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Woah, so Regal!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HeldItem.type == ModContent.ItemType<Gargantua>() && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Woah, so Regal!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HeldItem.type == ModContent.ItemType<Laevateinn>() && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "I can tell already you're a michevous type...", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HeldItem.type == ModContent.ItemType<TrueLaevateinn>() && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "I can tell already you're a michevous type...", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HeldItem.type == ModContent.ItemType<SoulEdge>() && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Spooky! I like your style!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HeldItem.type == ModContent.ItemType<TrueSoulEdge>() && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Spooky! I like your style!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HeldItem.type == ItemID.TerraBlade && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Such great power you hold. Why still have me here?", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.InModBiome<RiftSurface>() == true && CommentaryTimer >= 1200) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Hah! I feel empowered here!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (npc != null && npc.boss && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Hah Hah! Time for combat!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HasBuff(BuffID.Darkness) && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, "Don't worry! I'll keep the way lit!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}



			// Past here is "Ambient Dialogue", which more  or less is just the stuff they say when nothing is happening for extended periods of time.

			if (!foundtarget && CommentaryTimer >= 1200) 
			{
				string[] ambientDialogue = new string[]
				{
					"I wonder if the other rift swords are aware of me... Maybe we're all connected somehow?",
					"Cha Cha Real Smooth...",
					"Knock Knock! Who's there? Nobody.",
					"I could use a sharpening... oh wait, I'm energy-based.",
					"The air is notably dry today...",
					"The void feels oddly quiet today...",
					"Blah Blah Blah Blah...",
					"Radio Silent. Do you copy?",
					"Damn, I kinda want some popcorn right now..."
				};

				string randomDialogue = ambientDialogue[Main.rand.Next(ambientDialogue.Length)];

				// Display the randomly selected dialogue
				CombatText.NewText(Projectile.Hitbox, ColorLib.Rift, randomDialogue, true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}
			
		}

		private void CheckForZeroCharge()
		{
			if (hasReplaced || Projectile.owner != Main.myPlayer)
				return;

			Player player = Main.LocalPlayer;
			var modPlayer = player.GetModPlayer<LivingShadowPlayer>();

			if (modPlayer.LivingShadowCurrent <= 0)
			{
				hasReplaced = true;

				// Convert into husk and remove this one
				Projectile.Kill(); // Triggers OnKill
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (!hasReplaced || Projectile.owner != Main.myPlayer)
				return;

			Player player = Main.LocalPlayer;
			var modPlayer = player.GetModPlayer<LivingShadowPlayer>();

			if (modPlayer.LivingShadowCurrent <= 0)
			{
				Projectile.NewProjectileDirect(
					Projectile.GetSource_Death(),
					Projectile.position,
					Projectile.oldVelocity,
					ModContent.ProjectileType<RiftSwordMinionHusk>(),
					0,
					0
				);
			}
		}
	}
}
