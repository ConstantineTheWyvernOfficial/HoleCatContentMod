using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DestroyerTest.Common;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using System.Threading;
using Terraria.Chat;
using DestroyerTest.Common.Systems;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Buffs;
using Terraria.Audio;
using InnoVault.PRT;
using DestroyerTest.Content.Particles;

namespace DestroyerTest.Common
{
    public class DamageTrackerGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int LastDamageDone { get; set; } = 0;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (var proj in Main.projectile)
            {
                if (proj.active && proj.owner == Main.myPlayer)
                {
                    LastDamageDone = damageDone;
                }
            }
        }
    }


    public class TenebrousTrialModDifficulty : ModSystem
    {
        public bool IsActive = false;
        public int BossDefeatCount = 0;
        public int MaxDPS = 70;
        public int MaxDamageperHit = 20;
        public static LocalizedText EyeofCuthuluDefeated { get; private set; }
        public static LocalizedText KingSlimeDefeated { get; private set; }
        public static LocalizedText BrainofCuthuluDefeated { get; private set; }
        public static LocalizedText EaterofWorldsDefeated { get; private set; }
        public static LocalizedText DeerclopsDefeated { get; private set; }
        public static LocalizedText QueenBeeDefeated { get; private set; }
        public static LocalizedText SkeletronDefeated { get; private set; }
        public static LocalizedText WallofFleshDefeated { get; private set; }
        public static LocalizedText QueenSlimeDefeated { get; private set; }
        public static LocalizedText DestroyerDefeated { get; private set; }
        public static LocalizedText TwinsDefeated { get; private set; }
        public static LocalizedText SkeletronPrimeDefeated { get; private set; }
        public static LocalizedText NautilusDefeated { get; private set; }
        public static LocalizedText PlanteraDefeated { get; private set; }
        public static LocalizedText GolemDefeated { get; private set; }
        public static LocalizedText FishronDefeated { get; private set; }
        public static LocalizedText EmpressDefeated { get; private set; }
        public static LocalizedText CultistDefeated { get; private set; }
        public static LocalizedText LunarBossDefeated { get; private set; }
        private LocalizedText GetBossDefeatedText(string bossName)
        {
            return bossName switch
            {
                nameof(DownedBossSystem.downedEoCBoss) => EyeofCuthuluDefeated,
                nameof(DownedBossSystem.downedKingSlimeBoss) => KingSlimeDefeated,
                nameof(DownedBossSystem.downedBoCBoss) => BrainofCuthuluDefeated,
                nameof(DownedBossSystem.downedEoWBoss) => EaterofWorldsDefeated,
                nameof(DownedBossSystem.downedDeerclopsMiniBoss) => DeerclopsDefeated,
                nameof(DownedBossSystem.downedQueenBeeBoss) => QueenBeeDefeated,
                nameof(DownedBossSystem.downedSkeletronBoss) => SkeletronDefeated,
                nameof(DownedBossSystem.downedWallBoss) => WallofFleshDefeated,
                nameof(DownedBossSystem.downedQueenSlimeBoss) => QueenSlimeDefeated,
                nameof(DownedBossSystem.downedDestroyerBoss) => DestroyerDefeated,
                nameof(DownedBossSystem.downedTwinsBoss) => TwinsDefeated,
                nameof(DownedBossSystem.downedSkeletronPrimeBoss) => SkeletronPrimeDefeated,
                nameof(DownedBossSystem.downedNautilusMiniBoss) => NautilusDefeated,
                nameof(DownedBossSystem.downedPlanteraBoss) => PlanteraDefeated,
                nameof(DownedBossSystem.downedGolemBoss) => GolemDefeated,
                nameof(DownedBossSystem.downedFishronBoss) => FishronDefeated,
                nameof(DownedBossSystem.downedEmpressBoss) => EmpressDefeated,
                nameof(DownedBossSystem.downedCultistBoss) => CultistDefeated,
                nameof(DownedBossSystem.downedLunarBoss) => LunarBossDefeated,
                _ => null
            };
        }

        public bool HasPlayed_EoC = false;
        public bool HasPlayed_KS = false;
        public bool HasPlayed_BoC = false;
        public bool HasPlayed_EoW = false;
        public bool HasPlayed_QB = false;
        public bool HasPlayed_Deerclops = false;
        public bool HasPlayed_Skeletron = false;
        public bool HasPlayed_WoF = false;
        public bool HasPlayed_QS = false;
        public bool HasPlayed_Destroyer = false;
        public bool HasPlayed_Twins = false;
        public bool HasPlayed_SkeletronPrime = false;
        public bool HasPlayed_Nautilus = false;
        public bool HasPlayed_Plantera = false;
        public bool HasPlayed_Golem = false;
        public bool HasPlayed_Fishron = false;
        public bool HasPlayed_Empress = false;
        public bool HasPlayed_Cultist = false;
        public bool HasPlayed_LunarBoss = false;
        public override void SetStaticDefaults()
        {
            EyeofCuthuluDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(EyeofCuthuluDefeated)}");
            KingSlimeDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(KingSlimeDefeated)}");
            BrainofCuthuluDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(BrainofCuthuluDefeated)}");
            EaterofWorldsDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(EaterofWorldsDefeated)}");
            DeerclopsDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(DeerclopsDefeated)}");
            QueenBeeDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(QueenBeeDefeated)}");
            SkeletronDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(SkeletronDefeated)}");
            WallofFleshDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(WallofFleshDefeated)}");
            QueenSlimeDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(QueenSlimeDefeated)}");
            DestroyerDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(DestroyerDefeated)}");
            TwinsDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(TwinsDefeated)}");
            SkeletronPrimeDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(SkeletronPrimeDefeated)}");
            NautilusDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(NautilusDefeated)}");
            PlanteraDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(PlanteraDefeated)}");
            GolemDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(GolemDefeated)}");
            FishronDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(FishronDefeated)}");
            EmpressDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(EmpressDefeated)}");
            CultistDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(CultistDefeated)}");
            LunarBossDefeated = Mod.GetLocalization($"TenebrousTrial.{nameof(LunarBossDefeated)}");
        }

        public override void PostUpdateWorld()
        {
            if ((Main.worldName == "Shade Under The Trees" ||
            Main.worldName == "Tenebrous Trial" ||
            Main.worldName == "Respite from Sun" ||
            Main.worldName == "Twilight At Last" ||
            Main.worldName == "Night Life" ||
            Main.worldName == "Garden of Eden" ||
            Main.worldName == "Moon" ||
            Main.worldName == "Witching Hour" ||
            Main.worldName == "Tenebris") && DownedBossSystem.downedLunarBoss == false
            )
            {
                IsActive = true;
            }
            else
            {
                IsActive = false;
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["IsActive"] = IsActive;
            tag["BossDefeatCount"] = BossDefeatCount;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            IsActive = tag.GetBool("IsActive");
            BossDefeatCount = tag.GetInt("BossDefeatCount");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(IsActive);
            writer.Write(BossDefeatCount);
        }

        public override void NetReceive(BinaryReader reader)
        {
            IsActive = reader.ReadBoolean();
            BossDefeatCount = reader.ReadInt32();
        }

        public void BossText()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return; // This should not happen, but just in case.
            }
            foreach (var bossFlag in typeof(DownedBossSystem).GetFields())
            {
                if (bossFlag.FieldType == typeof(bool) && (bool)bossFlag.GetValue(null))
                {
                    var bossDefeatedText = GetBossDefeatedText(bossFlag.Name);
                    if (bossDefeatedText != null)
                    {
                        BroadcastBossDefeatMessage(bossDefeatedText);
                    }
                }
            }
        }
        /*
        private void BroadcastBossDefeatMessage(LocalizedText bossDefeatedText) {
            if (Main.netMode == NetmodeID.SinglePlayer) {
                Main.NewText(bossDefeatedText.Value, ColorLib.TenebrisGradient);
            } else if (Main.netMode == NetmodeID.Server) {
                ChatHelper.BroadcastChatMessage(bossDefeatedText.ToNetworkText(), ColorLib.TenebrisGradient);
            }
        }*/

        private void BroadcastBossDefeatMessage(LocalizedText bossDefeatedText)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(bossDefeatedText.Value, ColorLib.TenebrisGradient);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(bossDefeatedText.ToNetworkText(), ColorLib.TenebrisGradient);
            }
        }

        public override void PostUpdateTime()
        {
            Player player = Main.LocalPlayer;
            DamageTrackerGlobalProjectile damageTracker = new DamageTrackerGlobalProjectile();
            int DPS = Main.LocalPlayer.dpsDamage;
            int DamagePerHit = 0;
            foreach (var proj in Main.projectile)
            {
                if (proj.active && proj.owner == Main.myPlayer)
                {
                    DamagePerHit = damageTracker.LastDamageDone;
                }
            }

            if (IsActive)
            {
                
                if (DownedBossSystem.downedLunarBoss == false)
                {
                    OverlayManagement(player);
                }
                if (DPS > MaxDPS || DamagePerHit > MaxDamageperHit)
                    {
                        player.AddBuff(ModContent.BuffType<ShimmeringFlames>(), 60);
                    }
            }
        }

        public static int VisionRadius = 100;
        public static int ParticleAmount = 300;
        public void OverlayManagement(Player player)
        {
            // This is an example of radius code.
            foreach (Dust d in Main.dust)
            {
                if (Vector2.Distance(d.position, player.Center) > VisionRadius)
                {

                }
            }

            Rectangle ScreenArea = new Rectangle(
                (int)(Main.screenPosition.X),
                (int)(Main.screenPosition.Y),
                Main.screenWidth,
                Main.screenHeight
            );

            for (int i = 0; i < ParticleAmount; i++)
            {
                Vector2 Randpos;
                int attempts = 0;
                float radiusSq = VisionRadius * VisionRadius;
                do
                {
                    Randpos = Main.rand.NextVector2FromRectangle(ScreenArea);
                    attempts++;
                }
                while (Vector2.DistanceSquared(Randpos, player.Center) <= radiusSq && attempts < 100);


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

                Color[] ColorType = new Color[]
                {
                    new Color(0, 0, 0),
                    new Color(0, 0, 0),
                    new Color(0, 0, 0),
                    new Color(0, 0, 0),
                    new Color(0, 0, 0),
                    new Color(0, 0, 0),
                    new Color(0, 0, 0),
                    new Color(0, 0, 0),
                    new Color(37, 9, 39),
                    new Color(61, 52, 38),
                    new Color(21, 23, 39),
                };

                PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], Randpos, Vector2.Zero, ColorType[Main.rand.Next(ColorType.Length)], 1);
            }

        }
    }

    public class TenebrousTrialBossDefeatManager : GlobalNPC
    {
        static TenebrousTrialModDifficulty TTDifficulty = ModContent.GetInstance<TenebrousTrialModDifficulty>();
        public override bool InstancePerEntity => false;

        public override void OnKill(NPC npc)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
            if (TTDifficulty.IsActive == true)
            {
                switch (npc.type)
                {
                    case NPCID.EyeofCthulhu:
                        HandleBossDefeat(ref DownedBossSystem.downedEoCBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/EyeofCuthuluDefeat_TT", "TenebrousTrial.EyeofCuthuluDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 20;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.KingSlime:
                        HandleBossDefeat(ref DownedBossSystem.downedKingSlimeBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/KingQueenSlimeDefeat_TT", "TenebrousTrial.KingSlimeDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.BrainofCthulhu:
                        HandleBossDefeat(ref DownedBossSystem.downedBoCBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/BrainofCuthuluDefeat_TT", "TenebrousTrial.BrainofCuthuluDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 20;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.EaterofWorldsHead:
                        HandleBossDefeat(ref DownedBossSystem.downedEoWBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/EaterofWorldsDefeat_TT", "TenebrousTrial.EaterofWorldsDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 20;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.Deerclops:
                        HandleBossDefeat(ref DownedBossSystem.downedDeerclopsMiniBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/DeerclopsDefeat_TT", "TenebrousTrial.DeerclopsDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.QueenBee:
                        HandleBossDefeat(ref DownedBossSystem.downedQueenBeeBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/QueenBeeDefeat_TT", "TenebrousTrial.QueenBeeDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.SkeletronHead:
                        HandleBossDefeat(ref DownedBossSystem.downedSkeletronBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/SkeletronDefeat_TT", "TenebrousTrial.SkeletronDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.WallofFlesh:
                        HandleBossDefeat(ref DownedBossSystem.downedWallBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/WallofFleshDefeat_TT", "TenebrousTrial.WallofFleshDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 50;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.QueenSlimeBoss:
                        HandleBossDefeat(ref DownedBossSystem.downedQueenSlimeBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/KingQueenSlimeDefeat_TT", "TenebrousTrial.QueenSlimeDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.TheDestroyer:
                        HandleBossDefeat(ref DownedBossSystem.downedDestroyerBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/MechBossDefeat_TT", "TenebrousTrial.DestroyerDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.Retinazer:
                        HandleBossDefeat(ref DownedBossSystem.downedTwinsBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/MechBossDefeat_TT", "TenebrousTrial.TwinsDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.SkeletronPrime:
                        HandleBossDefeat(ref DownedBossSystem.downedSkeletronPrimeBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/MechBossDefeat_TT", "TenebrousTrial.SkeletronPrimeDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.BloodNautilus:
                        HandleBossDefeat(ref DownedBossSystem.downedNautilusMiniBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/DreadnautilusDefeat_TT", "TenebrousTrial.NautilusDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;  
                    case NPCID.Plantera:
                        HandleBossDefeat(ref DownedBossSystem.downedPlanteraBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/PlanteraDefeat_TT", "TenebrousTrial.PlanteraDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 30;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.Golem:
                        HandleBossDefeat(ref DownedBossSystem.downedGolemBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/GolemDefeat_TT", "TenebrousTrial.GolemDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 20;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.DukeFishron:
                        HandleBossDefeat(ref DownedBossSystem.downedFishronBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/FishronDefeat_TT", "TenebrousTrial.FishronDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.HallowBoss:
                        HandleBossDefeat(ref DownedBossSystem.downedEmpressBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/EmpressDefeat_TT", "TenebrousTrial.EmpressDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 10;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.CultistBoss:
                        HandleBossDefeat(ref DownedBossSystem.downedCultistBoss, null, "TenebrousTrial.CultistDefeated");
                        TenebrousTrialModDifficulty.VisionRadius += 20;
                        TTDifficulty.MaxDamageperHit += 8;
                        TTDifficulty.MaxDPS += 10;
                        break;
                    case NPCID.MoonLordCore:
                        HandleBossDefeat(ref DownedBossSystem.downedLunarBoss, "DestroyerTest/Assets/Audio/TenebrousTrialBossDefeats/LightRestored_TT", "TenebrousTrial.LunarBossDefeated");
                        TTDifficulty.MaxDamageperHit += 160000;
                        TTDifficulty.MaxDPS += 1200000;
                        break;
                }
            }
        }

        private void HandleBossDefeat(ref bool bossFlag, string soundPath, string localizationKey)
        {
            TenebrousTrialModDifficulty TTDifficulty = ModContent.GetInstance<TenebrousTrialModDifficulty>();
            if (!bossFlag && TTDifficulty.IsActive == true)
            {
                bossFlag = true;
                BroadcastBossDefeatMessage(Mod.GetLocalization(localizationKey));
                PlayBossDefeatSound(soundPath);
            }
        }

        private void BroadcastBossDefeatMessage(LocalizedText bossDefeatedText)
        {
            TenebrousTrialModDifficulty TTDifficulty = ModContent.GetInstance<TenebrousTrialModDifficulty>();
            if (Main.netMode == NetmodeID.SinglePlayer && TTDifficulty.IsActive == true)
            {
                Main.NewText(bossDefeatedText.Value, ColorLib.TenebrisGradient);
            }
            else if (Main.netMode == NetmodeID.Server && TTDifficulty.IsActive == true)
            {
                ChatHelper.BroadcastChatMessage(bossDefeatedText.ToNetworkText(), ColorLib.TenebrisGradient);
            }
        }

        private void PlayBossDefeatSound(string soundPath)
        {
            TenebrousTrialModDifficulty TTDifficulty = ModContent.GetInstance<TenebrousTrialModDifficulty>();
            if (!string.IsNullOrEmpty(soundPath))
            {
                SoundEngine.PlaySound(new SoundStyle(soundPath));
            }
        }
    }
}