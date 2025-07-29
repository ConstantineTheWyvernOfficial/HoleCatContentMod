using DestroyerTest.Content.Entity;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Specialized;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Entity
{
	[AutoloadBossHead]
	// These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
	internal class ExoDestroyerHead : WormHead
	{
		private enum AIState {
		Idle,
		ProbeSpawn,
		Spin,
		Ram
		}

		private AIState currentState = AIState.Idle;
		private int stateTimer = 0;
		private int attackCounter = 0;
		private float spinRadius = 200f; // Adjust as needed
		private float spinAngle = 0f;
		public override int BodyType => ModContent.NPCType<ExoDestroyerBody>();

		public override int TailType => ModContent.NPCType<ExoDestroyerTail>();

		public override void SetStaticDefaults() {
			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers() { // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "DestroyerTest/Content/Entity/ExoDestroyerBestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				Position = new Vector2(40f, 24f),
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = 12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
		}

		public override void SetDefaults() {
			// Head is 10 defense, body 20, tail 30.
			//NPC.CloneDefaults(NPCID.Worm);
			NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath45;
			NPC.boss = true;
			NPC.lifeMax = 56000;
			NPC.noTileCollide = true;
			NPC.width = 76;
			NPC.height = 104;
			NPC.noGravity = true; // Just in case
			NPC.knockBackResist = 0f;
			NPC.damage = 35;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange([
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			]);
		}

		public override void Init() {
			// Set the segment variance
			// If you want the segment length to be constant, set these two properties to the same value
			MinSegmentLength = 50;
			MaxSegmentLength = 50;

			CommonWormInit(this);
		}

		// This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
		internal static void CommonWormInit(Worm worm) {
			// These two properties handle the movement of the worm
			worm.MoveSpeed = 15f;
			worm.Acceleration = 0.045f;
		}

		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(attackCounter);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			attackCounter = reader.ReadInt32();
		}

		public override void AI() {
		if (Main.netMode == NetmodeID.MultiplayerClient) return;

			Player target = Main.player[NPC.target];

			if (stateTimer > 0) {
        	stateTimer--;
			} else {
				// Pick a random attack if idle or if the previous attack finished
				if (currentState == AIState.Idle || currentState == AIState.Ram) { // Ram might need a cooldown
					currentState = (AIState)Main.rand.Next(1, 4); // Choose a new attack state
					stateTimer = 360; // Each attack lasts 6 seconds
					attackCounter = 0; // Reset attack counter
				} else {
					currentState = AIState.Idle;
					stateTimer = 60; // 1 second idle before new attack
				}
			}

			switch (currentState) {
				case AIState.Idle:
					//MoveErratically();
					break;

				case AIState.ProbeSpawn:
					MoveWhileSpawningProbes(target);
					SpawnProbes(target);
					break;

				case AIState.Spin:
					MoveWhileSpinning(target);
					SpinAroundPlayer(target);
					break;

				case AIState.Ram:
					AdjustRamPath(target);
					RamPlayer(target);
					break;
			}


			if (stateTimer <= 0) {
				currentState = AIState.Idle;
				stateTimer = 60; // 1 second of idle before picking a new attack
			}

			if (NPC.velocity.Length() < 0.5f) {
				NPC.velocity += new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
			}

			NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
		}

		//private void MoveErratically() {
			//if (Main.rand.NextBool(50)) { // Change direction roughly every 10 ticks
				//Vector2 randomDirection = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)).SafeNormalize(Vector2.Zero);
				//NPC.velocity = randomDirection * Main.rand.NextFloat(6f, 12f); // Random speed
			//}

			// Add some smooth acceleration/deceleration for more natural movement
			//NPC.velocity *= 0.98f; // Slightly slows down over time
		//}
		private void SpawnProbes(Player target) {
			if (attackCounter % 30 == 0) { // Spawn every 30 ticks
				int probe = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ExoProbe>());
				Main.npc[probe].target = NPC.target;
			}
			attackCounter++;
		}

		private void SpinAroundPlayer(Player target) {
			spinAngle += MathHelper.ToRadians(10f);
			Vector2 offset = new Vector2((float)Math.Cos(spinAngle), (float)Math.Sin(spinAngle)) * spinRadius;
			NPC.Center = target.Center + offset;
		}

		private void RamPlayer(Player target) {
			Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);
			float maxDistance = 30 * 16f; // 30 tiles (1 tile = 16 pixels)
			
			if (Vector2.Distance(NPC.Center, target.Center) > maxDistance) {
				NPC.velocity = direction * 20f; // Moves towards player
			} else {
				NPC.velocity *= 0.9f; // Slow down when within range
			}
		}

		private void MoveWhileSpawningProbes(Player target) {
			// Slow, drifting movement while spawning
			Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);
			NPC.velocity = direction * 5f + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
		}

		private void MoveWhileSpinning(Player target) {
			// Slightly adjust distance to stay within a circular range
			Vector2 offset = NPC.Center - target.Center;
			float distance = offset.Length();

			if (distance < spinRadius - 20) {
				NPC.velocity += offset.SafeNormalize(Vector2.Zero) * 0.5f; // Slightly move outward
			} else if (distance > spinRadius + 20) {
				NPC.velocity -= offset.SafeNormalize(Vector2.Zero) * 0.5f; // Slightly move inward
			}

			NPC.velocity *= 0.97f; // Slow down to prevent excessive drifting
		}

		private void AdjustRamPath(Player target) {
			// Adjust course dynamically while ramming
			Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero);
			NPC.velocity = Vector2.Lerp(NPC.velocity, direction * 20f, 0.1f); // Smoothly adjust path
		}
	}

	internal class ExoDestroyerBody : WormBody
	{
        public override string Texture => "DestroyerTest/Content/Entity/ExoDestroyerBody_T1";
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.RespawnEnemyID[NPC.type] = ModContent.NPCType<ExoDestroyerHead>();
		}

		public override void SetDefaults() {
			//NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.width = 100;
			NPC.height = 32;
			NPC.aiStyle = -1;
			NPC.lifeMax = 6000;
			NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath45;
			NPC.noTileCollide = true;
			NPC.noGravity = true; // Just in case
			NPC.timeLeft = 90000;
			NPC.boss = true;
			NPC.knockBackResist = 0f;
			NPC.damage = 35;

			// Extra body parts should use the same Banner value as the main ModNPC.
			Banner = ModContent.NPCType<ExoDestroyerHead>();
		}


		public override void Init() {
			ExoDestroyerHead.CommonWormInit(this);
		}
	}

	internal class ExoDestroyerTail : WormTail
	{
		public override string Texture => "DestroyerTest/Content/Entity/ExoDestroyerTail";
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.RespawnEnemyID[NPC.type] = ModContent.NPCType<ExoDestroyerHead>();
		}

		public override void SetDefaults() {
			//NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.aiStyle = -1;
			NPC.width = 56;
			NPC.height = 90;
			NPC.lifeMax = 6000;
			NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath45;
			NPC.noTileCollide = true;
			NPC.noGravity = true; // Just in case
			NPC.timeLeft = 90000;
			NPC.boss = true;
			NPC.knockBackResist = 0f;
			NPC.damage = 35;
			

			// Extra body parts should use the same Banner value as the main ModNPC.
			Banner = ModContent.NPCType<ExoDestroyerHead>();
		}

		public override void Init() {
			ExoDestroyerHead.CommonWormInit(this);
		}
	}

	public class ExoDestroyerScene : ModSceneEffect
	{
		public override bool IsSceneEffectActive(Player player)
		{
			return NPC.AnyNPCs(ModContent.NPCType<ExoDestroyerHead>()) ||
				NPC.AnyNPCs(ModContent.NPCType<ExoDestroyerBody>()) ||
				NPC.AnyNPCs(ModContent.NPCType<ExoDestroyerTail>());
		}


		public override int Music
			=> MusicLoader.GetMusicSlot(Mod, "Assets/Music/OrdealBoss");

		public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
	}
}