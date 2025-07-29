using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.RiftBiome.RiftDesertResources;
using DestroyerTest.Content.RiftBiome.RiftSurfaceResources;

namespace DestroyerTest.Content.RiftBiomeSpread
{
	public class RiftSolution : ModItem
	{
		public override string Texture => "DestroyerTest/Content/RiftBiomeSpread/RiftSolution";

		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override void SetDefaults() {
			Item.DefaultToSolution(ModContent.ProjectileType<RiftSolutionProjectile>());
			Item.value = Item.buyPrice(0, 0, 25);
			Item.rare = ModContent.RarityType<RiftRarity1>();
			//Item.ammo = Item.type;
			Item.maxStack = Item.CommonMaxStack;
			//Item.ammo = ModContent.ItemType<RiftSolution>();
		}

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) {
			itemGroup = ContentSamples.CreativeHelper.ItemGroup.Solutions;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Living_Shadow>()
				.AddIngredient(ItemID.BottledWater)
				.AddTile(TileID.Solidifier)
				.Register();
		}
	}

	public class RiftSolutionProjectile : ModProjectile
	{
		public override string Texture => "DestroyerTest/Content/Projectiles/RiftSolutionProjectile";

		public ref float Progress => ref Projectile.ai[0];

		public override void SetDefaults() {
			// This method quickly sets the projectile properties to match other sprays.
			Projectile.DefaultToSpray();
			Projectile.aiStyle = 0; // Here we set aiStyle back to 0 because we have custom AI code
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
		}
		


		public override void AI() {
			// Set the dust type to ExampleSolution
			int dustType = ModContent.DustType<Dusts.RiftDust>();

			

			if (Projectile.owner == Main.myPlayer) {
				Convert((int)(Projectile.position.X + (Projectile.width * 0.5f)) / 16, (int)(Projectile.position.Y + (Projectile.height * 0.5f)) / 16, 2);
			}

			if (Projectile.timeLeft > 133) {
				Projectile.timeLeft = 133;
			}

			if (Progress > 7f) {
				float dustScale = 1f;

				if (Progress == 8f) {
					dustScale = 0.2f;
				}
				else if (Progress == 9f) {
					dustScale = 0.4f;
				}
				else if (Progress == 10f) {
					dustScale = 0.6f;
				}
				else if (Progress == 11f) {
					dustScale = 0.8f;
				}

				Progress += 1f;


				var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

				dust.noGravity = true;
				dust.scale *= 1.75f;
				dust.velocity.X *= 2f;
				dust.velocity.Y *= 2f;
				dust.scale *= dustScale;
			}
			else {
				Progress += 1f;
			}

			Projectile.rotation += 0.3f * Projectile.direction;
		}

		private static void Convert(int i, int j, int size = 4) {
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt((size * size) + (size * size))) {
						
						if (!WorldGen.InWorld(k, l, 0)) continue; // Ensures we stay in bounds

						if (k < 0 || k >= Main.maxTilesX || l < 0 || l >= Main.maxTilesY)
    continue;
						
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;

						// Convert all walls to ExampleWall (or ExampleWallUnsafe for SpiderUnsafe)
						if (wall != 0 && wall != ModContent.WallType<Wall_DangerousRiftWall>()) {
							if (wall == WallID.SpiderUnsafe)
								Main.tile[k, l].WallType = (ushort)ModContent.WallType<Wall_DangerousRiftWall>();
							else
								Main.tile[k, l].WallType = (ushort)ModContent.WallType<Wall_RiftWall>();
							WorldGen.SquareWallFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						// If the tile is stone, convert to ExampleBlock
						if (TileID.Sets.Conversion.Stone[type]) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tile_RiftStone>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						// If the tile is sand, convert to ExampleSand
						else if (TileID.Sets.Conversion.Sand[type]) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tile_RiftSilt>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						// If the tile is sand, convert to ExampleSand
						else if (TileID.Sets.Conversion.Dirt[type]) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tile_RiftDirt>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						// If the tile is sand, convert to ExampleSand
						else if (TileID.Sets.Conversion.Grass[type]) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tile_RiftDirt>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						// If the tile is sand, convert to ExampleSand
						else if (TileID.Sets.Conversion.HardenedSand[type]) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tile_HardenedRiftSilt>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						// If the tile is sand, convert to ExampleSand
						else if (TileID.Sets.Conversion.Sandstone[type]) {
							Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tile_RiftSiltStone>();
							WorldGen.SquareTileFrame(k, l);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (WallID.Sets.Conversion.HardenedSand[wall]) { 
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Wall_RiftSiltWallUnsafe>();
						}
						else if (WallID.Sets.Conversion.Sandstone[wall]) { 
							Main.tile[k, l].WallType = (ushort)ModContent.WallType<Wall_RiftSiltStoneWall>();
						}
					}
				}
			}
		}
	}
}