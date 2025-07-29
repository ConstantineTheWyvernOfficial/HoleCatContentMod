using DestroyerTest.Common;
using DestroyerTest.Content.Projectiles.ConstitutionBoss;
using DestroyerTest.Content.RiftBiome;
using DestroyerTest.Content.Buffs;
using log4net.Repository.Hierarchy;
using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
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
using DestroyerTest.Content.Projectiles.NightmareRose;
using Terraria.Graphics;
using DestroyerTest.Content.Projectiles;
using rail;
using System.Security.Policy;
using DestroyerTest.Common.Systems;
using Terraria.Localization;
using InnoVault.PRT;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Magic;
using DestroyerTest.Content.RogueItems;
using DestroyerTest.Content.Equips;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Content.SummonItems;

namespace DestroyerTest.Content.Entity
{
    [AutoloadBossHead]
    public class NightmareRoseBoss : ModNPC
    {
        public override string BossHeadTexture => "DestroyerTest/Content/Entity/NightmareRoseBoss_Head_Boss";
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
                CustomTexturePath = "DestroyerTest/Content/Entity/NightmareRoseBossBestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = Vector2.Zero,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            NPC.width = 144;
            NPC.height = 274;
            NPC.aiStyle = -1;
            NPC.damage = 70;
            NPC.defense = 17;
            NPC.lifeMax = 300000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = false;
            NPC.lavaImmune = true;
            NPC.noTileCollide = false;
            NPC.knockBackResist = 0f;
            NPC.timeLeft = 150000;
            NPC.boss = true;
            NPC.npcSlots = 90f;
            NPC.netUpdate = true;
            NPC.netID = ModContent.NPCType<NightmareRoseBoss>();


        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("An unholy amalgamation of Creatures, disguised poorly as a rose. Little is known of it other than that it seens to pop up randomly."),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
            });
        }

        public override bool CheckActive()
        {
            return false;
        }

        public static bool EternityIsActive()
        {
            if (ModLoader.TryGetMod("FranciumMultiCrossMod", out Mod MCM))
            {
                object result = MCM.Call("IsEternityMode");
                if (result is bool b)
                    return b;
            }
            return false;
        }

        #region Difficulty Attack Pools
        /// <summary>
        /// Attacks that are available on all difficulties. Will be selected from for classic mode AI.
        /// </summary>
        public enum BaseAttacks
        {

        }

        /// <summary>
        /// Attacks that are Exclusively available on Expert Mode and above. Will be selected from for Expert and Master AI.
        /// </summary>
        public enum ExpertAttacks
        {

        }

        /// <summary>
        /// Attacks that are Exclusively available on Master Mode. Will be selected from for Master AI.
        /// </summary>
        public enum MasterAttacks
        {

        }

        /// <summary>
        /// Attacks that are Exclusively available on Revengeance Mode. Will be selected from for Revengeance and Death AI.
        /// </summary>
        public enum RevenganceAttacks
        {

        }
        
        /// <summary>
        /// Attacks that are Exclusively available on Death Mode. Will be selected from for Death AI.
        /// </summary>
        public enum DeathAttacks
        {

        }

        /// <summary>
        /// Attacks that are Exclusively available on Eternity Mode. Will be selected from for Eternity and Masochist AI.
        /// </summary>
        public enum EternityAttacks
        {

        }

        /// <summary>
        /// Attacks that are Exclusively available on Masochist Mode. Will be selected from for Masochist AI.
        /// </summary>
        public enum MasochistAttacks
        {

        }
        #endregion

        public enum AttackState
        {
            Idle,
            CursedFlames,
            VilethornFloor,
            Minions,
            RottenPetals,
            OvergrownHammer,
            DemoniteWhisper,
            CorruptSigil,
            BlossomMine,
            Desperation,
            Nodes
        }

        #region Vars

        public AttackState currentState = AttackState.Idle;
        public Vector2 PlayerCenter = Vector2.Zero;
        public Vector2 DirectionToPlayerCenter = Vector2.Zero;
        public Vector2 NPCHead;
        public int BorderRad = 1200;
        public bool BorderActive = false;
        public int FlameTimer = 0;
        public int FlameInterval = 0;
        public int FlameStartTimer = 60;
        public int VileThornCooldown = 0;
        public int VileThornCount = 0;
        public int MinionSpawnTimer = 0;
        public int MinionSpawnCount = 0;
        /// <summary>
        /// hammeractive is not affected by state resets.
        /// </summary>
        public bool HammerActive = false;
        public int MinionFailsafe = 0;
        public bool HasBoosted = false;
        public int SigilTimer = 600;
        public int SoulInterval = 0;
        public int SoulSpawnCount = 0;
        public bool HasSpawnedSigil = false;
        public bool HasSpawnedMines = false;
        public int ProjSpawnTimer = 0;
        public int DesperationTimer = 0;
        public bool HasTriggeredNodes = false;
        public bool anyNodesAlive;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((int)currentState);
            writer.WriteVector2(PlayerCenter);
            writer.WriteVector2(DirectionToPlayerCenter);
            writer.WriteVector2(NPCHead);
            writer.Write(BorderRad);
            writer.Write(BorderActive);
            writer.Write(FlameTimer);
            writer.Write(FlameInterval);
            writer.Write(VileThornCooldown);
            writer.Write(VileThornCount);
            writer.Write(MinionSpawnTimer);
            writer.Write(MinionSpawnCount);
            writer.Write(HammerActive);
            writer.Write(MinionFailsafe);
            writer.Write(HasBoosted);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            currentState = (AttackState)reader.ReadInt32();
            PlayerCenter = reader.ReadVector2();
            DirectionToPlayerCenter = reader.ReadVector2();
            NPCHead = reader.ReadVector2();
            BorderRad = reader.ReadInt32();
            BorderActive = reader.ReadBoolean();
            FlameTimer = reader.ReadInt32();
            FlameInterval = reader.ReadInt32();
            VileThornCooldown = reader.ReadInt32();
            VileThornCount = reader.ReadInt32();
            MinionSpawnTimer = reader.ReadInt32();
            MinionSpawnCount = reader.ReadInt32();
            HammerActive = reader.ReadBoolean();
            MinionFailsafe = reader.ReadInt32();
            HasBoosted = reader.ReadBoolean();
        }
        
        #endregion

        public override void OnSpawn(IEntitySource source)
        {
            BorderActive = true;
            currentState = AttackState.Idle;
            NPCHead = NPC.Center + new Vector2(0, -60);
            int[] types = new int[]
                {
                PRTLoader.GetParticleID<BlackFire1>(),
                PRTLoader.GetParticleID<BlackFire2>(),
                PRTLoader.GetParticleID<BlackFire3>(),
                PRTLoader.GetParticleID<BlackFire4>(),
                PRTLoader.GetParticleID<BlackFire5>(),
                PRTLoader.GetParticleID<BlackFire6>(),
                PRTLoader.GetParticleID<BlackFire7>()
                };

            // Spawn a particle that travels outward at a random angle from the boss center
            for (int b = 0; b < 70; b++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(2f, 6f);

                PRTLoader.NewParticle(
                    types[Main.rand.Next(types.Length)],
                    Main.rand.NextVector2FromRectangle(NPC.getRect()),
                    velocity,
                    default,
                    1.0f
                );
            }
        }


        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (currentState == AttackState.Desperation || anyNodesAlive)
                return false;

            return base.CanBeHitByProjectile(projectile);
        }

            public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
            {
                if (currentState == AttackState.Desperation || anyNodesAlive)
                {
                    NPC.immortal = true;
                    modifiers.FinalDamage *= 0f;
                }
            }
            
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (currentState == AttackState.Desperation || anyNodesAlive)
                return false;

            return base.CanBeHitByItem(player, item);
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (currentState == AttackState.Desperation || anyNodesAlive)
            {
                NPC.immortal = true;
                modifiers.FinalDamage *= 0f;
            }
        }

        public int DeathInterval = 10;
        public override void AI()
        {
            MusicCreditSystem.ShowCredit = true;
            MusicCreditSystem.CreditText = "Track: 'Running From Demons' By Waterflame | https://www.youtube.com/channel/UCVuv5iaVR55QXIc_BHQLakA";
            Player player = Main.LocalPlayer;

            DirectionToPlayerCenter = (player.Center - NPCHead).SafeNormalize(Vector2.UnitY);

            BorderActive = true; // This happens **after** the loop finishes

            if (BorderActive)
            {
                int DustAmount = 90;

                for (int i = 0; i < DustAmount; i++)
                {
                    float angle = MathHelper.TwoPi * i / DustAmount;
                    Vector2 Pos = NPCHead + BorderRad * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    Dust Border = Dust.NewDustPerfect(Pos, DustID.CursedTorch, Vector2.Zero, 0, default, 1f);
                    Border.noGravity = true;
                    Border.fadeIn = 1f;
                    Border.scale = 3.5f;
                }
            }

            if (player.Distance(NPC.Center) >= BorderRad && BorderActive)
            {
                player.Hurt(PlayerDeathReason.LegacyDefault(), 90, 0, false, true, -1, false, 9, 9, 0);
            }

            if (NPC.life <= NPC.lifeMax * 0.05f)
            {
                // Make the NPC invincible to all damage not self-inflicted

                currentState = AttackState.Desperation;
            }

            if (player.dead)
            {
                DeathInterval--;
                if (DeathInterval <= 0)
                {
                    NPC.active = false;
                }
            }

            // Assuming this is inside your boss NPC code
            anyNodesAlive = Main.npc.Any(n => n.active && n.type == ModContent.NPCType<CursedFlameNode>());

            if (anyNodesAlive)
            {
                NPC.dontTakeDamage = true;
                NPC.immortal = true;
                NPC.life += 20;
            }
            else
            {
                NPC.immortal = false;
                NPC.dontTakeDamage = false;
            }





            if (NPC.life >= NPC.lifeMax * 0.24f && NPC.life <= NPC.lifeMax * 0.25f)
            {
                if (HasTriggeredNodes == false)
                {
                    currentState = AttackState.Nodes;
                    HasTriggeredNodes = true;
                }
            }

            if (player.active == false || player.dead == true)
            {
                OnKill();
            }












            PlayerCenter = player.Center;




            // Removed unused 'effects' variable.

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/RunningFromDemons");
            }



            /*
            if (NPC.Center.DistanceSQ(player.Center) > 40000)
            {
                TeleManager(player, ref TeleCircumferencePoint);
            }
            */

            NPC.velocity = Vector2.Zero;

            int MinionSpawnType = Main.rand.Next(new int[]
                            {
                                NPCID.DevourerHead,
                                NPCID.SeekerHead,
                                NPCID.Clinger,
                                NPCID.Slimer,
                                NPCID.Corruptor
                            });





            Mod.Logger.Info($"Current State: {currentState}");

            switch (currentState)
            {
                case AttackState.Idle:
                    if (NPC.type == ModContent.NPCType<NightmareRoseBoss>())
                    {
                        NPC.aiStyle = -1;

                        if (Main.rand.NextBool(3))
                        {
                            currentState = GetRandomState();
                        }
                    }
                    break;
                case AttackState.Nodes:
                    if (NPC.type == ModContent.NPCType<NightmareRoseBoss>())
                    {
                        NPC.aiStyle = -1;
                        NodeSpawn();
                        currentState = GetRandomState();
                    }
                    break;
                case AttackState.CursedFlames:
                    {
                        if (FlameStartTimer >= 60)
                        {
                            SoundEngine.PlaySound(new SoundStyle("DestroyerTest/Assets/Audio/NightmareRose/CursedFlamesWarn") with { Volume = 1.5f });
                        }
                        FlameStartTimer--;
                        if (HasBoosted == false)
                        {
                            player.velocity += new Vector2(0, -15);
                            HasBoosted = true;
                        }




                        if (FlameTimer < 240 && FlameStartTimer <= 0)
                        {
                            FlameTimer++;
                            FlameInterval++;
                            Vector2 velocity = DirectionToPlayerCenter.SafeNormalize(Vector2.UnitY);
                            if (FlameInterval >= 10)
                            {
                                Projectile.NewProjectile(Entity.GetSource_FromThis(), NPCHead, velocity * 20f, ModContent.ProjectileType<CursedFlameProj>(), 50, 0);
                                FlameInterval = 0;
                            }
                        }
                        if (FlameTimer >= 240)
                        {
                            ResetState();
                        }
                    }
                    break;
                case AttackState.VilethornFloor:
                    {
                        player.wingTime = 0;
                        VileThornCooldown++;
                        if (VileThornCooldown > 30)
                        {
                            VileThornCount += 1;

                            for (int e = 0; e < 6; e++)
                            {
                                Projectile proj = Projectile.NewProjectileDirect(
                                        Entity.GetSource_FromThis(),
                                        NPCHead,
                                        new Vector2(Main.rand.NextFloat(-2, 2), -15),
                                        ProjectileID.CursedFlameFriendly,
                                        10,
                                        2
                                    );
                                proj.tileCollide = true;
                                proj.hostile = true;
                                proj.friendly = false;
                                proj.timeLeft = 480;
                            }

                            // Reset cooldown and count if needed
                            VileThornCooldown = 0;
                            if (VileThornCount >= 8)
                            {
                                ResetState();
                            }
                        }
                    }
                    break;

                case AttackState.Minions:
                    {
                        MinionFailsafe++;
                        MinionSpawnTimer++;

                        if (MinionSpawnTimer == 10)
                        {
                            NPC Minion = NPC.NewNPCDirect(Entity.GetSource_FromThis(), NPCHead, MinionSpawnType);
                            Minion.damage = 30;
                            Minion.lifeMax = 400;
                            Minion.life = 400;
                            Minion.noGravity = true;

                            MinionSpawnCount += 1;
                            MinionSpawnTimer = 0;
                        }
                        if (MinionSpawnCount >= 6)
                        {
                            currentState = AttackState.Idle;
                            ResetState();
                        }
                        if (MinionFailsafe >= 1200)
                        {
                            currentState = AttackState.Idle;
                            ResetState();
                        }
                    }
                    break;
                case AttackState.RottenPetals:
                    {
                        // Will sprite later.
                        ResetState();
                    }
                    break;


                case AttackState.OvergrownHammer:
                    {
                        // Will sprite a custom giant hammer. for now, sizing up the cursed hammer will work just fine.
                        if (NPC.life < NPC.lifeMax * 0.4f && HammerActive == false)
                        {
                            NPC MinionHammer = NPC.NewNPCDirect(Entity.GetSource_FromThis(), NPC.Center, ModContent.NPCType<GigaCursedHammer>());
                            MinionHammer.UpdateNPC(3);
                            HammerActive = true;
                        }
                        else
                        {
                            ResetState();
                        }
                    }
                    break;
                case AttackState.DemoniteWhisper:
                    {
                        SoulInterval++;
                        if (SoulInterval >= 120)
                        {
                            SummonSouls();
                            SoulSpawnCount++;
                            SoulInterval = 0;
                        }
                        if (SoulSpawnCount >= 4)
                        {
                            ResetState();
                        }
                    }
                    break;

                case AttackState.CorruptSigil:
                    {
                        SigilTimer--;
                        if (HasSpawnedSigil == false)
                        {
                            ManageSigil(Main.rand.NextVector2FromRectangle(new Rectangle(0, 0, BorderRad, BorderRad)));
                            HasSpawnedSigil = true;
                        }
                        if (SigilTimer <= 0)
                        {
                            ResetState();
                        }
                    }
                    break;
                case AttackState.BlossomMine:
                    {
                        if (HasSpawnedMines == false)
                        {
                            BlossomMines(Main.rand.NextVector2FromRectangle(new Rectangle(0, 0, BorderRad, BorderRad)));
                            HasSpawnedMines = true;
                        }
                        if (HasSpawnedMines == true)
                        {
                            ResetState();
                        }
                    }
                    break;
                case AttackState.Desperation:
                    {
                        ProjSpawnTimer++;
                        DesperationTimer++;
                        ContemptAttack();

                        if (DesperationTimer >= 600)
                        {
                            NPC.immortal = false;
                            NPC.StrikeInstantKill();
                        }
                    }
                    break;


            }
        }

        /// <summary>
        /// AI that runs on Classic Mode. Vanilla.
        /// </summary>
        public void ClassicAI()
        {

        }

        /// <summary>
        /// AI that runs on Expert Mode. Vanilla.
        /// </summary>
        public void ExpertAI()
        {

        }

        /// <summary>
        /// AI that runs on Master Mode. Vanilla.
        /// </summary>
        public void MasterAI()
        {

        }

        /// <summary>
        /// AI that runs on Revengeance Mode. Calamity.
        /// </summary>
        public void RevenganceAI()
        {

        }

        /// <summary>
        /// AI that runs on Death Mode. Calamity.
        /// </summary>
        public void DeathAI()
        {

        }

        /// <summary>
        /// AI that runs on Eternity Mode. Fargo's Souls.
        /// </summary>
        public void EternityAI()
        {

        }

        /// <summary>
        /// AI that runs on Masochist Mode. Fargo's Souls.
        /// </summary>
        public void MasochistAI()
        {

        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(spriteBatch, screenPos, drawColor);
            if (FlameTimer < 240 && FlameTimer >= 0 && currentState == AttackState.CursedFlames)
            {
                DrawTelegraph(NPCHead, PlayerCenter, ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/CursedFlamesTelegraph").Value);
            }

        }


        private Dictionary<AttackState, float> stateWeights = new()
        {
            { AttackState.Idle, 1.0f },
            { AttackState.CursedFlames, 1.0f },
            { AttackState.DemoniteWhisper, 1.0f },
            { AttackState.Minions, 1.0f },
            { AttackState.OvergrownHammer, 1.0f },
            { AttackState.RottenPetals, 1.0f },
            { AttackState.VilethornFloor, 1.0f },
            { AttackState.CorruptSigil, 1.0f },
            { AttackState.BlossomMine, 1.0f },
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
            if (NPC.type == ModContent.NPCType<NightmareRoseBoss>())
            {
                currentState = GetRandomState();
                NPC.ai[0] = NPC.ai[1] = NPC.ai[2] = NPC.ai[3] = 0;

                // Reset public bools and integers (except HammerActive)
                HasSpawnedMines = false;
                HasSpawnedSigil = false;
                HasBoosted = false;
                FlameTimer = 0;
                FlameStartTimer = 60;

                VileThornCooldown = 0;
                VileThornCount = 0;
                MinionSpawnTimer = 0;
                MinionSpawnCount = 0;
                SigilTimer = 600;
                SoulInterval = 0;
                SoulSpawnCount = 0;
            }
        }

        public void DrawTelegraph(Vector2 start, Vector2 end, Texture2D texture)
        {
            Vector2 direction = end - start;
            float length = direction.Length();
            direction.Normalize();
            texture ??= ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/CursedFlamesTelegraph").Value;
            SpriteBatch spriteBatch = Main.spriteBatch;

            float rotation = direction.ToRotation();

            // Assuming your texture is a chain segment, like 16px long
            float segmentLength = texture.Height * 0.75f; // or Width, depending on the texture orientation
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            for (float i = 0; i < length; i += segmentLength)
            {
                Vector2 position = start + direction * i;

                Main.spriteBatch.Draw(
                    texture,
                    position - Main.screenPosition,
                    null,
                    new Color(179, 252, 0) * 0.2f,
                    rotation + MathHelper.PiOver2, // Adjust if your texture points upward
                    new Vector2(texture.Width / 2f, texture.Height / 2f), // Origin at center
                    1f, // Scale
                    SpriteEffects.None,
                    0f
                );
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public void NodeSpawn()
        {
            float radius = 200;
            int projectileCount = 6;

            
                SoundEngine.PlaySound(SoundID.Item20);

                for (int i = 0; i < projectileCount; i++)
                {
                    // Get evenly spaced angle with rotation offset
                    float angle = MathHelper.TwoPi * i / projectileCount;
                    Vector2 spawnOffset = radius * angle.ToRotationVector2(); // position on the circle
                    Vector2 spawnPosition = NPC.Center + spawnOffset;

                    NPC.NewNPC(Entity.GetSource_FromThis(), (int)spawnPosition.X, (int)spawnPosition.Y, ModContent.NPCType<CursedFlameNode>());

                PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp1>(), NPC.Center, Vector2.Zero, ColorLib.CursedFlames, 0.4f);

            }
        }

        public void ManageSigil(Vector2 SpawnPos)
        {
            Projectile.NewProjectile(
                Entity.GetSource_FromThis(),
                SpawnPos,
                DirectionToPlayerCenter * 2f,
                ModContent.ProjectileType<CorruptSigil>(),
                60,
                2f
            );

        }

        public void SummonSouls()
        {
            Player player = Main.LocalPlayer;
            SoundEngine.PlaySound(new SoundStyle("DestroyerTest/Assets/Audio/NightmareRose/SoulSummon"));
            player.GetModPlayer<ScreenshakePlayer>().screenshakeTimer = 10;
            player.GetModPlayer<ScreenshakePlayer>().screenshakeMagnitude = 1;
            for (int a = 0; a < 10; a++)
            {
                Vector2 SpawnPoint = new Vector2(NPC.Center.X + Main.rand.Next(-BorderRad, BorderRad), NPC.Center.Y + 600);
                Projectile.NewProjectile(Entity.GetSource_FromThis(), SpawnPoint, new Vector2(0, -8), ModContent.ProjectileType<TormentedSoul>(), 20, 2);
            }
        }

        public void BlossomMines(Vector2 SpawnPos)
        {
            for (int e = 0; e < 6; e++)
            {
                Vector2 minePosition = Main.rand.NextVector2FromRectangle(
                new Rectangle(
                    (int)Main.LocalPlayer.Center.X - Main.screenWidth / 2,
                    (int)Main.LocalPlayer.Center.Y - Main.screenHeight / 2,
                    Main.screenWidth,
                    Main.screenHeight
                    )
                );

                Projectile.NewProjectile(
                    Entity.GetSource_FromThis(),
                    minePosition,
                    Vector2.Zero,
                    ModContent.ProjectileType<BlossomMine>(),
                    10,
                    0f
                );
            }
        }

        public void ContemptAttack()
        {
            float radius = BorderRad;
            int projectileCount = 6;
            float rotationOffset = (float)(Main.GameUpdateCount % 360) * MathHelper.ToRadians(0.5f);
            // ^^^ rotates the whole formation over time. You can tweak the 1.5f for rotation speed.

            if (ProjSpawnTimer >= 10)
            {
                SoundEngine.PlaySound(SoundID.Item20);

                for (int i = 0; i < projectileCount; i++)
                {
                    // Get evenly spaced angle with rotation offset
                    float angle = MathHelper.TwoPi * i / projectileCount + rotationOffset;
                    Vector2 spawnOffset = radius * angle.ToRotationVector2(); // position on the circle
                    Vector2 spawnPosition = NPC.Center + spawnOffset;

                    Vector2 toOrigin = NPC.Center - spawnPosition;
                    toOrigin = toOrigin.SafeNormalize(Vector2.UnitY);

                    Projectile flame = Projectile.NewProjectileDirect(
                        Entity.GetSource_FromThis(),
                        spawnPosition,
                        toOrigin * 20f,
                        ModContent.ProjectileType<CursedFlameProj>(),
                        40,
                        2,
                        Main.LocalPlayer.whoAmI
                    );
                    flame.timeLeft = 70;
                    flame.tileCollide = true;

                    if (flame.Center == NPC.Center)
                        flame.Kill();
                }

                PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp1>(), NPC.Center, Vector2.Zero, ColorLib.CursedFlames, 0.4f);
                ProjSpawnTimer = 0;
            }
        }


        public override void OnKill()
        {
            MusicCreditSystem.ShowCredit = false;
            DownedBossSystem.downedNightmareRoseBoss = true;
            int[] types = new int[]
                {
                PRTLoader.GetParticleID<BlackFire1>(),
                PRTLoader.GetParticleID<BlackFire2>(),
                PRTLoader.GetParticleID<BlackFire3>(),
                PRTLoader.GetParticleID<BlackFire4>(),
                PRTLoader.GetParticleID<BlackFire5>(),
                PRTLoader.GetParticleID<BlackFire6>(),
                PRTLoader.GetParticleID<BlackFire7>()
                };

            // Spawn a particle that travels outward at a random angle from the boss center
            for (int b = 0; b < 70; b++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(2f, 6f);
                
                PRTLoader.NewParticle(
                    types[Main.rand.Next(types.Length)],
                    Main.rand.NextVector2FromRectangle(NPC.getRect()),
                    velocity,
                    default,
                    1.0f
                );
            }
        }

    }

    public class NightmareRoseBCL : ModSystem
    {
        public override void PostSetupContent()
        {
            // Most often, mods require you to use the PostSetupContent hook to call their methods. This guarantees various data is initialized and set up properly

            // Boss Checklist shows comprehensive information about bosses in its own UI. We can customize it:
            // https://forums.terraria.org/index.php?threads/.50668/
            DoBossChecklistIntegration();

            // We can integrate with other mods here by following the same pattern. Some modders may prefer a ModSystem for each mod they integrate with, or some other design.
        }

        private void DoBossChecklistIntegration()
        {
            // The mods homepage links to its own wiki where the calls are explained: https://github.com/JavidPack/BossChecklist/wiki/%5B1.4.4%5D-Boss-Log-Entry-Mod-Call
            // If we navigate the wiki, we can find the "LogBoss" method, which we want in this case
            // A feature of the call is that it will create an entry in the localization file of the specified NPC type for its spawn info, so make sure to visit the localization file after your mod runs once to edit it

            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
            {
                return;
            }

            // For some messages, mods might not have them at release, so we need to verify when the last iteration of the method variation was first added to the mod, in this case 1.6
            // Usually mods either provide that information themselves in some way, or it's found on the GitHub through commit history/blame
            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            // The "LogBoss" method requires many parameters, defined separately below:

            // Your entry key can be used by other developers to submit mod-collaborative data to your entry. It should not be changed once defined
            string internalName = "NightmareRose";

            // Value inferred from boss progression, see the wiki for details
            float weight = 18.35f;

            // Used for tracking checklist progress
            Func<bool> downed = () => DownedBossSystem.downedNightmareRoseBoss;

            LocalizedText Hint = Language.GetText("Mods.DestroyerTest.BossChecklist.NightmareRose.Hint");

            LocalizedText Despawn = Language.GetText("Mods.DestroyerTest.NPCs.NightmareRose.DespawnMessage");

            // The NPC type of the boss
            int bossType = ModContent.NPCType<NightmareRoseBoss>();

            // The item used to summon the boss with (if available)
            int spawnItem = ModContent.ItemType<TheBotanistsCurse>();

            // "collectibles" like relic, trophy, mask, pet
            List<int> collectibles = new List<int>()
            {
                ModContent.ItemType<Contempt>(),
                ModContent.ItemType<GigaCursedHammerWeapon>(),
                ModContent.ItemType<PossessedDartRifleItem>(),
                ModContent.ItemType<DeadlyBlossom>(),
                ItemID.CursedFlame,
                ModContent.ItemType<Item_NightmareRoseRelic>(),
                ModContent.ItemType<Item_NightmareRoseTrophy>()
            };

            // By default, it draws the first frame of the boss, omit if you don't need custom drawing
            // But we want to draw the bestiary texture instead, so we create the code for that to draw centered on the intended location
            var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                Texture2D texture = ModContent.Request<Texture2D>("DestroyerTest/Content/Extras/NightmareRoseBossBossChecklist").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                sb.Draw(texture, centered, color);
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["spawnItems"] = spawnItem,
                    ["collectibles"] = collectibles,
                    ["customPortrait"] = customPortrait,
                    ["spawnInfo"] = Hint,
                    ["despawnMessage"] = Despawn,

                    // Other optional arguments as needed are inferred from the wiki
                }
            );


            // Other bosses or additional Mod.Call can be made here.
        }
    }

    [AutoloadBossHead]
    public class CursedFlameNode : ModNPC
    {
        public override string BossHeadTexture => "DestroyerTest/Content/Entity/CursedFlameNode_Head_Boss";
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CanHitPastShimmer[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
            NPCID.Sets.TrailCacheLength[Type] = 20;
            NPCID.Sets.TrailingMode[Type] = 3;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "DestroyerTest/Content/Entity/NodesBestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = Vector2.Zero,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 100;
            NPC.aiStyle = -1;
            NPC.damage = 25;
            NPC.defense = 16;
            NPC.lifeMax = 10000;
            NPC.HitSound = new SoundStyle("DestroyerTest/Assets/Audio/NodeHit");
            NPC.DeathSound = new SoundStyle("DestroyerTest/Assets/Audio/NodeExplode");
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.timeLeft = 150000;
            NPC.boss = true;
            NPC.npcSlots = 12f;
            NPC.netUpdate = true;
            NPC.netID = ModContent.NPCType<CursedFlameNode>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("Elemental Constructs that strengthen the potency of Cursed Flames and Ichor."),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
            });
        }

        public override bool CheckActive()
        {
            return false;
        }
        
        

        public override void AI()
        {


            NPC bossNPC = null;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n.active && n.type == ModContent.NPCType<NightmareRoseBoss>())
                {
                    bossNPC = n;
                    break;
                }
            }

            if (bossNPC == null)
            {
                NPC.active = false;
                return;
            }
            else
            {
                NPC.active = true;
            }

            // Access ModNPC safely
            NightmareRoseBoss modBoss = bossNPC.ModNPC as NightmareRoseBoss;

            // If NPCHead is a custom property
            Vector2 OrbitCenter = modBoss != null ? modBoss.NPCHead : bossNPC.Center;

            bool ParentAlive = Main.npc.Any(n => n.active && n.type == ModContent.NPCType<NightmareRoseBoss>());

            if (ParentAlive)
            {
                NPC.active = true;
            }
            else
            {
                NPC.active = false;
            }

            // Orbit settings
            float radius = 600f;
            float speed = 0.05f;
            float angle = Main.GameUpdateCount * speed;

            // Get a list of all active CursedFlameNode NPCs
            List<NPC> allNodes = new List<NPC>();

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC node = Main.npc[i];
                if (node.active && node.type == ModContent.NPCType<CursedFlameNode>())
                {
                    allNodes.Add(node);
                }
            }

            // Sort the list by whoAmI to ensure consistent order across clients and frames
            allNodes.Sort((a, b) => a.whoAmI.CompareTo(b.whoAmI));

            int index = allNodes.IndexOf(NPC);
            int total = allNodes.Count;

            // Calculate spacing
            float spacing = MathHelper.TwoPi / (total == 0 ? 1 : total);
            float myAngle = angle + index * spacing;

            // Final orbit
            Vector2 offset = new Vector2(MathF.Cos(myAngle), MathF.Sin(myAngle)) * radius;
            NPC.Center = OrbitCenter + offset - new Vector2(NPC.width / 2, NPC.height / 2);

        }
    }
}
