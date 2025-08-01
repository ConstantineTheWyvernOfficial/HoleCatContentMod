
using DestroyerTest.Content.Entity;
using DestroyerTest.Rarity;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Consumables
{
    // This is the item used to summon a boss, in this case the modded Minion Boss from Example Mod. For vanilla boss summons, see comments in SetStaticDefaults
    public class TheBotanistsCurse : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.

            // If this would be for a vanilla boss that has no summon item, you would have to include this line here:
            // NPCID.Sets.MPAllowedEnemies[NPCID.Plantera] = true;

            // Otherwise the UseItem code to spawn it will not work in multiplayer
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 42;
            Item.maxStack = 20;
            Item.value = 1000;
            Item.rare = ModContent.RarityType<CorruptionSpecialRarity>();
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossSpawners;
        }

        public override bool CanUseItem(Player player)
        {
            // If you decide to use the below UseItem code, you have to include !NPC.AnyNPCs(id), as this is also the check the server does when receiving MessageID.SpawnBoss.
            // If you want more constraints for the summon item, combine them as boolean expressions:
            //    return !Main.IsItDay() && !NPC.AnyNPCs(ModContent.NPCType<MinionBossBody>()); would mean "not daytime and no MinionBossBody currently alive"
            return !NPC.AnyNPCs(ModContent.NPCType<NightmareRoseBoss>()) && player.ZoneCorrupt;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // If the player using the item is the client
                // (explicitly excluded serverside here)
                SoundStyle Summon = new SoundStyle("DestroyerTest/Assets/Audio/NightmareRose/SoulSummon");
                SoundEngine.PlaySound(Summon, player.position);

                int type = ModContent.NPCType<NightmareRoseBoss>();

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // If the player is not in multiplayer, spawn directly
                    NPC.NewNPC(player.GetSource_ItemUse(Item), (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, type, 0);
                }
                else
                {
                    // If the player is in multiplayer, request a spawn
                    // This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
            }

            return true;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient<NightmarePowder>()
                .AddIngredient(ItemID.DemoniteOre, 8)
                .AddIngredient(ItemID.CorruptSeeds, 1)
                .AddIngredient(ItemID.Book)
				.Register();
		}

    }
    
    
    
}