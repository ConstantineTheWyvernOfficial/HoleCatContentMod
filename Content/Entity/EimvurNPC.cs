
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.Resources.Cloths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using DestroyerTest.Common.Systems;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;

namespace DestroyerTest.Content.Entity
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class EimvurNPC : ModNPC
	{
		
		public int NumberOfTimesTalkedTo = 0;

		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void Load() {
			// Adds our Shimmer Head to the NPCHeadLoader.
			
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
			

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
				// Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
				// If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            // Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
            // NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
            NPC.Happiness
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like) // Example Person prefers the forest.
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Hate) // Example Person dislikes the Desert.
                .SetBiomeAffection<CrimsonBiome>(AffectionLevel.Love) // Example Person likes the Underground
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love) // Loves living near the dryad.
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like) // Likes living near the guide.
				.SetNPCAffection(NPCID.Corruptor, AffectionLevel.Dislike) // Dislikes living near the merchant.
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Hate) // Hates living near the demolitionist.
			; // < Mind the semicolon!

			// This creates a "profile" for ExamplePerson, which allows for different textures during a party and/or while the NPC is shimmered.
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party")
			);
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 238;
			NPC.defense = 60;
			NPC.lifeMax = 27000;
			NPC.HitSound = SoundID.NPCHit55;
			NPC.DeathSound = SoundID.NPCDeath39;
			NPC.knockBackResist = 0f;
			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("The Blood Goddess herself, condensed to a human form. Her more natural form is a little... Uncomfortable... to most townspeople. She cares a lot for her son."),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				//new FlavorTextBestiaryInfoElement("Mods.ExampleMod.Bestiary.ExamplePerson")
			});
		}

		public override void HitEffect(NPC.HitInfo hit) {
			int num = NPC.life > 0 ? 1 : 5;

			for (int k = 0; k < num; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
			}
		}

		public override void OnSpawn(IEntitySource source) {
			if(source is EntitySource_SpawnNPC) {
				// A TownNPC is "unlocked" once it successfully spawns into the world.
				TownNPCRespawnSystem.unlockedEimvurSpawn = true;
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			if (TownNPCRespawnSystem.unlockedEimvurSpawn) {
				// If Example Person has spawned in this world before, we don't require the user satisfying the ExampleItem/ExampleBlock inventory conditions for a respawn.
				return true;
			}

			foreach (var player in Main.ActivePlayers) {
				// Player has to have either an ExampleItem or an ExampleBlock in order for the NPC to spawn
				if (Main.bloodMoon && player.HasBuff<OminousPrescence>()) {
					return true;
				}
			}

			return false;
		}


		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Eimvur",
			};
		}

		public override void FindFrame(int frameHeight) {
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0 && Main.rand.NextBool(4)) {
				chat.Add(Language.GetTextValue("Test for party girl?", Main.npc[partyGirl].GivenName));
			}
			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("What do you want? You smell."));
			chat.Add(Language.GetTextValue("You ever lament things outside your control? I don't."));
			chat.Add(Language.GetTextValue("You mortals have a strange fascination with flowers."));
			chat.Add(Language.GetTextValue("You would've loved to see my son as a little child."));
            chat.Add(Language.GetTextValue("Gövic... I don't know where I am... or if I'll see you again..... Gah! Why must you sneak up on me like that!?"), 5.0);
			chat.Add(Language.GetTextValue("No, I will not let you call me 'Mommy'."), 0.1);

			NumberOfTimesTalkedTo++;
			if (NumberOfTimesTalkedTo >= 10) {
				//This counter is linked to a single instance of the NPC, so if ExamplePerson is killed, the counter will reset.
				chat.Add(Language.GetTextValue("I enjoy conversation but you're tiring me out. Go away."));
			}

			

			if (Main.bloodMoon)
			{
				chat.Add(Language.GetTextValue("All the other women here are miserable tonight. I feel a little guilty for not feeling the pain."));
				chat.Add(Language.GetTextValue("I feel amazing tonight!"));
			}


			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			// Here is some additional logic based on the chosen chat line. In this case, we want to display an item in the corner for StandardDialogue4.
			//if (chosenChat == Language.GetTextValue("Agh, it seems the fabric on Bobby's handle has faded. Would it be any trouble to fetch a new roll of red satin to refit it?")) {
				// Main.npcChatCornerItem shows a single item in the corner, like the Angler Quest chat.
				//Main.npcChatCornerItem = ModContent.ItemType<RedCloth>();
			//}

			return chosenChat;
		}

		

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ItemID.VampireKnives, 1, 10, 26));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toKingStatue) => false;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 140;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 30;
			randExtraCooldown = 20;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			projType = ProjectileID.InsanityShadowFriendly;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 23f;
			randomOffset = 2f;
			// SparklingBall is not affected by gravity, so gravityCorrection is left alone.
		}

		public override void LoadData(TagCompound tag) {
			NumberOfTimesTalkedTo = tag.GetInt("numberOfTimesTalkedTo");
		}

		public override void SaveData(TagCompound tag) {
			tag["numberOfTimesTalkedTo"] = NumberOfTimesTalkedTo;
		}

	}
}