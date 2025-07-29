using DestroyerTest.Content.RiftBiome;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using SteelSeries.GameSense.DeviceZone;
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
    public class GigaCursedHammer : ModNPC
    {



        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            NPC.width = 164;
            NPC.height = 164;
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
                int headSlot = Mod.AddBossHeadTexture("DestroyerTest/Content/Entity/GigaCursedHammer_Head", ModContent.NPCType<GigaCursedHammer>());

                // Assign it to the NPC type
                NPCID.Sets.BossHeadTextures[ModContent.NPCType<GigaCursedHammer>()] = headSlot;
            }
        }

        public override void AI()
        {
            bool ParentAlive = Main.npc.Any(n => n.active && n.type == ModContent.NPCType<NightmareRoseBoss>());

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


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A hammer made of demonite all the way to its core, with the heads being ignited in cursed flames."),
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Texture2D glowMask = ModContent.Request<Texture2D>("DestroyerTest/Content/Entity/GigaCursedHammerGlowmask").Value;
            Vector2 drawPos = NPC.Center - screenPos;
            Vector2 drawPos2 = NPC.Center - screenPos;

            Vector2 offset = new Vector2(2, 2); // adjust the value as needed
            drawPos2 -= offset;
            Main.EntitySpriteDraw(texture, drawPos, null, drawColor, NPC.rotation, texture.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(glowMask, drawPos2, null, Color.White, NPC.rotation, glowMask.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false; // Prevents the vanilla drawing
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
        }

        
    }
}