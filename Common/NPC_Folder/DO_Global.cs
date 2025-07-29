using DestroyerTest.Content.Projectiles;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Common
{
	internal class DO_Global : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool DaylightOverloadActive;

		public override void ResetEffects(NPC npc) {
			DaylightOverloadActive = false;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage) {
			if (DaylightOverloadActive) {
				if (npc.lifeRegen > 0) {
					npc.lifeRegen = 0;
				}
				// Remember, lifeRegen affects the actual life loss, damage is just the text.
				// The logic shown here matches how vanilla debuffs stack in terms of damage numbers shown and actual life loss.
				npc.lifeRegen -= 3;
				if (damage < 4) {
					damage = 20;
				}
			}
		}

	}
}