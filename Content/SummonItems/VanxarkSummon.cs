
using DestroyerTest.Content.SummonItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.SummonItems
{
	public class VanxarkSummon : ModProjectile
	{

		public override void SetStaticDefaults() {
			if (Projectile.type < Main.projFrames.Length)
            Main.projFrames[Projectile.type] = 8;
    
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.LightPet[Projectile.type] = true;

            ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 8)
				.WithOffset(-10, -20f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults() {
			Projectile.width = 428;
			Projectile.height = 412;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.timeLeft *= 5;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.scale = 0.8f;
			Projectile.tileCollide = false;
            Projectile.hide = true; // Prevent automatic drawing
            Projectile.CloneDefaults(ProjectileID.ZephyrFish); // Copy the stats of the Zephyr Fish
            Projectile.netImportant = true;
			Projectile.netUpdate = true;

			AIType = ProjectileID.ZephyrFish; // Mimic as the Zephyr Fish during AI.
		}

        public void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            // Add this projectile's index to the list that draws behind NPCs and tiles
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }

		public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Despawn the projectile if the player is inactive
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }

            // Keep the projectile from disappearing as long as the player isn't dead and has the buff
            if (!player.dead && player.HasBuff(ModContent.BuffType<VanxarkBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            // Instantly snap the entity to the player's position
            Projectile.Center = player.MountedCenter - new Vector2(Projectile.width / 2, Projectile.height / 2) + new Vector2(-140 -50);
            Projectile.velocity = Vector2.Zero; // Ensure it doesn’t drift

            // Lock rotation to prevent weird spinning (optional)
            Projectile.rotation = 0f;

            // Light emission (only if needed)
            if (!Main.dedServ)
            {
                Lighting.AddLight(Projectile.Center, Projectile.Opacity * 0.7f, Projectile.Opacity * 0.7f, Projectile.Opacity * 0.7f);
            }
        }
	}
}