
using DestroyerTest.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Buffs
{
    public class HateWither : ModBuff
    {
        private const float SpreadRadius = 100f; // Radius in pixels for buff spreading

        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(NPC target, ref int buffIndex) {
            if (target.TryGetGlobalNPC<HWTarget>(out var modNPC)) {
                modNPC.lifeRegenDebuff = true;
            }

            // Spread the buff to nearby NPCs
            foreach (NPC npc in Main.npc) {
                if (npc.active && !npc.friendly && npc.whoAmI != target.whoAmI && npc.Distance(target.Center) < SpreadRadius) {
                    npc.AddBuff(Type, target.buffTime[buffIndex]); // Apply buff with same duration
                }
            }
        }
    }

    public class HWTarget : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool lifeRegenDebuff;

        public override void ResetEffects(NPC npc) {
            lifeRegenDebuff = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            if (lifeRegenDebuff) {
                // Spawn dust around NPC and make it gravitate toward the center
                for (int i = 0; i < 5; i++) {
                    Vector2 spawnPosition = npc.Center + Main.rand.NextVector2Circular(30f, 30f);
                    Vector2 velocity = Vector2.Normalize(npc.Center - spawnPosition) * 2f; // Moves toward center
                    int dustIndex = Dust.NewDust(spawnPosition, 0, 0, DustID.LifeCrystal, velocity.X, velocity.Y, 0, Color.Red, 1.5f);
                    Main.dust[dustIndex].noGravity = true;
                }

                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 48;
            }
        }
    }
}
