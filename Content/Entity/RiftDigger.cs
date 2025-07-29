using DestroyerTest.Content.Entity;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using DestroyerTest.Content.RiftBiome;
using Terraria.ModLoader.Utilities;

namespace DestroyerTest.Content.Entity
{
	// These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
	internal class RiftDiggerHead : WormHead
	{
		public override int BodyType => ModContent.NPCType<RiftDiggerBody>();

		public override int TailType => ModContent.NPCType<RiftDiggerTail>();

		SoundStyle Roar = new SoundStyle("DestroyerTest/Assets/Audio/WormRoar") // The sound played when the worm roars, can be overridden by the tail or body if desired
		{
			Volume = 0.4f,
			PitchVariance = 0.2f,
			MaxInstances = 3 // Allow up to 3 instances of this sound to play at once
		};

		public override void SetStaticDefaults() {
			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers() { // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "DestroyerTest/Content/Entity/RiftDiggerHeadBestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				Position = new Vector2(40f, 24f),
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = 12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
		}

		public override void SetDefaults() {
			// Head is 10 defense, body 20, tail 30.
			NPC.CloneDefaults(NPCID.DiggerHead);
			NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit12;
            NPC.DeathSound = SoundID.NPCDeath26;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange([
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			]);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// Check if the player is in the Shimmer zone and the Cultist Boss has been defeated
			if (ModContent.GetInstance<RiftSurface>().IsBiomeActive(spawnInfo.Player)) // Ensure the Cultist Boss has been defeated
			{
					// Set spawn chance relative to standard overworld night monsters
					return SpawnCondition.OverworldDaySlime.Chance; // 10% of regular zombie spawn rate
				
			}
			return 0f; // Prevent spawning otherwise
		}

		public override void Init() {
			// Set the segment variance
			// If you want the segment length to be constant, set these two properties to the same value
			MinSegmentLength = 6;
			MaxSegmentLength = 12;

			CommonWormInit(this);
		}

		// This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
		internal static void CommonWormInit(Worm worm) {
			// These two properties handle the movement of the worm
			worm.MoveSpeed = 5.5f;
			worm.Acceleration = 0.045f;
		}

		private int attackCounter;
		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(attackCounter);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			attackCounter = reader.ReadInt32();
		}

		public override void AI() {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (attackCounter > 0) {
					attackCounter--; // Tick down the attack counter.
				}

				Player target = Main.player[NPC.target];

				// Play the Roar sound dynamically based on conditions
				if (NPC.HasValidTarget && attackCounter == 0) {
					// Check if the worm is close to the player or has changed direction significantly
					float distanceToPlayer = Vector2.Distance(NPC.Center, target.Center);
					bool significantDirectionChange = Math.Abs(NPC.velocity.X - NPC.oldVelocity.X) > 2f || Math.Abs(NPC.velocity.Y - NPC.oldVelocity.Y) > 2f;

					if (distanceToPlayer < 500f || significantDirectionChange) {
						attackCounter = 120; // Reset the attack counter (e.g., 2 seconds cooldown)
						SoundEngine.PlaySound(Roar, NPC.position); // Play the Roar sound
					}
				}

				// Additional AI logic for movement or other behaviors can go here
			}
		}
	}

	internal class RiftDiggerBody : WormBody
	{
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.RespawnEnemyID[NPC.type] = ModContent.NPCType<RiftDiggerHead>();
		}

		public override void SetDefaults() {
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.aiStyle = -1;

			// Extra body parts should use the same Banner value as the main ModNPC.
			Banner = ModContent.NPCType<RiftDiggerHead>();
		}

		public override void Init() {
			RiftDiggerHead.CommonWormInit(this);
		}
	}

	internal class RiftDiggerTail : WormTail
	{
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPCID.Sets.RespawnEnemyID[NPC.type] = ModContent.NPCType<RiftDiggerHead>();
		}

		public override void SetDefaults() {
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.aiStyle = -1;

			// Extra body parts should use the same Banner value as the main ModNPC.
			Banner = ModContent.NPCType<RiftDiggerHead>();
		}

		public override void Init() {
			RiftDiggerHead.CommonWormInit(this);
		}
	}
}