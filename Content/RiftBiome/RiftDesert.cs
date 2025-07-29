using DestroyerTest.Common.Systems;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Resources.Cloths;
using DetroyerTest.Content.RiftBiome;
using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Events;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RiftBiome
{
	// Shows setting up two basic biomes. For a more complicated example, please request.
	public class RiftDesert : ModBiome
	{
		// Select all the scenery
		public override ModWaterStyle WaterStyle => ModContent.GetInstance<RiftWaterStyle>(); // Sets a water style for when inside this biome
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<RiftDesertBackgroundStyle>();
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;
        // Select Music

        public override int Music
		{
			get
			{
				if (Main.eclipse == true)
				{
					return MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftEvent");
				}
				if (Main.bloodMoon == true)
				{
					return MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftEvent");
				}
				if (Main.snowMoon == true)
				{
					return MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftEvent");
				}
				if (Main.pumpkinMoon == true)
				{
					return MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftEvent");
				}
				if (Main.getGoodWorld == true)
				{
					return MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftEvent");
				}
                if (Sandstorm.Happening)
                {
                    return MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftEmptiness");
                }
				else
				{
					return MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftV1");
				}
				
			}
		}

       public void SandstormSkyFX()
        {
            if (Sandstorm.Happening)
            {
                Main.moonPhase = 4; // Hide the moon
                SkyManager.Instance.Activate("MyMod:RiftDarkSky"); // Enable the black sky
            }
            else
            {
                SkyManager.Instance.Deactivate("MyMod:RiftDarkSky"); // Restore normal sky
            }
        }

		public void ModifyMusic(ref int music, ref SceneEffectPriority priority)
		{
			if (Main.snowMoon || Main.pumpkinMoon)
			{
				FieldInfo eventMusicField = typeof(Main).GetField("curMusic", BindingFlags.NonPublic | BindingFlags.Static);
				eventMusicField?.SetValue(null, MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftEvent"));

				priority = SceneEffectPriority.BossHigh;
			}
		}


		// public override int BiomeTorchItemType => ModContent.ItemType<ExampleTorch>();
		// public override int BiomeCampfireItemType => ModContent.ItemType<ExampleCampfire>();

		// Populate the Bestiary Filter
		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => BackgroundPath; // Re-uses Bestiary Background for Map Background

		public void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface, // Define biome appearance
				new FlavorTextBestiaryInfoElement("A lifeless void, populeted by naught but anerobic bacterium. The reason being all the smalll flames springing up in any crevice where combustible material gathers. Living shadows are very unstable, and letting them encase the land was likely never intended by nature.") // Modify description
			});
		}

		// Calculate when the biome is active.
		public override bool IsBiomeActive(Player player) {
			// First, we will use the exampleBlockCount from our added ModSystem for our first custom condition
			bool b1 = ModContent.GetInstance<RiftDesertTileCount>().RiftDesertBlockCount >= 15;

			// Second, we will limit this biome to the inner horizontal third of the map as our second custom condition
			//bool b2 = Math.Abs(player.position.ToTileCoordinates().X - Main.maxTilesX / 2) < Main.maxTilesX / 6;

			// Finally, we will limit the height at which this biome can be active to above ground (ie sky and surface). Most (if not all) surface biomes will use this condition.
			bool b3 = player.ZoneSkyHeight || player.ZoneOverworldHeight;

            if (b1 && b3 && !player.HasBuff<StoneLungs>() && !player.HasBuff<AirSeal>()) {
				player.AddBuff(BuffID.Suffocation, 360); // Apply the suffocation buff if all conditions are met
			    }

			return b1 && b3;
		}

		// Declare biome priority. The default is BiomeLow so this is only necessary if it needs a higher priority.
		public override SceneEffectPriority Priority
		{
			get
			{
				if (Main.eclipse || Main.bloodMoon || Main.snowMoon || Main.pumpkinMoon)
				{
					return SceneEffectPriority.BossMedium; // BossHigh is stronger than Event priority
				}
				if (Main.snowMoon || Main.pumpkinMoon)
				{
					return SceneEffectPriority.BossHigh; // BossHigh is stronger than Event priority
				}
				return SceneEffectPriority.BiomeMedium; // Normal priority otherwise
			}
		}
	}
}