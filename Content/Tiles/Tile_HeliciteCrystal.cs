using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace DestroyerTest.Content.Tiles
{
	public class Tile_HeliciteCrystal : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			TileID.Sets.BlockMergesWithMergeAllBlock[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 410; // Metal Detector value, see https://terraria.wiki.gg/wiki/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(255, 155, 0), name);

			DustType = 84;
			HitSound = SoundID.Shatter;
			//MineResist = 4f;
			MinPick = 200;
		}

		// Example of how to enable the Biome Sight buff to highlight this tile. Biome Sight is technically intended to show "infected" tiles, so this example is purely for demonstration purposes.
		public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor)
		{
			sightColor = Color.Blue;
			return true;
		}
	}

	// ExampleOreSystem contains code related to spawning ExampleOre. It contains both spawning ore during world generation, seen in ModifyWorldGenTasks, and spawning ore after defeating a boss, seen in BlessWorldWithExampleOre and MinionBossBody.OnKill.
	public class HeliciteSystem : ModSystem
	{
		public static LocalizedText HelicitePassMessage { get; private set; }
		public static LocalizedText BlessedWithHeliciteMessage { get; private set; }

		public override void SetStaticDefaults()
		{
			HelicitePassMessage = Mod.GetLocalization($"WorldGen.{nameof(HelicitePassMessage)}");
			BlessedWithHeliciteMessage = Mod.GetLocalization($"WorldGen.{nameof(BlessedWithHeliciteMessage)}");
		}

		// This method is called from MinionBossBody.OnKill the first time the boss is killed.
		// The logic is located here for organizational purposes.
		public void BlessWorldWithHelicite()
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return; // This should not happen, but just in case.
			}

			// Since this happens during gameplay, we need to run this code on another thread. If we do not, the game will experience lag for a brief moment. This is especially necessary for world generation tasks that would take even longer to execute.
			// See https://github.com/tModLoader/tModLoader/wiki/World-Generation/#long-running-tasks for more information.
			ThreadPool.QueueUserWorkItem(_ =>
			{
				// Broadcast a message to notify the user.
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.NewText(BlessedWithHeliciteMessage.Value, 255, 155, 0);
				}
				else if (Main.netMode == NetmodeID.Server)
				{
					ChatHelper.BroadcastChatMessage(BlessedWithHeliciteMessage.ToNetworkText(), new Color(255, 155, 0));
				}



				// 100 controls how many splotches of ore are spawned into the world, scaled by world size. For comparison, the first 3 times altars are smashed about 275, 190, or 120 splotches of the respective hardmode ores are spawned. 
				int splotches = (int)(40 * (Main.maxTilesX / 4200f)); // was 100, now safer
				int highestY = (int)Utils.Lerp(Main.rockLayer, Main.UnderworldLayer, 0.5);
				for (int iteration = 0; iteration < splotches; iteration++)
				{
					Mod.Logger.Info($"[HeliciteGen] Placing {splotches} splotches");
					// Find a point in the lower half of the rock layer but above the underworld depth.
					int i = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
					int j = WorldGen.genRand.Next(highestY, Main.UnderworldLayer);

					// OreRunner will spawn ExampleOre in splotches. OnKill only runs on the server or single player, so it is safe to run world generation code.
					GenerateBezierVein(new Vector2(i, j), (ushort)ModContent.TileType<Tile_HeliciteCrystal>(), 400f, 400f);


				}
			});
		}

		public void GenerateBezierVein(Vector2 start, ushort tileType, float maxLength, float maxWidth)
		{
			Vector2 end = start + new Vector2(WorldGen.genRand.NextFloat(-maxLength * 0.1f, maxLength * 0.1f), WorldGen.genRand.NextFloat(-maxLength, maxLength));
			Vector2 ctrl1 = Vector2.Lerp(start, end, 0.33f) + new Vector2(WorldGen.genRand.NextFloat(-maxLength * 0.05f, maxLength * 0.05f), WorldGen.genRand.NextFloat(-maxLength * 0.2f, maxLength * 0.2f));
			Vector2 ctrl2 = Vector2.Lerp(start, end, 0.66f) + new Vector2(WorldGen.genRand.NextFloat(-maxLength * 0.05f, maxLength * 0.05f), WorldGen.genRand.NextFloat(-maxLength * 0.2f, maxLength * 0.2f));

			int tileLimit = 400; // hard cap per vein
			int placedTiles = 0;

			for (float t = 0f; t <= 1f && placedTiles < tileLimit; t += 0.04f) // doubled step
			{
				Vector2 pos = CubicBezier(start, ctrl1, ctrl2, end, t);
				float width = maxWidth * (float)Math.Sin(t * Math.PI);
				float height = width * 0.4f;

				int xRad = (int)Math.Ceiling(width);
				int yRad = (int)Math.Ceiling(height);

				for (int dx = -xRad; dx <= xRad; dx++)
				{
					for (int dy = -yRad; dy <= yRad; dy++)
					{
						if (placedTiles >= tileLimit) break;

						float normX = dx / width;
						float normY = dy / height;

						if (normX * normX + normY * normY <= 1f)
						{
							int tileX = (int)(pos.X + dx);
							int tileY = (int)(pos.Y + dy);

							if (WorldGen.InWorld(tileX, tileY) && Main.tile[tileX, tileY] != null)
							{
								WorldGen.PlaceTile(tileX, tileY, tileType, true, true);
								placedTiles++;
							}
						}
					}
				}
			}
		}


		public Vector2 CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
		{
			float u = 1 - t;
			return u * u * u * p0 +
				3 * u * u * t * p1 +
				3 * u * t * t * p2 +
				t * t * t * p3;
		}

		public class HelicitePass : GenPass
		{

			public HelicitePass(string name, float loadWeight) : base(name, loadWeight)
			{
			}

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				// progress.Message is the message shown to the user while the following code is running.
				// Try to make your message clear. You can be a little bit clever, but make sure it is descriptive enough for troubleshooting purposes.
				progress.Message = HeliciteSystem.HelicitePassMessage.Value;

				// Ores are quite simple, we simply use a for loop and the WorldGen.TileRunner to place splotches of the specified Tile in the world.
				// "6E-05" is "scientific notation". It simply means 0.00006 but in some ways is easier to read.
				int splotches = (int)(40 * (Main.maxTilesX / 4200f)); // or fewer for performance

				for (int k = 0; k < splotches; k++)
				{
					int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
					int y = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, Main.UnderworldLayer);

					GenerateBezierVein(new Vector2(x, y), (ushort)ModContent.TileType<Tile_HeliciteCrystal>(), 400f, 400f);
				}

			}

			public void GenerateBezierVein(Vector2 start, ushort tileType, float maxLength, float maxWidth)
			{
				Vector2 end = start + new Vector2(WorldGen.genRand.NextFloat(-maxLength * 0.1f, maxLength * 0.1f), WorldGen.genRand.NextFloat(-maxLength, maxLength));
				Vector2 ctrl1 = Vector2.Lerp(start, end, 0.33f) + new Vector2(WorldGen.genRand.NextFloat(-maxLength * 0.05f, maxLength * 0.05f), WorldGen.genRand.NextFloat(-maxLength * 0.2f, maxLength * 0.2f));
				Vector2 ctrl2 = Vector2.Lerp(start, end, 0.66f) + new Vector2(WorldGen.genRand.NextFloat(-maxLength * 0.05f, maxLength * 0.05f), WorldGen.genRand.NextFloat(-maxLength * 0.2f, maxLength * 0.2f));

				int tileLimit = 400; // hard cap per vein
				int placedTiles = 0;

				for (float t = 0f; t <= 1f && placedTiles < tileLimit; t += 0.04f) // doubled step
				{
					Vector2 pos = CubicBezier(start, ctrl1, ctrl2, end, t);
					float width = maxWidth * (float)Math.Sin(t * Math.PI);
					float height = width * 0.4f;

					int xRad = (int)Math.Ceiling(width);
					int yRad = (int)Math.Ceiling(height);

					for (int dx = -xRad; dx <= xRad; dx++)
					{
						for (int dy = -yRad; dy <= yRad; dy++)
						{
							if (placedTiles >= tileLimit) break;

							float normX = dx / width;
							float normY = dy / height;

							if (normX * normX + normY * normY <= 1f)
							{
								int tileX = (int)(pos.X + dx);
								int tileY = (int)(pos.Y + dy);

								if (WorldGen.InWorld(tileX, tileY) && Main.tile[tileX, tileY] != null)
								{
									WorldGen.PlaceTile(tileX, tileY, tileType, true, true);
									placedTiles++;
								}
							}
						}
					}
				}
			}


			public Vector2 CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
			{
				float u = 1 - t;
				return u * u * u * p0 +
					3 * u * u * t * p1 +
					3 * u * t * t * p2 +
					t * t * t * p3;
			}


		}
	}
}