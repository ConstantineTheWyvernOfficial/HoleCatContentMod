
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.MetallurgySeries;
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

namespace DestroyerTest.Content.Entity
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Prospector : ModNPC
	{
		
		public const string ShopName = "Metals";
		public int NumberOfTimesTalkedTo = 0;

		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void Load() {
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
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
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			NPCID.Sets.ShimmerTownTransform[Type] = true; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

			// Connects this NPC with a custom emote.
			// This makes it when the NPC is in the world, other NPCs will "talk about him".
			// By setting this you don't have to override the PickEmote method for the emote to appear.
			NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<ProspectorEmote>();

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
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like) // Example Person prefers the forest.
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike) // Example Person dislikes the Desert.
				.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Love) // Example Person likes the Underground
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Love) // Loves living near the dryad.
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Like) // Likes living near the guide.
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Dislike) // Dislikes living near the merchant.
				.SetNPCAffection(NPCID.Golfer, AffectionLevel.Hate) // Hates living near the demolitionist.
			; // < Mind the semicolon!

			// This creates a "profile" for ExamplePerson, which allows for different textures during a party and/or while the NPC is shimmered.
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, Texture + "_Shimmer_Party")
			);
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("A master of Minerals has entered your town! The Prospector will sell you metals and crystals not available in your world!"),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				//new FlavorTextBestiaryInfoElement("Mods.ExampleMod.Bestiary.ExamplePerson")
			});
		}

		public override void HitEffect(NPC.HitInfo hit) {
			int num = NPC.life > 0 ? 1 : 5;

			for (int k = 0; k < num; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt);
			}

			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				// Retrieve the gore types. This NPC has shimmer and party variants for head, arm, and leg gore. (12 total gores)
				string variant = "";
				if (NPC.IsShimmerVariant) variant += "_Shimmer";
				if (NPC.altTexture == 1) variant += "_Party";
				int hatGore = NPC.GetPartyHatGore();
				int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
				int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
				int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

				// Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
				if (hatGore > 0) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
			}
		}

		public override void OnSpawn(IEntitySource source) {
			if(source is EntitySource_SpawnNPC) {
				// A TownNPC is "unlocked" once it successfully spawns into the world.
				TownNPCRespawnSystem.unlockedProspectorSpawn = true;
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			if (TownNPCRespawnSystem.unlockedProspectorSpawn) {
				// If Example Person has spawned in this world before, we don't require the user satisfying the ExampleItem/ExampleBlock inventory conditions for a respawn.
				return true;
			}

			foreach (var player in Main.ActivePlayers) {
				// Player has to have either an ExampleItem or an ExampleBlock in order for the NPC to spawn
				if (player.inventory.Any(item => item.type == ItemID.CopperOre || item.type == ItemID.Diamond)) {
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
				"Fitzgerald",
				"Ford",
				"Filch",
				"Foster",
				"Fulton",
				"Fennel",
				"Faulkner",
				"Ferris"
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
			chat.Add(Language.GetTextValue("You ever play Sudoku? No? Good, because I don't either."));
			chat.Add(Language.GetTextValue("I used to know this guy named clayton. He had a real thick skull and crumbled under pressure."));
			chat.Add(Language.GetTextValue("I hear you have a startyling deficiency of Beryllium in your storage! You wouldn't believe what I'm selling!"));
			chat.Add(Language.GetTextValue("Theres just something so calm about striking rocks with a pickaxe for hours on end..."));
			chat.Add(Language.GetTextValue("Here to purchase some of my wares? I assure you, I have everything you need! Hm? No, I dont set gems back in place."), 5.0);
			chat.Add(Language.GetTextValue("Y'know, sometimes this lamp pole is so tiring to hold. I have to keep holding it even when I'm asleep."), 0.1);

			NumberOfTimesTalkedTo++;
			if (NumberOfTimesTalkedTo >= 10) {
				//This counter is linked to a single instance of the NPC, so if ExamplePerson is killed, the counter will reset.
				chat.Add(Language.GetTextValue("All bark and no buck! How much time do you have to waste each day?"));
			}

			if (Main.eclipse && !Main.LocalPlayer.ZoneRockLayerHeight || !Main.LocalPlayer.ZoneDirtLayerHeight || !Main.LocalPlayer.ZoneUnderworldHeight)
			{
				chat.Add(Language.GetTextValue("I feel a horrid chill coming down my spine... What is it, fellow?"));
				chat.Add(Language.GetTextValue("Why, I havent seen the sky this dark in my entire life! What is it, fellow?"));
			}

			if (Main.bloodMoon && !Main.LocalPlayer.ZoneRockLayerHeight || !Main.LocalPlayer.ZoneDirtLayerHeight || !Main.LocalPlayer.ZoneUnderworldHeight)
			{
				chat.Add(Language.GetTextValue("How bizarre, I just cried blood... What is it, fellow?"));
				chat.Add(Language.GetTextValue("I can't even be safe in my own home! What is it, fellow?"));
			}

			if (Main.bloodMoon || Main.eclipse && Main.LocalPlayer.ZoneRockLayerHeight || Main.LocalPlayer.ZoneDirtLayerHeight)
			{
				chat.Add(Language.GetTextValue("Hello hello! Come to check out the caves again? Hm? No I don't feel any unease at the moment. Why do you mention it?"));
				chat.Add(Language.GetTextValue("Blood Moon? I had no idea such was happning tonight! Good thing I'm safe underground here!"));
			}

			if (Main.LocalPlayer.ZoneUnderworldHeight)
			{
				chat.Add(Language.GetTextValue("Gee, it's hot as my fathers bollocks down here! Could you remind me why I'm down here again?"));
				chat.Add(Language.GetTextValue("A glass of water would go real nice right now..."));
			}

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			// Here is some additional logic based on the chosen chat line. In this case, we want to display an item in the corner for StandardDialogue4.
			//if (chosenChat == Language.GetTextValue("Agh, it seems the fabric on Bobby's handle has faded. Would it be any trouble to fetch a new roll of red satin to refit it?")) {
				// Main.npcChatCornerItem shows a single item in the corner, like the Angler Quest chat.
				//Main.npcChatCornerItem = ModContent.ItemType<RedCloth>();
			//}

			return chosenChat;
		}
			public override void SetChatButtons(ref string button, ref string button2) { 
				// Define what the chat buttons will display
				button = Language.GetTextValue("LegacyInterface.28");
				button2 = "Where's the other Metallurgy Stuff?"; 
			}

			public override void OnChatButtonClicked(bool button1, ref string shop) {
				if (button1) {
					// Handle button 1 (LegacyInterface.28 button)
					// (Your logic for button 1 goes here if needed)
				} 
				else {
					// If button2 was clicked, show the metallurgic info
					Main.npcChatText = $"Well, you see, it's more a matter of your own sanity. I don't know if you're keenly aware, but many alloys that exist have very similar properties. If there were to be an alloy for every possible combination and items using that alloy, the amount of redundancy would make the mod seem boring as all hell!";
					
					shop = ShopName; // Open the shop linked to this NPC
				}
			}

		// Not completely finished, but below is what the NPC will sell
		public override void AddShops() {
			var npcShop = new NPCShop(Type, ShopName)
				.Add(new Item(ModContent.ItemType<Flux>()) { shopCustomPrice = Item.buyPrice(copper: 40) })
				.Add(new Item(ModContent.ItemType<AluminumOre>()) { shopCustomPrice = Item.buyPrice(copper: 30) })
				.Add(new Item(ModContent.ItemType<BerylliumOre>()) { shopCustomPrice = Item.buyPrice(silver: 51, copper: 90) })
				.Add(new Item(ModContent.ItemType<ManganeseOre>()) { shopCustomPrice = Item.buyPrice(copper: 30) })
				.Add(new Item(ModContent.ItemType<NickelOre>()) { shopCustomPrice = Item.buyPrice(silver: 1) })
				.Add(new Item(ModContent.ItemType<ZincOre>()) { shopCustomPrice = Item.buyPrice(copper: 30) })
				.Add(new Item(ModContent.ItemType<Carbon>()) { shopCustomPrice = Item.buyPrice(copper: 20) })
				.Add(new Item(ModContent.ItemType<Phosphorite>()) { shopCustomPrice = Item.buyPrice(copper:20) });
			npcShop.Register(); // Name of this shop tab
		}

		public override void ModifyActiveShop(string shopName, Item[] items) {
			foreach (Item item in items) {
				// Skip 'air' items and null items.
				if (item == null || item.type == ItemID.None) {
					continue;
				}

				// If NPC is shimmered then reduce all prices by 50%.
				if (NPC.IsShimmerVariant) {
					int value = item.shopCustomPrice ?? item.value;
					item.shopCustomPrice = value / 2;
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ItemID.Grenade, 1, 10, 26));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toKingStatue) => true;

		// Make something happen when the npc teleports to a statue. Since this method only runs server side, any visual effects like dusts or gores have to be synced across all clients manually.
		public override void OnGoToStatue(bool toKingStatue) {
			if (Main.netMode == NetmodeID.Server) {
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)NPC.whoAmI);
				packet.Send();
			}
			else {
				StatueTeleport();
			}
		}

		// Create a square of pixels around the NPC on teleport.
		public void StatueTeleport() {
			for (int i = 0; i < 30; i++) {
				Vector2 position = Main.rand.NextVector2Square(-20, 21);
				if (Math.Abs(position.X) > Math.Abs(position.Y)) {
					position.X = Math.Sign(position.X) * 20;
				}
				else {
					position.Y = Math.Sign(position.Y) * 20;
				}

				Dust.NewDustPerfect(NPC.Center + position, DustID.Dirt, Vector2.Zero).noGravity = true;
			}
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			projType = ProjectileID.Grenade;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
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