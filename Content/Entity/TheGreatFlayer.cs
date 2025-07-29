using DestroyerTest.Content.RiftBiome;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DestroyerTest.Content.Entity
{
    [AutoloadHead]
    public class TheGreatFlayer : ModNPC
    {



        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            NPC.width = 160;
            NPC.height = 160;
            NPC.aiStyle = 23;
            NPC.damage = 100;
            NPC.defense = 9999;
            NPC.lifeMax = 160000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                // This gives you a valid texture index
                int headSlot = Mod.AddBossHeadTexture("DestroyerTest/Content/Entity/TheGreatFlayer_Head", ModContent.NPCType<TheGreatFlayer>());

                // Assign it to the NPC type
                NPCID.Sets.BossHeadTextures[ModContent.NPCType<TheGreatFlayer>()] = headSlot;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A cleaver made from Flesh and Bones. It is a horrifying sight."),
            });
        }

        public override void AI()
        {
            bool ParentAlive = Main.npc.Any(n => n.active && n.type == ModContent.NPCType<WyvernCorpseHead>());

            if (ParentAlive)
            {
                
                NPC.active = true;
            }
            else
            {
                NPC.StrikeInstantKill();
                NPC.active = false;
            }
        }

       

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Ichor, 300);
        }

        
    }
}