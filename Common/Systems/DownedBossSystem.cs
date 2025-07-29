using System.Collections;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DestroyerTest.Common.Systems
{
	// Acts as a container for "downed boss" flags.
	// Set a flag like this in your bosses OnKill hook:
	//    NPC.SetEventFlagCleared(ref DownedBossSystem.downedMinionBoss, -1);

	// Saving and loading these flags requires TagCompounds, a guide exists on the wiki: https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound
	public class DownedBossSystem : ModSystem
	{	
		public static bool downedEoCBoss = false;
		public static bool downedKingSlimeBoss = false;
		public static bool downedBoCBoss = false;
		public static bool downedEoWBoss = false;
		public static bool downedQueenBeeBoss = false;
		public static bool downedDeerclopsMiniBoss = false;
		public static bool downedSkeletronBoss = false;
		public static bool downedConstitutionBoss = false;

		public static bool downedWallBoss = false;
		public static bool downedQueenSlimeBoss = false;
		public static bool downedDestroyerBoss = false;
		public static bool downedTwinsBoss = false;
		public static bool downedSkeletronPrimeBoss = false;
		public static bool downedNautilusMiniBoss = false;
		public static bool downedPlanteraBoss = false;
		public static bool downedGolemBoss = false;
		public static bool downedFishronBoss = false;
		public static bool downedEmpressBoss = false;
		public static bool downedCultistBoss = false;
		public static bool downedLunarBoss = false;
		public static bool downedWyvernCorpseBoss = false;
		public static bool downedNightmareRoseBoss = false;

		public override void ClearWorld()
		{
			downedEoCBoss = false;
			downedKingSlimeBoss = false;
			downedBoCBoss = false;
			downedEoWBoss = false;
			downedQueenBeeBoss = false;
			downedDeerclopsMiniBoss = false;
			downedSkeletronBoss = false;
			downedConstitutionBoss = false;
			downedWallBoss = false;
			downedQueenSlimeBoss = false;
			downedDestroyerBoss = false;
			downedTwinsBoss = false;
			downedSkeletronPrimeBoss = false;
			downedNautilusMiniBoss = false;
			downedPlanteraBoss = false;
			downedFishronBoss = false;
			downedEmpressBoss = false;
			downedGolemBoss = false;
			downedCultistBoss = false;
			downedLunarBoss = false;
			downedWyvernCorpseBoss = false;
			downedNightmareRoseBoss = false;
		}

		// We save our data sets using TagCompounds.
		// NOTE: The tag instance provided here is always empty by default.
		public override void SaveWorldData(TagCompound tag)
		{
			if (downedEoCBoss)
			{
				tag["downedEoCBoss"] = true;
			}
			if (downedBoCBoss)
			{
				tag["downedBoCBoss"] = true;
			}
			if (downedEoWBoss)
			{
				tag["downedEoWBoss"] = true;
			}
			if (downedDeerclopsMiniBoss)
			{
				tag["downedDeerclopsMiniBoss"] = true;
			}
			if (downedQueenBeeBoss)
			{
				tag["downedQueenBeeBoss"] = true;
			}
			if (downedSkeletronBoss)
			{
				tag["downedSkeletronBoss"] = true;
			}
			if (downedConstitutionBoss)
			{
				tag["downedConstitutionBoss"] = true;
			}
			if (downedWallBoss)
			{
				tag["downedWallBoss"] = true;
			}
			if (downedQueenSlimeBoss)
			{
				tag["downedQueenSlimeBoss"] = true;
			}
			if (downedDestroyerBoss)
			{
				tag["downedDestroyerBoss"] = true;
			}
			if (downedTwinsBoss)
			{
				tag["downedTwinsBoss"] = true;
			}
			if (downedSkeletronPrimeBoss)
			{
				tag["downedPrimeBoss"] = true;
			}
			if (downedNautilusMiniBoss)
			{
				tag["downedNautilusMiniBoss"] = true;
			}
			if (downedPlanteraBoss)
			{
				tag["downedPlanteraBoss"] = true;
			}
			if (downedGolemBoss)
			{
				tag["downedGolemBoss"] = true;
			}
			if (downedFishronBoss)
			{
				tag["downedFishronBoss"] = true;
			}
			if (downedEmpressBoss)
			{
				tag["downedEmpressBoss"] = true;
			}
			if (downedCultistBoss)
			{
				tag["downedCultistBoss"] = true;
			}
			if (downedLunarBoss)
			{
				tag["downedLunarBoss"] = true;
			}
			if (downedWyvernCorpseBoss)
			{
				tag["downedWyvernCorpseBoss"] = true;
			}
			if (downedNightmareRoseBoss) {
				tag["downedNightmareRoseBoss"] = true;
			}
		}

		public override void LoadWorldData(TagCompound tag)
		{
			downedEoCBoss = tag.ContainsKey("downedEoCBoss");
			downedBoCBoss = tag.ContainsKey("downedBoCBoss");
			downedEoWBoss = tag.ContainsKey("downedEoWBoss");
			downedDeerclopsMiniBoss = tag.ContainsKey("downedDeerclopsMiniBoss");
			downedQueenBeeBoss = tag.ContainsKey("downedQueenBeeBoss");
			downedSkeletronBoss = tag.ContainsKey("downedSkeletronBoss");
			downedConstitutionBoss = tag.ContainsKey("downedConstitutionBoss");
			downedWallBoss = tag.ContainsKey("downedWallBoss");
			downedQueenSlimeBoss = tag.ContainsKey("downedQueenSlimeBoss");
			downedDestroyerBoss = tag.ContainsKey("downedDestroyerBoss");
			downedTwinsBoss = tag.ContainsKey("downedTwinsBoss");
			downedSkeletronPrimeBoss = tag.ContainsKey("downedPrimeBoss");
			downedNautilusMiniBoss = tag.ContainsKey("downedNautilusMiniBoss");
			downedPlanteraBoss = tag.ContainsKey("downedPlanteraBoss");
			downedGolemBoss = tag.ContainsKey("downedGolemBoss");
			downedFishronBoss = tag.ContainsKey("downedFishronBoss");
			downedEmpressBoss = tag.ContainsKey("downedEmpressBoss");
			downedCultistBoss = tag.ContainsKey("downedCultistBoss");
			downedLunarBoss = tag.ContainsKey("downedLunarBoss");
			downedWyvernCorpseBoss = tag.ContainsKey("downedWyvernCorpseBoss");
			downedNightmareRoseBoss = tag.ContainsKey("downedNightmareRoseBoss");
		}

		public override void NetSend(BinaryWriter writer) {
			// Order of parameters is important and has to match that of NetReceive
			writer.WriteFlags(
				downedEoCBoss, 
				downedKingSlimeBoss, 
				downedBoCBoss, 
				downedEoWBoss, 
				downedQueenBeeBoss, 
				downedDeerclopsMiniBoss, 
				downedSkeletronBoss, 
				downedConstitutionBoss
				
			);
			writer.WriteFlags(
				downedWallBoss,
				downedQueenSlimeBoss, 
				downedDestroyerBoss, 
				downedTwinsBoss, 
				downedSkeletronPrimeBoss, 
				downedNautilusMiniBoss, 
				downedPlanteraBoss, 
				downedGolemBoss
			);
			writer.WriteFlags(
				downedFishronBoss,
				downedEmpressBoss, 
				downedCultistBoss, 
				downedLunarBoss,
				downedWyvernCorpseBoss,
				downedNightmareRoseBoss
			);
			// WriteFlags supports up to 8 entries, if you have more than 8 flags to sync, call WriteFlags again.

			// If you need to send a large number of flags, such as a flag per item type or something similar, BitArray can be used to efficiently send them. See Utils.SendBitArray documentation.
		}

		public override void NetReceive(BinaryReader reader) {
			// Order of parameters is important and has to match that of NetSend
			reader.ReadFlags(
				out downedEoCBoss, 
				out downedKingSlimeBoss, 
				out downedBoCBoss, 
				out downedEoWBoss, 
				out downedQueenBeeBoss, 
				out downedDeerclopsMiniBoss, 
				out downedSkeletronBoss, 
				out downedConstitutionBoss
			);
			reader.ReadFlags(
				out downedWallBoss,
				out downedQueenSlimeBoss, 
				out downedDestroyerBoss, 
				out downedTwinsBoss, 
				out downedSkeletronPrimeBoss, 
				out downedNautilusMiniBoss, 
				out downedPlanteraBoss, 
				out downedGolemBoss
			);
			reader.ReadFlags(
				out downedFishronBoss,
				out downedEmpressBoss, 
				out downedCultistBoss, 
				out downedLunarBoss,
				out downedWyvernCorpseBoss,
				out downedNightmareRoseBoss
			);
			// ReadFlags supports up to 8 entries, if you have more than 8 flags to sync, call ReadFlags again.
		}
	}

	
}