using DestroyerTest.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Buffs
{
    // This class serves as an example of a debuff that causes constant loss of life
    // See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
    public class SpiritDrift : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }

        // Allows you to make this buff give certain effects to the given player
        public override void Update(NPC target, ref int buffIndex)
        {
            if (target.TryGetGlobalNPC<SDTarget>(out var modNPC))
            {
                modNPC.Levitation = true;
            }
        }
    }

    public class SDTarget : GlobalNPC
    {
        public override bool InstancePerEntity => true; // Ensures each NPC has its own instance

        public bool Levitation;

        public override void ResetEffects(NPC npc)
        {
            Levitation = false;
        }

        public override void PostAI(NPC npc)
        {
            // Call the custom Update logic
            Update(npc);
        }

        public void Update(NPC npc)
        {
            // Only apply the effects if Levitation is active
            if (Levitation)
            {
            npc.velocity = new Vector2(npc.velocity.X / 2, -1.4f);

            // Corrected Dust usage
            Dust.NewDust(npc.position, npc.width, npc.height, DustID.BlueMoss, 0.0f, 0.5f, 0, default, 1);

            // Gradually increase the blue color intensity
            npc.color = new Color(npc.color.R, npc.color.G, (byte)MathHelper.Clamp(npc.color.B + 5, 0, 255));
            }
        }
    }
}
