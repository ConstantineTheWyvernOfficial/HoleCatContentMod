
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

using Terraria.Localization;
using DestroyerTest.Content.Entity;
using System.Linq;

namespace DestroyerTest.Content.SummonItems
{
	// This file contains all the code necessary for a minion
	// - ModItem - the weapon which you use to summon the minion with
	// - ModBuff - the icon you can click on to despawn the minion
	// - ModProjectile - the minion itself

	// It is not recommended to put all these classes in the same file. For demonstrations sake they are all compacted together so you get a better overview.
	// To get a better understanding of how everything works together, and how to code minion AI, read the guide: https://github.com/tModLoader/tModLoader/wiki/Basic-Minion-Guide
	// This is NOT an in-depth guide to advanced minion AI
	public class PosessedPickaxeBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
		}

		public override void Update(Player player, ref int buffIndex) {
			// If the minions exist reset the buff time, otherwise remove the buff from the player
			if (player.ownedProjectileCounts[ModContent.ProjectileType<PosessedPickaxeProjectile>()] > 0) {
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
	public class PosessedPickaxe : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f; // The default value is 1, but other values are supported. See the docs for more guidance. 
		}

		public override void SetDefaults() {
			Item.damage = 0;
			Item.knockBack = 0f;
			Item.mana = 10; // mana cost
			Item.width = 14;
			Item.height = 14;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.RaiseLamp; // how the player's arm moves when using the item
			Item.value = 18000;
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.UseSound = SoundID.Item1;
			Item.accessory = true;

			// These below are needed for a minion weapon
			Item.noMelee = true; // this item doesn't do any melee damage
			Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
			Item.buffType = ModContent.BuffType<PosessedPickaxeBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<PosessedPickaxeProjectile>(); // This item creates the minion projectile
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
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<PosessedPickaxeProjectile>(), damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;

			// Prevent the game from spawning another projectile automatically
			return false;
		}

		public void UpdateEquip(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			player.AddBuff(ModContent.BuffType<PosessedPickaxeBuff>(), 60);

			
				var projectile = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<PosessedPickaxeProjectile>(), damage, knockback, Main.myPlayer);
				projectile.originalDamage = Item.damage;
			
		}
	}

	

	// This minion shows a few mandatory things that make it behave properly.
	// Its attack pattern is simple: If an enemy is in range of 43 tiles, it will fly to it and deal contact damage
	// If the player targets a certain NPC with right-click, it will fly through tiles to it
	// If it isn't attacking, it will float near the player with minimal movement
	public class PosessedPickaxeProjectile : ModProjectile
	{
		// In your projectile class
		private enum MinionState {
			Idle,       // Before orbit
			Orbiting,   // Default state
			Attacking   // Aggro on Prospector
		}
		private MinionState state = MinionState.Idle;

		
        public override string Texture => "DestroyerTest/Content/SummonItems/PosessedPickaxeMinion";

		public bool PlayedSound = false;
		private void GenerateDust()
		{
			
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.Iron,
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
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
		}
		int trailLength = 10; // Adjust for desired effect
		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			// Draw the base projectile using the default drawing system (Deferred)
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

			// Glow effect (Immediate drawing with Additive blending)
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/PosessedPickaxeGlowmaskColor").Value;
			Main.EntitySpriteDraw(
				glowTexture,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor,
				Projectile.rotation,
				glowTexture.Size() / 2,
				0.09f * Projectile.scale,
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

		public int SearchTimer = 240; // Unrelated to actually searching for targets. 
		private bool EnterOrbitMode(Player owner) {
			return SearchTimer <= 0;
		}

		public override void AI()
		{
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile otherProjectile = Main.projectile[i];
				if (otherProjectile.active && otherProjectile.type == Projectile.type && otherProjectile.whoAmI != Projectile.whoAmI)
					{
						Projectile.Kill(); // Kill this projectile if another one is active
						return;
					}
			}
			Player owner = Main.player[Projectile.owner];
			NPC prospector = Main.npc.FirstOrDefault(n => n.active && n.type == ModContent.NPCType<Prospector>());

			switch (state)
			{
				case MinionState.Idle:
					Projectile.velocity = new Vector2(0, -0.02f); // gentle float
					SearchTimer--;
					if (SearchTimer <= 0)
						state = MinionState.Orbiting;
					break;

				case MinionState.Orbiting:
					if (prospector != null && Vector2.Distance(owner.Center, prospector.Center) < 500f && DTUtils.PromiseEquipped == false)
					{
						CombatText.NewText(Projectile.Hitbox, Color.IndianRed, "RAH! YOU! LEMME AT HIM! LEMME AT HIM!", true);
						Projectile.hostile = true;
						Projectile.friendly = false;
						state = MinionState.Attacking;
						SoundEngine.PlaySound(SoundID.Item119);
					}
					else
					{
						DoOrbit(owner);
					}
					break;

				case MinionState.Attacking:
					if (prospector == null || !prospector.active)
					{
						Projectile.hostile = false;
						Projectile.friendly = true;
						state = MinionState.Orbiting;
						break;
					}

					Vector2 attackVel = (prospector.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 10f;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, attackVel, 0.1f); // homing
					break;
			}
			CheckActive(owner);
			GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
			Movement(distanceToIdlePosition, vectorToIdlePosition);
			GenerateDust();
			Visuals();
			Commentary(Main.LocalPlayer);
		}


		// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
		private bool CheckActive(Player owner) {
			if (owner.dead || !owner.active) {
				owner.ClearBuff(ModContent.BuffType<PosessedPickaxeBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<PosessedPickaxeBuff>())) {
				Projectile.timeLeft = 2;
			}

            
			return true;
		}


		public bool flag1 = false;
		public bool flag2 = false;
		public bool flag3 = false;
		public int PromiseTimer = 0;
		private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition) {
			
			GenerateDust();

			Vector2 idlePosition = owner.Center;
			idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

			// All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

			// Teleport to player if distance is too big
			vectorToIdlePosition = idlePosition - Projectile.Center;
			distanceToIdlePosition = vectorToIdlePosition.Length();

			if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 1000f) {
				SoundEngine.PlaySound(SoundID.Item104);
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

			if (DTUtils.PromiseEquipped == true)
			{
				
				PromiseTimer++;
				if (PromiseTimer < 60 && PromiseTimer > 0 && flag1 == false)
				{
				CombatText.NewText(Projectile.Hitbox, Color.DeepPink, "Wait, he really said sorry? He even put it in writing?", true);
				PlayRadioINSound();
				flag1 = true;
				}
				else if (PromiseTimer < 120 && PromiseTimer > 60 && flag2 == false)
				{
				CombatText.NewText(Projectile.Hitbox, Color.DeepPink, "Gee, I oughta thank him sometime... Maybe...", true);
				PlayRadioINSound();
				flag2 = true;
				}
				else if (PromiseTimer < 180 && PromiseTimer > 120 && flag3 == false)
				{
				CombatText.NewText(Projectile.Hitbox, Color.DeepPink, "As a token of gratitude, I'll stop hoarding some of my curse's perks for  myself.", true);
				PlayRadioINSound();
				flag3 = true;
				}
				owner.magicLantern = true;
				owner.dangerSense = true;
			}
		}

		private void DoOrbit(Player owner)
		{
			float orbitRadius = 80f; // Distance from player
			float orbitSpeed = 0.05f; // Radians per tick

			// Store and increment a local AI timer for rotation
			Projectile.localAI[0] += orbitSpeed;

			// Keep it from going out of float range
			if (Projectile.localAI[0] > MathHelper.TwoPi)
				Projectile.localAI[0] -= MathHelper.TwoPi;

			// Calculate orbit position
			Vector2 orbitOffset = new Vector2((float)Math.Cos(Projectile.localAI[0]), (float)Math.Sin(Projectile.localAI[0])) * orbitRadius;
			Vector2 desiredPosition = owner.Center + orbitOffset;
			Vector2 direction = desiredPosition - Projectile.Center;

			Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction, 0.2f);
		}

		

		private void PlayRadioINSound() {
			SoundStyle ChimeIn = new SoundStyle("DestroyerTest/Assets/Audio/ChimeIn");
			SoundEngine.PlaySound(ChimeIn, Projectile.Center);
		}

		private void Movement(float distanceToIdlePosition, Vector2 vectorToIdlePosition) {
			Player owner = Main.LocalPlayer;

			if (EnterOrbitMode(owner)) {

				OrbitPlayer(owner);
				return; // Skip regular targeting movement
			}
			else
			{
				Projectile.velocity += new Vector2(0, -0.7f);
			}
			
			GenerateDust();
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
			Lighting.AddLight(Projectile.Center, Color.SkyBlue.ToVector3() * 0.78f);

		}

		private void OrbitPlayer(Player owner) {
			float orbitRadius = 120f;
			float orbitSpeed = 0.05f; // Radians per tick
			float angleOffset = MathHelper.TwoPi;

			float angle = Main.GameUpdateCount * orbitSpeed + angleOffset;
			Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * orbitRadius;

			Vector2 desiredPosition = owner.Center + offset;

			Vector2 toPosition = desiredPosition - Projectile.Center;
			float speed = 8f;
			float inertia = 10f;

			// Calculate desired velocity to move toward the position
			Vector2 desiredVelocity = toPosition.SafeNormalize(Vector2.Zero) * speed;

			// Smooth the transition of the velocity for a natural feel
			Projectile.velocity = (Projectile.velocity * (inertia - 1) + desiredVelocity) / inertia;
		}



		public int CommentaryTimer = 0;
		public void Commentary(Player player)
		{
			CommentaryTimer++;

			if (player.HeldItem.type == ItemID.IronPickaxe && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, Color.SkyBlue, "Oh how I long to just be a miner with my own pickaxe instead of being this Unholy thing...", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			if (player.HasBuff(BuffID.Darkness) && CommentaryTimer >= 360) 
			{
				CombatText.NewText(Projectile.Hitbox, Color.SkyBlue, "Don't worry! I'll keep the way lit!", true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}

			// Past here is "Ambient Dialogue", which more  or less is just the stuff they say when nothing is happening for extended periods of time.

			if (CommentaryTimer >= 1200) 
			{
				string[] ambientDialogue = new string[]
				{
					"We'll meet again... Dont know where, Dont know when...",
					"Falsehoods. All of them...",
					"Ores, ores ores... Geoffrey had some ores...",
					"Dah Dadah Dadah...",
					"I yearn for the cracking of obsidian...",
					"The void feels oddly quiet today...",
					"Tung Tung Tung Tung Tung Tung Sahur",
					"I'd have fallen asleep by now...",
					"That damned fool, when I get my hands on him..."
				};

				string randomDialogue = ambientDialogue[Main.rand.Next(ambientDialogue.Length)];

				// Display the randomly selected dialogue
				CombatText.NewText(Projectile.Hitbox, Color.SkyBlue, randomDialogue, true);
				PlayRadioINSound();
				CommentaryTimer = 0;
			}
			
		}
	}
}
