using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.RiftBiome.RiftDesertResources;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

// This file contains ExampleSandBallProjectile, ExampleSandBallFallingProjectile, and ExampleSandBallGunProjectile.
// ExampleSandBallFallingProjectile and ExampleSandBallGunProjectile inherit from ExampleSandBallProjectile, allowing cleaner code and shared logic.
// ExampleSandBallFallingProjectile is the projectile that spawns when the ExampleSand tile falls.
// ExampleSandBallGunProjectile is the projectile that is shot by the Sandgun weapon.
// Both projectiles share the same aiStyle, ProjAIStyleID.FallingTile, but the AIType line in ExampleSandBallGunProjectile ensures that specific logic of the aiStyle is used for the sandgun projectile.
// It is possible to make a falling projectile not using ProjAIStyleID.FallingTile, but it is a lot of code.
namespace DestroyerTest.Content.Projectiles
{
	public abstract class RiftSiltBallProjectile : ModProjectile
	{
		public override string Texture => "DestroyerTest/Content/Projectiles/RiftSiltBallProjectile";

		public override void SetStaticDefaults() {
			ProjectileID.Sets.FallingBlockDoesNotFallThroughPlatforms[Type] = true;
			ProjectileID.Sets.ForcePlateDetection[Type] = true;
		}
	}

	public class RiftSiltBallFallingProjectile : RiftSiltBallProjectile
	{
		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			ProjectileID.Sets.FallingBlockTileItem[Type] = new(ModContent.TileType<Tile_RiftSilt>(), ModContent.ItemType<Item_RiftSilt>());
		}

		public override void SetDefaults() {
			// The falling projectile when compared to the sandgun projectile is hostile.
			Projectile.CloneDefaults(ProjectileID.EbonsandBallFalling);
		}
	}
}