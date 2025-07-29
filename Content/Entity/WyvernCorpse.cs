
using DestroyerTest.Common;
using DestroyerTest.Common.Systems;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Content.Equips;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Projectiles.CorpseBoss;
using DestroyerTest.Content.Projectiles.CorpseBoss.Organs;
using DestroyerTest.Content.Projectiles.VampireBoss;
using DestroyerTest.Content.SummonItems;
using DestroyerTest.Content.Tiles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Animations;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DestroyerTest.Content.Entity
{
    /// <summary>
    /// This is the code from Consolaria's Arch Wyvern. I do not own any of this except for the textures I paint over it. This code will be replaced in the future, when I am capable of modding something so advanced. (Trust me. I tried many times with the example worm. It did not go well.)
    /// </summary>
    [AutoloadBossHead]
    public class WyvernCorpseHead : ModNPC
    {
        public enum attackType
        {
            Follow,
            Dash,
            IchorRam,
            Circle,
            OrganBurst,
            SummonCrimsonMinions,
            SummonAxes,
            BloodShoot,
            FleshBombShoot,
            Clouds,
            Nodes,
            Desperation
        }

        public SoundStyle Roar = new SoundStyle("DestroyerTest/Assets/Audio/Corpse/CorpseRoar1") with { PitchVariance = 1.0f };
        public static LocalizedText BestiaryText
        {
            get; private set;
        }



        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "DestroyerTest/Content/Entity/WyvernCorpseBestiary",

                PortraitPositionXOverride = -25f,
                PortraitPositionYOverride = 0f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.ShadowFlame] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;

            BestiaryText = this.GetLocalization("Bestiary");
        }

        public override void SetDefaults()
        {
            NPC.width = 100;
            NPC.height = 36;

            NPC.aiStyle = NPCAIStyleID.Worm;

            NPC.damage = 100;
            NPC.defense = 40;
            NPC.lifeMax = 300000;

            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit8;
            NPC.DeathSound = SoundID.NPCDeath8;
            NPC.boss = true;

            NPC.knockBackResist = 0.0f;
            NPC.rarity = 2;
            NPC.npcSlots = 20f;

            NPC.netAlways = true;
            NPC.netUpdate = true;
            NPC.netID = ModContent.NPCType<WyvernCorpseHead>();

            NPC.alpha = 255;
            NPC.value = Item.buyPrice(gold: 1, silver: 75);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement(BestiaryText.ToString())
            });
        }
        public override bool CheckActive()
        {
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.1f;
            return new bool?();
        }

        // Multiplayer-synced fields
        public attackType CurrentAttack = attackType.Follow;

        public int FollowTime = 0;
        public bool IsDashing = false;
        public int DashCount = 0;
        public int DashTime = 40;
        public int SpitTime = 0;
        public float circleradius = 800f;
        public float circlerotspeed = 0.05f;
        public int CircleTime = 0;
        public int OrganBurstIntervalTimer = 0;
        public int OrganBurstCount = 0;
        public bool HasSummoned40PercentMinions = false;
        public int BloodTimer = 0;
        public int BloodInterval = 0;
        public int MinionSpawnTimer = 0;
        public int MinionSpawnCount = 0;
        public int MinionSpawnType = 0;
        public int BombSpawnTimer = 0;
        public int BombSpawnCount = 0;
        public bool HasTriggeredNodes = false;
        public Vector2 DesperationOrbitCenter;
        public int DesperationTimer = 0;
        public bool anyNodesAlive;

        // Write extra AI fields for multiplayer sync
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((int)CurrentAttack);
            writer.Write(FollowTime);
            writer.Write(IsDashing);
            writer.Write(DashCount);
            writer.Write(DashTime);
            writer.Write(SpitTime);
            writer.Write(circleradius);
            writer.Write(circlerotspeed);
            writer.Write(CircleTime);
            writer.Write(OrganBurstIntervalTimer);
            writer.Write(OrganBurstCount);
            writer.Write(HasSummoned40PercentMinions);
            writer.Write(BloodTimer);
            writer.Write(BloodInterval);
            writer.Write(MinionSpawnTimer);
            writer.Write(MinionSpawnCount);
            writer.Write(MinionSpawnType);
            writer.Write(BombSpawnTimer);
            writer.Write(BombSpawnCount);
            writer.Write(HasTriggeredNodes);
            writer.WriteVector2(DesperationOrbitCenter);
            writer.Write(DesperationTimer);
            writer.Write(anyNodesAlive);
        }

        // Read extra AI fields for multiplayer sync
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            CurrentAttack = (attackType)reader.ReadInt32();
            FollowTime = reader.ReadInt32();
            IsDashing = reader.ReadBoolean();
            DashCount = reader.ReadInt32();
            DashTime = reader.ReadInt32();
            SpitTime = reader.ReadInt32();
            circleradius = reader.ReadSingle();
            circlerotspeed = reader.ReadSingle();
            CircleTime = reader.ReadInt32();
            OrganBurstIntervalTimer = reader.ReadInt32();
            OrganBurstCount = reader.ReadInt32();
            HasSummoned40PercentMinions = reader.ReadBoolean();
            BloodTimer = reader.ReadInt32();
            BloodInterval = reader.ReadInt32();
            MinionSpawnTimer = reader.ReadInt32();
            MinionSpawnCount = reader.ReadInt32();
            MinionSpawnType = reader.ReadInt32();
            BombSpawnTimer = reader.ReadInt32();
            BombSpawnCount = reader.ReadInt32();
            HasTriggeredNodes = reader.ReadBoolean();
            DesperationOrbitCenter = reader.ReadVector2();
            DesperationTimer = reader.ReadInt32();
            anyNodesAlive = reader.ReadBoolean();
        }

        

        public override bool? CanBeHitByProjectile(Projectile projectile)
            {
                if (CurrentAttack == attackType.Desperation || anyNodesAlive)
                    return false;

                return base.CanBeHitByProjectile(projectile);
            }

            public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
            {
                if (CurrentAttack == attackType.Desperation || anyNodesAlive)
                {
                    NPC.immortal = true;
                    modifiers.FinalDamage *= 0f;
                }
            }
            
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (CurrentAttack == attackType.Desperation || anyNodesAlive)
                return false;

            return base.CanBeHitByItem(player, item);
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (CurrentAttack == attackType.Desperation || anyNodesAlive)
            {
                NPC.immortal = true;
                modifiers.FinalDamage *= 0f;
            }
        }

        public override bool? CanCollideWithPlayerMeleeAttack(Player player, Item item, Rectangle meleeAttackHitbox)
        {
            return base.CanCollideWithPlayerMeleeAttack(player, item, meleeAttackHitbox);
        }

        public Vector2 Center;
        public int DeathInterval = 10;

        public bool HasMultOnEnrage = false;
        public override void AI()
        {
            MusicCreditSystem.ShowCredit = true;
            MusicCreditSystem.CreditText = "Track: 'Running From Demons' By Waterflame | https://www.youtube.com/channel/UCVuv5iaVR55QXIc_BHQLakA";

            Player player = Main.player[NPC.target];

            Center = NPC.Center;

            float circleangle = Main.GameUpdateCount * circlerotspeed;

            Vector2 offset = new Vector2(MathF.Cos(circleangle), MathF.Sin(circleangle)) * circleradius;

            Vector2 offsetDes = new Vector2(MathF.Cos(circleangle), MathF.Sin(circleangle)) * 20;

            Vector2 ToPlayer = NPC.Center - player.Center;

            Vector2 ToPlayerInverse = player.Center - NPC.Center;


            Vector2 RandNearPlayer = player.Center + new Vector2(Main.rand.NextFloat(-200f, 200f), Main.rand.NextFloat(-200f, 200f));

            if (NPC.target < 0 || NPC.target == 250 || player.dead) NPC.TargetClosest(true);
            if (player.dead && NPC.timeLeft > 300) NPC.timeLeft = 300;

            if (!(player.ZoneCrimson && (player.ZoneOverworldHeight || player.ZoneSkyHeight)))
            {
                NPC.velocity += new Vector2(0, -400);
                if (NPC.position.Y < Main.worldSurface * 16f)
                {
                    NPC.immortal = true;
                    if (!HasMultOnEnrage)
                    {
                        NPC.velocity *= 3f;
                        NPC.damage *= 2;
                        HasMultOnEnrage = true;
                    }
                    
                    //Main.NewText("CTA : The Wyvern Corpse can only be fought in the surface or space crimson biome.", 255, 35, 0);
                }
            }

            // Assuming this is inside your boss NPC code
            anyNodesAlive = Main.npc.Any(n => n.active && n.type == ModContent.NPCType<IchorNode>());

            if (anyNodesAlive)
            {
                NPC.dontTakeDamage = true;
                NPC.immortal = true;
                NPC.life++;
            }
            else if (!anyNodesAlive && CurrentAttack == attackType.Desperation)
            {
                NPC.immortal = true;
            }
            else
            {
                NPC.dontTakeDamage = false;

            }

            if (player.dead)
            {
                DeathInterval--;
                if (DeathInterval <= 0)
                {
                    NPC.active = false;
                }
            }

            if (NPC.life >= NPC.lifeMax * 0.24f && NPC.life <= NPC.lifeMax * 0.25f)
            {
                if (HasTriggeredNodes == false)
                {
                    CurrentAttack = attackType.Nodes;

                    HasTriggeredNodes = true;

                }
            }

            if (NPC.life <= NPC.lifeMax * 0.05f)
            {
                CurrentAttack = attackType.Desperation;
            }

            if (CurrentAttack == attackType.Desperation)
            {
                NPC.dontTakeDamage = true;
                NPC.immortal = true;
            }




            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.ai[0] == 0f)
                {
                    NPC.ai[2] = NPC.whoAmI;
                    NPC.realLife = NPC.whoAmI;
                    int num96 = NPC.whoAmI;
                    for (int num97 = 0; num97 < 20; num97++)
                    {
                        int WyvBodyInt = ModContent.NPCType<WyvernCorpseBody1>();
                        if (num97 == 4 || num97 == 16) WyvBodyInt = ModContent.NPCType<WyvernCorpseLegs>();
                        else
                        {
                            if (num97 == 17) WyvBodyInt = ModContent.NPCType<WyvernCorpseBody2>();
                            else
                            {
                                if (num97 == 18) WyvBodyInt = ModContent.NPCType<WyvernCorpseBody3>();
                                else
                                {
                                    if (num97 == 19) WyvBodyInt = ModContent.NPCType<WyvernCorpseTail>();
                                }
                            }
                        }
                        int num99 = NPC.NewNPC(NPC.GetSource_FromAI(), ((int)NPC.position.X + (NPC.width / 2)), (int)(NPC.position.Y + NPC.height), WyvBodyInt, NPC.whoAmI);
                        Main.npc[num99].ai[2] = NPC.whoAmI;
                        Main.npc[num99].realLife = NPC.whoAmI;
                        Main.npc[num99].ai[1] = num96;
                        Main.npc[num96].ai[0] = num99;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num99);
                        num96 = num99;
                    }
                }
            }

            Mod.Logger.Info($"Current State: {CurrentAttack}");

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/RunningFromDemons");
            }

            int MinionSpawnType = Main.rand.Next(new int[]
                            {
                                NPCID.IchorSticker,
                                NPCID.Crimera,
                                NPCID.BloodZombie,
                                NPCID.LeechHead,
                                NPCID.Crimslime
                            });

            ImportantMathematics();

            switch (CurrentAttack)
            {
                case attackType.Follow:
                    {
                        NPC.aiStyle = NPCAIStyleID.Worm;
                        FollowTime++;
                        if (FollowTime == 360)
                        {
                            CurrentAttack = attackType.IchorRam;
                        }
                    }
                    break;
                case attackType.Nodes:
                    if (NPC.type == ModContent.NPCType<WyvernCorpseHead>())
                    {
                        NPC.aiStyle = -1;
                        NodeSpawn();

                        CurrentAttack = attackType.IchorRam;
                        ResetStats();
                    }
                    break;
                case attackType.Dash:
                    {
                        NPC.aiStyle = NPCAIStyleID.Worm;
                        if (NPC.DistanceSQ(player.Center) < 4000 && DashTime > 0 && IsDashing == false)
                        {
                            SoundEngine.PlaySound(Roar, NPC.Center);
                            NPC.velocity = NPC.oldVelocity * 5;
                            DashTime--;
                            IsDashing = true;
                            DashCount += 1;
                        }
                        if (DashTime <= 0)
                        {
                            IsDashing = false;
                        }
                        if (DashCount == 5)
                        {
                            CurrentAttack = attackType.SummonCrimsonMinions;
                            ResetStats();
                        }
                    }
                    break;
                case attackType.IchorRam:
                    {
                        NPC.aiStyle = NPCAIStyleID.Bat;
                        SpitTime++;
                        Projectile IchorSpit = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), NPC.Center, NPC.velocity * 3, ProjectileID.GoldenShowerHostile, 40, 2);
                        if (SpitTime >= 480)
                        {
                            CurrentAttack = attackType.OrganBurst;
                            ResetStats();
                        }
                    }
                    break;
                case attackType.Circle:
                    {
                        NPC.position = player.Center + offset - new Vector2(NPC.width / 2, NPC.height / 2);
                        circleradius--;
                        if (Main.rand.NextBool(3) && NPC.life > NPC.lifeMax * 0.4f)
                        {
                            NPC Minion = NPC.NewNPCDirect(Entity.GetSource_FromThis(), NPC.Center, MinionSpawnType);
                            Minion.damage = 30;
                            Minion.lifeMax = 400;
                            Minion.life = 400;
                            Minion.noGravity = true;
                        }
                        if (Main.rand.NextBool(3) && NPC.life < NPC.lifeMax * 0.4f)
                        {
                            Projectile FleshBomb = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), RandNearPlayer, Vector2.Zero, ModContent.ProjectileType<FleshBomb>(), 20, 1);
                        }
                        CircleTime++;
                        if (CircleTime >= 600)
                        {
                            CurrentAttack = attackType.BloodShoot;
                            ResetStats();
                        }
                    }
                    break;
                case attackType.OrganBurst:
                    {
                        NPC.aiStyle = NPCAIStyleID.CursedSkull;
                        float numberProjectiles = 5 + Main.rand.Next(3); // 3, 4, or 5 shots
                        float rotation = MathHelper.ToRadians(45);
                        Vector2 position = NPC.Center;
                        Vector2 velocity = NPC.velocity;

                        position += Vector2.Normalize(velocity) * 45f;


                        int type = Main.rand.Next(new int[]
                            {
                                ModContent.ProjectileType<OrganProjectile_Variant1>(),
                                ModContent.ProjectileType<OrganProjectile_Variant2>(),
                                ModContent.ProjectileType<OrganProjectile_Variant3>(),
                                ModContent.ProjectileType<OrganProjectile_Variant4>()
                            });
                        OrganBurstIntervalTimer++;
                        if (OrganBurstIntervalTimer == 20)
                        {
                            SoundEngine.PlaySound(Roar, NPC.Center);
                            for (int i = 0; i < numberProjectiles; i++)
                            {
                                Vector2 perturbedSpeed = (NPC.velocity).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                                Projectile Organ = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, perturbedSpeed, type, 44, 2);
                            }

                            // Find the first WyvernCorpseBody1 segment belonging to this boss
                            NPC B1 = null;
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                if (Main.npc[n].active && Main.npc[n].type == ModContent.NPCType<WyvernCorpseBody1>() && Main.npc[n].realLife == NPC.whoAmI)
                                {
                                    B1 = Main.npc[n];
                                    break;
                                }
                            }

                            if (B1 != null)
                            {
                                Projectile OrganBody = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), B1.Center, (B1.Center - player.Center) * 2f, type, 44, 2);
                            }
                            OrganBurstIntervalTimer = 0;
                            OrganBurstCount += 1;
                        }
                        if (OrganBurstCount >= 10 && NPC.life > NPC.lifeMax * 0.4f)
                        {
                            CurrentAttack = attackType.Circle;
                            ResetStats();
                        }
                        if (OrganBurstCount > 10 && NPC.life <= NPC.lifeMax * 0.4f)
                        {
                            CurrentAttack = attackType.SummonAxes;
                            ResetStats();
                        }
                    }
                    break;
                case attackType.SummonCrimsonMinions:
                    {
                        Vector2 UndergroundPos = new Vector2(player.Center.X, player.Center.Y + 5000);
                        NPC.velocity = NPC.Center - UndergroundPos;


                        Vector2 spawnOffset = new Vector2(Main.rand.Next(-400, 401), Main.rand.Next(-400, 401));
                        Vector2 spawnPosition = player.Center + spawnOffset;
                        int EnemyCount = 3;
                        MinionSpawnTimer++;





                        if (NPC.Center == UndergroundPos && MinionSpawnTimer == 10)
                        {
                            SoundEngine.PlaySound(Roar, NPC.Center);
                            for (int i = 0; i < EnemyCount; i++)
                            {
                                NPC Minion = NPC.NewNPCDirect(Entity.GetSource_FromThis(), (int)spawnPosition.X, (int)spawnPosition.Y, MinionSpawnType);
                                Minion.damage = 30;
                                Minion.noGravity = true;
                            }
                            MinionSpawnCount += 1;
                        }
                        if (MinionSpawnCount >= 6 || circleradius <= 1000f)
                        {
                            CurrentAttack = attackType.BloodShoot;
                            ResetStats();
                        }

                    }
                    break;
                case attackType.SummonAxes:
                    {
                        if (NPC.life <= NPC.lifeMax * 0.4f && HasSummoned40PercentMinions == false)
                        {
                            SoundEngine.PlaySound(Roar, NPC.Center);

                            NPC.NewNPC(Entity.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TheGreatFlayer>());

                            HasSummoned40PercentMinions = true;
                        }
                        else
                        {
                            CurrentAttack = attackType.BloodShoot;
                            ResetStats();
                        }
                    }
                    break;
                case attackType.BloodShoot:
                    {
                        NPC.aiStyle = NPCAIStyleID.CursedSkull;
                        BloodTimer++;
                        if (BloodTimer % 30 == 0)
                        {
                            Projectile Blood = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), NPC.Center, (player.Center - NPC.Center), ModContent.ProjectileType<BloodProjectile>(), 40, 1);
                        }
                        NPC B1 = null;
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            if (Main.npc[n].active && Main.npc[n].type == ModContent.NPCType<WyvernCorpseBody1>() && Main.npc[n].realLife == NPC.whoAmI)
                            {
                                B1 = Main.npc[n];
                                break;
                            }
                        }

                        if (B1 != null)
                        {
                            if (BloodTimer % 30 == 0)
                            {
                                Projectile BloodBody = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), B1.Center, (player.Center - B1.Center), ModContent.ProjectileType<BloodProjectile>(), 40, 1);
                            }
                        }

                        if (BloodTimer >= 120)
                        {
                            CurrentAttack = attackType.FleshBombShoot;
                            ResetStats();
                        }
                    }
                    break;
                case attackType.FleshBombShoot:
                    {
                        DesperationOrbitCenter = NPC.Center;
                        BombSpawnTimer++;
                        if (BombSpawnTimer >= 40)
                        {

                            Projectile FleshBomb = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<FleshBomb>(), 20, 1);
                            BombSpawnCount += 1;
                            BombSpawnTimer = 0;
                        }
                        if (BombSpawnCount >= 8)
                        {
                            CurrentAttack = attackType.Follow;
                            ResetStats();
                        }
                    }
                    break;

                case attackType.Desperation:
                    {
                        DesperationTimer++;
                        NPC.position = DesperationOrbitCenter + offsetDes - new Vector2(NPC.width / 2, NPC.height / 2);
                        DesperationOrbitCenter = Vector2.SmoothStep(DesperationOrbitCenter, player.Center, 0.01f);
                        circleradius--;
                        Projectile FinalRain = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), NPC.Center, new Vector2(Main.rand.NextFloat(-2, 2), 15), ProjectileID.GoldenShowerHostile, 25, 1);
                        if (DesperationTimer >= 600)
                        {
                            NPC.immortal = false;
                            NPC.StrikeInstantKill();
                        }
                    }
                    break;
            }


            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;
        }

        public void ResetStats()
        {
            FollowTime = 0;
            IsDashing = false;
            DashCount = 0;
            DashTime = 40;
            SpitTime = 0;
            CircleTime = 0;
            circleradius = 1500f;
            OrganBurstIntervalTimer = 0;
            OrganBurstCount = 0;
            BloodTimer = 0;
            MinionSpawnTimer = 0;
            MinionSpawnCount = 0;
            BombSpawnTimer = 0;
            BombSpawnCount = 0;
        }

        public void DrawTelegraph(Vector2 start, Vector2 end, Texture2D texture)
        {
            Vector2 direction = end - start;
            float length = direction.Length();
            direction.Normalize();
            texture ??= ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/LaserGlow").Value;

            float rotation = direction.ToRotation();

            // Assuming your texture is a chain segment, like 16px long
            float segmentLength = texture.Height; // or Width, depending on the texture orientation

            for (float i = 0; i < length; i += segmentLength)
            {
                Vector2 position = start + direction * i;

                Main.spriteBatch.Draw(
                    texture,
                    position - Main.screenPosition,
                    null,
                    Color.White,
                    rotation + MathHelper.PiOver2, // Adjust if your texture points upward
                    new Vector2(texture.Width / 2f, texture.Height / 2f), // Origin at center
                    1f, // Scale
                    SpriteEffects.None,
                    0f
                );
            }
        }


        public void ImportantMathematics()
        {
            Player player = Main.player[NPC.target];
            int num107 = (int)(NPC.position.X / 16f) - 1;
            int num108 = (int)((NPC.position.X + NPC.width) / 16f) + 2;
            int num109 = (int)(NPC.position.Y / 16f) - 1;
            int num110 = (int)((NPC.position.Y + NPC.height) / 16f) + 2;

            if (num107 < 0) num107 = 0;
            if (num108 > Main.maxTilesX) num108 = Main.maxTilesX;
            if (num109 < 0) num109 = 0;
            if (num110 > Main.maxTilesY) num110 = Main.maxTilesY;
            if (NPC.velocity.X < 0f) NPC.spriteDirection = 1;
            if (NPC.velocity.X > 0f) NPC.spriteDirection = -1;

            float num115 = 16f;
            float num116 = 0.4f;

            Vector2 vector14 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float num118 = Main.rand.Next(-500, 501) + player.position.X + (player.width / 2);
            float num119 = Main.rand.Next(-500, 501) + player.position.Y + (player.height / 2);
            num118 = ((int)(num118 / 16f) * 16);
            num119 = ((int)(num119 / 16f) * 16);
            vector14.X = ((int)(vector14.X / 16f) * 16);
            vector14.Y = ((int)(vector14.Y / 16f) * 16);
            num118 -= vector14.X;
            num119 -= vector14.Y;
            float num120 = (float)Math.Sqrt((num118 * num118 + num119 * num119));

            float num123 = Math.Abs(num118);
            float num124 = Math.Abs(num119);
            float num125 = num115 / num120;
            num118 *= num125;
            num119 *= num125;

            bool flag14 = false;
            if (((NPC.velocity.X > 0f && num118 < 0f) || (NPC.velocity.X < 0f && num118 > 0f) || (NPC.velocity.Y > 0f && num119 < 0f) || (NPC.velocity.Y < 0f && num119 > 0f)) && Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) > num116 / 2f && num120 < 300f)
            {
                flag14 = true;
                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < num115) NPC.velocity *= 1.1f;
            }
            if (NPC.position.Y > player.position.Y || (player.position.Y / 16f) > Main.worldSurface || player.dead)
            {
                flag14 = true;
                if (Math.Abs(NPC.velocity.X) < num115 / 2f)
                {
                    if (NPC.velocity.X == 0f) NPC.velocity.X = NPC.velocity.X - NPC.direction;
                    NPC.velocity.X = NPC.velocity.X * 1.1f;
                }
                else
                {
                    if (NPC.velocity.Y > -num115) NPC.velocity.Y = NPC.velocity.Y - num116;
                }
            }
            if (!flag14)
            {
                if ((NPC.velocity.X > 0f && num118 > 0f) || (NPC.velocity.X < 0f && num118 < 0f) || (NPC.velocity.Y > 0f && num119 > 0f) || (NPC.velocity.Y < 0f && num119 < 0f))
                {
                    if (NPC.velocity.X < num118) NPC.velocity.X = NPC.velocity.X + num116;
                    else
                    {
                        if (NPC.velocity.X > num118) NPC.velocity.X = NPC.velocity.X - num116;
                    }
                    if (NPC.velocity.Y < num119) NPC.velocity.Y = NPC.velocity.Y + num116;
                    else
                    {
                        if (NPC.velocity.Y > num119) NPC.velocity.Y = NPC.velocity.Y - num116;
                    }
                    if (Math.Abs(num119) < num115 * 0.2 && ((NPC.velocity.X > 0f && num118 < 0f) || (NPC.velocity.X < 0f && num118 > 0f)))
                    {
                        if (NPC.velocity.Y > 0f) NPC.velocity.Y = NPC.velocity.Y + num116 * 2f;
                        else NPC.velocity.Y = NPC.velocity.Y - num116 * 2f;
                    }
                    if (Math.Abs(num118) < num115 * 0.2 && ((NPC.velocity.Y > 0f && num119 < 0f) || (NPC.velocity.Y < 0f && num119 > 0f)))
                    {
                        if (NPC.velocity.X > 0f) NPC.velocity.X = NPC.velocity.X + num116 * 2f;
                        else NPC.velocity.X = NPC.velocity.X - num116 * 2f;
                    }
                }
                else
                {
                    if (num123 > num124)
                    {
                        if (NPC.velocity.X < num118) NPC.velocity.X = NPC.velocity.X + num116 * 1.1f;
                        else
                        {
                            if (NPC.velocity.X > num118) NPC.velocity.X = NPC.velocity.X - num116 * 1.1f;
                        }
                        if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num115 * 0.5)
                        {
                            if (NPC.velocity.Y > 0f) NPC.velocity.Y = NPC.velocity.Y + num116;
                            else NPC.velocity.Y = NPC.velocity.Y - num116;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.Y < num119) NPC.velocity.Y = NPC.velocity.Y + num116 * 1.1f;
                        else
                        {
                            if (NPC.velocity.Y > num119) NPC.velocity.Y = NPC.velocity.Y - num116 * 1.1f;
                        }
                        if ((Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) < num115 * 0.5)
                        {
                            if (NPC.velocity.X > 0f) NPC.velocity.X = NPC.velocity.X + num116;
                            else NPC.velocity.X = NPC.velocity.X - num116;
                        }
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1) effects = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, new Vector2(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - texture.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - texture.Height * NPC.scale + 4f + origin.Y * NPC.scale + 56f), new Rectangle?(NPC.frame), drawColor, NPC.rotation, origin, NPC.scale, effects, 0f);
            return false;
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

                    NPC.NewNPC(Entity.GetSource_FromThis(), (int)spawnPosition.X, (int)spawnPosition.Y, ModContent.NPCType<IchorNode>());

                PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp1>(), NPC.Center, Vector2.Zero, ColorLib.Ichor, 0.4f);

            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 4; i++)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Main.rand.Next(61, 64), 1f);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            npcLoot.Add(ItemDropRule.Common(ItemID.SoulofFlight, 1, 5, 20));



        }

        public override void OnKill()
        {
            MusicCreditSystem.ShowCredit = false;
        }

    }

    public class WyvernCorpseBCL : ModSystem
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
            string internalName = "WyvernCorpseHead";

            // Value inferred from boss progression, see the wiki for details
            float weight = 18.3f;

            // Used for tracking checklist progress
            Func<bool> downed = () => DownedBossSystem.downedWyvernCorpseBoss;

            LocalizedText Hint = Language.GetText("Mods.DestroyerTest.BossChecklist.WyvernCorpse.Hint");

            LocalizedText Despawn = Language.GetText("Mods.DestroyerTest.NPCs.WyvernCorpseHead.DespawnMessage");

            // The NPC type of the boss
            int bossType = ModContent.NPCType<WyvernCorpseHead>();

            // The item used to summon the boss with (if available)
            int spawnItem = ModContent.ItemType<EuthanizedViciousBunny>();

            // "collectibles" like relic, trophy, mask, pet
            List<int> collectibles = new List<int>()
            {
                ModContent.ItemType<RibChainsaw>(),
                ModContent.ItemType<GreatFlayer>(),
                ModContent.ItemType<WyvernTail>(),
                ModContent.ItemType<WyvernSkull>(),
                ItemID.Ichor,
                ModContent.ItemType<Item_WyvernCorpseRelic>(),
                ModContent.ItemType<Item_WyvernCorpseTrophy>()
            };

            // By default, it draws the first frame of the boss, omit if you don't need custom drawing
            // But we want to draw the bestiary texture instead, so we create the code for that to draw centered on the intended location
            var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) =>
            {
                Texture2D texture = ModContent.Request<Texture2D>("DestroyerTest/Content/Extras/WyvernCorpseBossChecklist").Value;
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
    public class IchorNode : ModNPC
    {
        public override string BossHeadTexture => "DestroyerTest/Content/Entity/IchorNode_Head_Boss";
        public override void SetStaticDefaults()
        {
            NPCID.Sets.CanHitPastShimmer[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
            NPCID.Sets.TrailCacheLength[Type] = 20;
            NPCID.Sets.TrailingMode[Type] = 3;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            // To hide from bestiary, override SetBestiary and leave it empty.
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 100;
            NPC.aiStyle = -1;
            NPC.damage = 25;
            NPC.defense = 20;
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
            NPC.netID = ModContent.NPCType<IchorNode>();
        }

        public override void SetBestiary(Terraria.GameContent.Bestiary.BestiaryDatabase database, Terraria.GameContent.Bestiary.BestiaryEntry bestiaryEntry)
        {
            // Do not add any info elements to hide from bestiary
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
                if (n.active && n.type == ModContent.NPCType<WyvernCorpseHead>())
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
            WyvernCorpseHead modBoss = bossNPC.ModNPC as WyvernCorpseHead;

            // If NPCHead is a custom property
            Vector2 OrbitCenter = modBoss != null ? modBoss.Center : bossNPC.Center;

            bool ParentAlive = Main.npc.Any(n => n.active && n.type == ModContent.NPCType<WyvernCorpseHead>());

            if (ParentAlive)
            {
                NPC.active = true;
            }
            else
            {
                NPC.active = false;
            }

            // Orbit settings
            float radius = 300f;
            float speed = 0.05f;
            float angle = Main.GameUpdateCount * speed;

            // Get a list of all active CursedFlameNode NPCs
            List<NPC> allNodes = new List<NPC>();

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC node = Main.npc[i];
                if (node.active && node.type == ModContent.NPCType<IchorNode>())
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