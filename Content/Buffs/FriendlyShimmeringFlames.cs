using DestroyerTest.Common;
using DestroyerTest.Content.Dusts;
using DestroyerTest.Content.Particles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Buffs
{
	// This class serves as an example of a debuff that causes constant loss of life
	// See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
	public class FriendlyShimmeringFlames : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}

		// Allows you to make this buff give certain effects to the given player
		public override void Update(NPC target, ref int buffIndex) {
			if (target.TryGetGlobalNPC<FSFTarget>(out var modNPC)) {
                modNPC.lifeRegenDebuff = true;
            }
		}
	}

	public class FSFTarget : GlobalNPC
    {
        public override bool InstancePerEntity => true; // Ensures each NPC has its own instance

        public bool lifeRegenDebuff;

        public override void ResetEffects(NPC npc) {
            lifeRegenDebuff = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            if (lifeRegenDebuff) {
                int[] types = new int[]
			{
				PRTLoader.GetParticleID<ColoredFire1>(),
				PRTLoader.GetParticleID<ColoredFire2>(),
				PRTLoader.GetParticleID<ColoredFire3>(),
				PRTLoader.GetParticleID<ColoredFire4>(),
				PRTLoader.GetParticleID<ColoredFire5>(),
				PRTLoader.GetParticleID<ColoredFire6>(),
				PRTLoader.GetParticleID<ColoredFire7>()
			};

				PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], Main.rand.NextVector2FromRectangle(npc.getRect()), new Vector2(0f, -0.1f), ColorLib.TenebrisGradient, 0.3f);

                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 24;
            }
        }
    }
}