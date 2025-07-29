using DestroyerTest.Common;
using DestroyerTest.Content.Projectiles.ConstitutionBoss;
using DestroyerTest.Content.RiftBiome;
using DestroyerTest.Content.Buffs;
using log4net.Repository.Hierarchy;
using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Steamworks;
using System;
using System.Collections;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using System.Collections.Generic;
using static DestroyerTest.Content.Entity.ConstitutionClone;
using System.Linq;
using ReLogic.Content;
using DestroyerTest.Content.Projectiles.VampireBoss;

namespace DestroyerTest.Content.Entity
{
    [AutoloadBossHead]
    public class VampireBoss : ModNPC
    {
        public override string BossHeadTexture => "DestroyerTest/Content/Entity/VampireBoss_Head_Boss";
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CanHitPastShimmer[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = false;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = false;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Oiled] = false;
            NPCID.Sets.TrailCacheLength[Type] = 20;
            NPCID.Sets.TrailingMode[Type] = 3;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "DestroyerTest/Content/Entity/VampireBossBestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = Vector2.Zero,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            NPC.width = 70;
            NPC.height = 64;
            NPC.aiStyle = -1;
            NPC.damage = 24;
            NPC.defense = 17;
            NPC.lifeMax = 600;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0f;
            NPC.timeLeft = 150000;
            NPC.boss = true;
            NPC.npcSlots = 10f;
            
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("Gentleman by day, Predator by night, the Vampire is the child of The Blood Goddess; Eimvur, The Witchcraft Goddess; Hequain (also known as Hekate), and a woman; Gövic. His nature is mostly unknown, though perhaps he may open up if you talk to him enough."),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
            });
        }

        public override bool CheckActive()
        {
            return false;
        }

        public enum AttackState
        {
            Idle,
            ShootBlood,
            BrethrenBats,
            GoldenShots,
            ChargeNoBite,
            ChargeAndBite,
            DarkHands,
            GetHelpFromMommyEimvur
        }

        public AttackState currentState = AttackState.Idle;
        public Vector2 PlayerCenter = Vector2.Zero;
        public Vector2 DirectionToPlayerCenter = Vector2.Zero;
        public int BloodShotAmount = 4;
        public int BloodShotTimer = 0;
        public int BloodShotCount = 0;
        public int BatTimer = 0;
        public int BatCount = 0;

        public override void OnSpawn(IEntitySource source)
        {
            currentState = AttackState.Idle;
        }


        public override void AI()
        {

            Player player = Main.LocalPlayer;

            DirectionToPlayerCenter = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero);

           

            PlayerCenter = player.Center;




            // Removed unused 'effects' variable.

            if (!Main.dedServ) {
				Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/OrdealBoss");
			}



            /*
            if (NPC.Center.DistanceSQ(player.Center) > 40000)
            {
                TeleManager(player, ref TeleCircumferencePoint);
            }
            */






            Mod.Logger.Info($"Current State: {currentState}");

            switch (currentState)
            {
                case AttackState.Idle:
                    if (NPC.type == ModContent.NPCType<VampireBoss>())
                    {
                        NPC.aiStyle = 10;
                        

                        if (Main.rand.NextBool(3))
                        {
                            currentState = GetRandomState();
                        }
                    }
                    break;
                case AttackState.ShootBlood:
                    if (NPC.type == ModContent.NPCType<VampireBoss>())
                    {
                        NPC.aiStyle = 10;
                        BloodShotTimer++;

                        if (BloodShotCount < 6 && BloodShotTimer == 60)
                        {
                            ShootBlood(NPC, player);
                            BloodShotTimer = 0;
                            BloodShotCount += 1;
                        }

                        if (BloodShotCount >= 6)
                        {
                            currentState = AttackState.Idle;
                        }
                    }
                    break;
                case AttackState.BrethrenBats:
                    if (NPC.type == ModContent.NPCType<VampireBoss>())
                    {
                        NPC.aiStyle = 10;
                        

                        BatTimer++;
                        if (BatTimer == 60 && BatCount < 4)
                        {
                            BreathrenBatSpawn(NPC, player);
                            BatCount += 1;
                            BatTimer = 0;
                        }

                        if (BatCount >= 4)
                        {
                            currentState = AttackState.Idle;
                        }
                    }
                    break;
                case AttackState.GoldenShots:
                    if (NPC.type == ModContent.NPCType<VampireBoss>())
                    {
                        // This will be worke out with newer sprites that allow the NPC to hold a gun.
                        currentState = AttackState.Idle;
                    }
                    break;
                case AttackState.ChargeNoBite:
                    if (NPC.type == ModContent.NPCType<VampireBoss>())
                    {
                        NPC.aiStyle = 10;
                        
                    }
                    break;
                case AttackState.ChargeAndBite:
                    if (NPC.type == ModContent.NPCType<VampireBoss>())
                    {
                        NPC.aiStyle = 10;
                       
                    }
                    break;

            }
        }

        private Dictionary<AttackState, float> stateWeights = new()
        {
            { AttackState.Idle, 1.0f },
            { AttackState.ShootBlood, 1.0f },
            { AttackState.BrethrenBats, 1.0f },
            { AttackState.GoldenShots, 1.0f },
            { AttackState.ChargeNoBite, 1.0f },
            { AttackState.ChargeAndBite, 1.0f },
            { AttackState.DarkHands, 1.0f },
            { AttackState.GetHelpFromMommyEimvur, 1.0f }
            };


        private AttackState GetRandomState()
        {

            // Exclude the current state
            var validStates = stateWeights
                .Where(pair => pair.Key != currentState && pair.Value > 0)
                .ToList();

            float totalWeight = validStates.Sum(pair => pair.Value);
            float roll = Main.rand.NextFloat() * totalWeight;

            float cumulative = 0f;
            foreach (var pair in validStates)
            {
                cumulative += pair.Value;
                if (roll <= cumulative)
                    return pair.Key;
            }

            // Fallback (should never happen unless all weights are 0)
            return currentState;
        }


        private void ResetState()
        {
            if (NPC.type == ModContent.NPCType<VampireBoss>())
            {
                currentState = GetRandomState();
                NPC.ai[0] = NPC.ai[1] = NPC.ai[2] = NPC.ai[3] = 0;
            }
        }

        public void ShootBlood(NPC npc, Player player)
        {
            if (npc.type == ModContent.NPCType<VampireBoss>())
            {
                for (int i = 0; i < BloodShotAmount; i++)
                {
                    Projectile Blood = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), npc.Center, DirectionToPlayerCenter * 28f, ModContent.ProjectileType<BloodProjectile>(), 4, 1);
                }
            }
        }

        public void BreathrenBatSpawn(NPC npc, Player player)
        {
            if (npc.type == ModContent.NPCType<VampireBoss>())
            {
                Vector2 PlayerLeft = player.Center + new Vector2(-50, 0);
                Vector2 PlayerRight = player.Center + new Vector2(50, 0);

                Dust.NewDust(PlayerLeft, 5, 5, DustID.Blood, 0f, 0f, 0, default, 1.2f);
                Dust.NewDust(PlayerRight, 5, 5, DustID.Blood, 0f, 0f, 0, default, 1.2f);

                Projectile BatLeft = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), PlayerLeft, DirectionToPlayerCenter, ProjectileID.BatOfLight, 8, 1);
                Projectile BatRight = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), PlayerLeft, DirectionToPlayerCenter, ProjectileID.BatOfLight, 8, 1);

            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (currentState == AttackState.ChargeAndBite)
            {
            target.AddBuff(BuffID.Bleeding, 480);
            }
        }

    }
}
