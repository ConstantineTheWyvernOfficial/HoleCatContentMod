using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Common.Systems;
using DestroyerTest.Content.Entity;
using Terraria.Audio;
using DestroyerTest.Content.BossBars;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.Magic.ScepterSubclass;

namespace DestroyerTest.Common.NPC_Folder
{
	internal class DTGlobal : GlobalNPC
	{
		// TODO, npc.netUpdate when this changes, and GlobalNPC gets a SendExtraAI hook
		public override bool InstancePerEntity => true;
        public bool DaylightOverload;

         private bool hasNotified = false;

		public override void ResetEffects(NPC npc)
        {
            DaylightOverload = true;
        }

       // Override the OnKill method for the Golem boss
        public override void OnKill(NPC npc)
        {
            // Check if the killed NPC is the Golem
            if (npc.type == NPCID.Golem)
            {
                // The first time this boss is killed, spawn ExampleOre into the world. This code is above SetEventFlagCleared because that will set downedMinionBoss to true.
                if (!DownedBossSystem.downedGolemBoss)
                {
                    ModContent.GetInstance<HeliciteSystem>().BlessWorldWithHelicite();
                }

                // This sets downedGolemBoss to true, and if it was false before, it initiates a lantern night
                DownedBossSystem.downedGolemBoss = true;

                // If needed, manually sync the world when the boss is killed
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
              if (npc.type == NPCID.CultistBoss)
            {
                // The first time this boss is killed, spawn ExampleOre into the world. This code is above SetEventFlagCleared because that will set downedMinionBoss to true.
                if (!DownedBossSystem.downedCultistBoss)
                {
                    ModContent.GetInstance<TenebrousSlime>();
                }

                // This sets downedGolemBoss to true, and if it was false before, it initiates a lantern night
                DownedBossSystem.downedGolemBoss = true;

                // If needed, manually sync the world when the boss is killed
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }

            // Check if the killed NPC is the Lunatic Cultist
            if (npc.type == NPCID.CultistBoss && !hasNotified)
            {
                // Play a sound effect (e.g., Item59 is the piggy bank sound)
                SoundStyle TenebrisSpawn = new SoundStyle("DestroyerTest/Assets/Audio/TenebrisSpawn");
                SoundEngine.PlaySound(TenebrisSpawn);

                // Send a chat message to all players
                Main.NewText("Strange Energies have been released into your world...", 255, 0, 0);

                // Ensure the notification happens only once per world
                hasNotified = true;
            }
        }


        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.SantaClaus)
            {

                shop.Add<WinterWonderland>(Condition.Hardmode);

            }

            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add<CursedStar>(Condition.DownedSkeletron); // Or wherever your item is
            }
            
            if (shop.NpcType == NPCID.TravellingMerchant)
            {
                shop.Add<FoxScepter>(Condition.DownedKingSlime); // Or wherever your item is
            }
        }

        public void SetupBossBar(NPC npc)
        {
            if (npc.boss)
            {
                npc.BossBar = ModContent.GetInstance<RiftBossBar>();
            }
        }

	}
}