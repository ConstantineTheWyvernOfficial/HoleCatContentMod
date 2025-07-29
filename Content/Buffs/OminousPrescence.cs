using System.Security.AccessControl;
using DestroyerTest.Content.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Buffs
{
	// This class serves as an example of a debuff that causes constant loss of life
	// See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
	public class OminousPrescence : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
            Main.buffNoTimeDisplay[Type] = true;
        }

		// Allows you to make this buff give certain effects to the given player
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<OPPlayer>().EimvurIsWatching = true;
		}
	}

	public class OPPlayer : ModPlayer
	{
		// Flag checking when life regen debuff should be activated
		public bool EimvurIsWatching;

		public override void ResetEffects() {
			EimvurIsWatching = false;
		}
	}
}