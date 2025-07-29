using DestroyerTest.Common.Systems;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.RiftBiome;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
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
	public class TenebrousSlime : ModNPC
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
            Main.npcFrameCount[NPC.type] = 2; // Set this to the number of frames in your sprite
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
				new FlavorTextBestiaryInfoElement("A conglomeration of Dark energies born of the Rift. Supposedly the one thing holding these energies back from this world was the cultists' ritual. Touching it will burn you severely."),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				//new FlavorTextBestiaryInfoElement("Mods.ExampleMod.Bestiary.ExamplePerson")
				
			});
			bestiaryEntry.Info.AddRange([
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns
			]);
		}

		public override void SetDefaults() {
			NPC.width = 32;
			NPC.height = 52;
			NPC.aiStyle = 1;
			NPC.damage = 15;
			NPC.defense = 12;
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = false;
			// Sets the above
			NPC.lavaImmune = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.Player;

            if (player.ZoneShimmer && DownedBossSystem.downedCultistBoss)
            {
                return 0.8f; // Or whatever spawn chance you want in the Dungeon
            }

            return 0f; // Prevent spawning otherwise
        }



        public override void FindFrame(int frameHeight) {
            NPC.frameCounter++; // Increments every tick (60 times per second)
            if (NPC.frameCounter >= 10) { // Change frames every 10 ticks
                NPC.frame.Y += frameHeight; // Move to the next frame
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type]) {
                NPC.frame.Y = 0; // Loop back to the first frame
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<ShimmeringFlames>(), 120, true, false);
        }
    }
}