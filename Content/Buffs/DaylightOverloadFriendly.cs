using DestroyerTest.Content.Dusts;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Buffs
{
	// This class serves as an example of a debuff that causes constant loss of life
	// See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
	public class DaylightOverloadFriendly : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}

		// Allows you to make this buff give certain effects to the given player
		public override void Update(NPC target, ref int buffIndex) {
			if (target.TryGetGlobalNPC<DOFTarget>(out var modNPC)) {
                modNPC.lifeRegenDebuff = true;
            }
		}
	}

	public class DOFTarget : GlobalNPC
    {
        public override bool InstancePerEntity => true; // Ensures each NPC has its own instance

        public bool lifeRegenDebuff;

        public override void ResetEffects(NPC npc) {
            lifeRegenDebuff = false;
        }

        public void UpdateLifeRegen(NPC npc, Player player, ref int damage) {
            if (lifeRegenDebuff && Main.dayTime) {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Lava, 0.0f, 0.5f, 0, default, 1);

                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 24;
            }
			if (lifeRegenDebuff && !Main.dayTime) {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Lava, 0.0f, 0.5f, 0, default, 1);
				player.moveSpeed += 0.15f;

                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 24;
            }
        }
    }
}