using DestroyerTest.Common.Systems;
using DestroyerTest.Content.RiftBiome;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DestroyerTest.Content.Entity
{

	public class PossessedScepter : ModNPC
	{

	

		public override void SetStaticDefaults() {
		}

		public override void SetDefaults() {
			NPC.width = 58;
			NPC.height = 58;
			NPC.aiStyle = 23;
			NPC.damage = 34;
			NPC.defense = 0;
			NPC.lifeMax = 1250;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath39;
            NPC.noGravity = true;
			NPC.lavaImmune = true;
            NPC.noTileCollide = true;
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement("A scepter, perhaps belonging to an ancient royal. Now its dusty form has been animated by tormented spirits."),

                
			});
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.Player;

            if (player.ZoneDungeon && DownedBossSystem.downedPlanteraBoss)
            {
                return 0.2f; // Or whatever spawn chance you want in the Dungeon
            }

            return 0f; // Prevent spawning otherwise
        }
    }
}