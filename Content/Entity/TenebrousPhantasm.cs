using DestroyerTest.Common.Systems;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.RiftBiome;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DestroyerTest.Content.Entity
{
	/// <summary>
	/// This file shows off a critter npc. The unique thing about critters is how you can catch them with a bug net.
	/// The important bits are: Main.npcCatchable, NPC.catchItem, and Item.makeNPC.
	/// We will also show off adding an item to an existing RecipeGroup (see ExampleRecipes.AddRecipeGroups).
	/// Additionally, this example shows an involved IL edit.
	/// </summary>
	public class TenebrousPhantasm : ModNPC
	{

		/// <summary>
		/// Change the following code sequence in Wiring.HitWireSingle
		/// <code>
		///case 61:
		///num115 = 361;
		/// </code>
		/// to
		/// <code>
		///case 61:
		///num115 = Main.rand.NextBool() ? 361 : NPC.type
		/// </code>
		/// This causes the frog statue to spawn this NPC 50% of the time
		/// </summary>
		/// <param name="ilContext"> </param>

		public override void SetStaticDefaults() {
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
				// Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
				// If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("A skittish cloud of Tenebris that has used the carcass of an Eater of Souls for protection. Supposedly the one thing holding these energies back from this world was the cultists' ritual. Touching it will burn you severely."),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				//new FlavorTextBestiaryInfoElement("Mods.ExampleMod.Bestiary.ExamplePerson")
			});

			bestiaryEntry.Info.AddRange([
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption
			]);
		}

		public override void SetDefaults() {
			NPC.width = 42;
			NPC.height = 78;
			NPC.damage = 21;
			NPC.defense = 24;
			NPC.lifeMax = 500;
			NPC.HitSound = SoundID.NPCHit27;
			NPC.DeathSound = SoundID.NPCDeath24;
            NPC.noGravity = true;
			// Sets the above
			NPC.lavaImmune = true;
			NPC.noTileCollide = false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// Check if the player is in the Corruption zone and the Cultist Boss has been defeated
			if (spawnInfo.Player.ZoneCorrupt && DownedBossSystem.downedCultistBoss)
			{
					return 0.8f; 
			}
			return 0f; // Prevent spawning otherwise
		}

		public override void AI()
		{
			Player player = Main.player[NPC.target]; // Get the target player
			NPC.TargetClosest(faceTarget: true); // Target the closest player and face them

			// Rotate to face the player
			Vector2 directionToPlayer = player.Center - NPC.Center;
			NPC.rotation = directionToPlayer.ToRotation() + MathHelper.PiOver2;

			// Fly towards the player
			float speed = 4f; // Movement speed
			Vector2 moveTo = player.Center - NPC.Center;
			float distance = moveTo.Length();
			if (distance > 20f) // Prevent jittering when close to the player
			{
				moveTo.Normalize();
				moveTo *= speed;
				NPC.velocity = (NPC.velocity * 20f + moveTo) / 21f; // Smooth movement
			}

			// Handle tile collision
			if (NPC.collideX)
			{
				NPC.velocity.X = -NPC.velocity.X * 0.7f; // Reverse and reduce X velocity
				if (NPC.direction == 1) NPC.position.X += 10f; // Push away from the tile
				else NPC.position.X -= 10f;
			}
			if (NPC.collideY)
			{
				NPC.velocity.Y = -NPC.velocity.Y * 0.7f; // Reverse and reduce Y velocity
				if (NPC.velocity.Y > 0) NPC.position.Y -= 10f; // Push away from the tile
				else NPC.position.Y += 10f;
			}

			// AI states
			if (NPC.ai[0] == 0) // State 0: Idle
			{
				NPC.ai[1]++;
				if (NPC.ai[1] >= 120) // After 2 seconds, switch to shooting state
				{
					NPC.ai[0] = 1;
					NPC.ai[1] = 0;
				}
			}
			else if (NPC.ai[0] == 1) // State 1: Shoot
			{
				NPC.ai[1]++;
				if (NPC.ai[1] % 60 == 0) // Fire a projectile every second
				{
					Vector2 shootDirection = directionToPlayer;
					shootDirection.Normalize();
					shootDirection *= 10f; // Projectile speed
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootDirection, ModContent.ProjectileType<TenebrisFireBall>(), 15, 1f, Main.myPlayer);
					SoundEngine.PlaySound(SoundID.Item20, NPC.position); // Play shooting sound
				}

				if (NPC.ai[1] >= 180) // After 3 seconds, switch back to idle state
				{
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
				}
			}
		}
		

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<ShimmeringFlames>(), 120, true, false);
        }
    }
}