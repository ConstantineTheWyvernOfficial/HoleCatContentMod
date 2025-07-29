
using System.IO;
using DestroyerTest.Content.Consumables;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DestroyerTest.Common
{
	public class GoodOmenPlayer : ModPlayer
	{
		public int Omens;
		

		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			health = StatModifier.Default;
			health.Base = Omens * GoodOmen.LifePerFruit;
			// Alternatively:  health = StatModifier.Default with { Base = exampleLifeFruits * ExampleLifeFruit.LifePerFruit };
			mana = StatModifier.Default;
			//mana.Base = exampleManaCrystals * ExampleManaCrystal.ManaPerCrystal;
			// Alternatively:  mana = StatModifier.Default with { Base = exampleManaCrystals * ExampleManaCrystal.ManaPerCrystal };
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)Player.whoAmI);
			packet.Write((byte)Omens);
			//packet.Write((byte)exampleManaCrystals);
			packet.Send(toWho, fromWho);
		}

		// Called in ExampleMod.Networking.cs
		public void ReceivePlayerSync(BinaryReader reader) {
			Omens = reader.ReadByte();
			//exampleManaCrystals = reader.ReadByte();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			GoodOmenPlayer clone = (GoodOmenPlayer)targetCopy;
			clone.Omens = Omens;
			clone.Omens = Omens;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			GoodOmenPlayer clone = (GoodOmenPlayer)clientPlayer;

			if (Omens != clone.Omens) {
				// This example calls SyncPlayer to send all the data for this ModPlayer when any change is detected, but if you are dealing with a large amount of data you should try to be more efficient and use custom packets to selectively send only specific data that has changed.
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
			}
		}

		// NOTE: The tag instance provided here is always empty by default.
		// Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
		public override void SaveData(TagCompound tag) {
			tag["Omens"] = Omens;
			//tag["exampleManaCrystals"] = exampleManaCrystals;
		}

		public override void LoadData(TagCompound tag) {
			Omens = tag.GetInt("Omens");
			//exampleManaCrystals = tag.GetInt("exampleManaCrystals");
		}
	}
}