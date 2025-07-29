using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Entity;
using DestroyerTest.Content.Particles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
    public class NightmarePowderProjectile : ModProjectile
    {


        public override void SetDefaults()
        {
            Projectile.width = 80; // The width of projectile hitbox
            Projectile.height = 80; // The height of projectile hitbox
            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 1f; // How much light emit around the projectile
            Projectile.timeLeft = 60; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
            Projectile.penetrate = 2;
        }



        public override void AI()
        {

            int[] types = new int[]
                {
                PRTLoader.GetParticleID<BlackFire1>(),
                PRTLoader.GetParticleID<BlackFire2>(),
                PRTLoader.GetParticleID<BlackFire3>(),
                PRTLoader.GetParticleID<BlackFire4>(),
                PRTLoader.GetParticleID<BlackFire5>(),
                PRTLoader.GetParticleID<BlackFire6>(),
                PRTLoader.GetParticleID<BlackFire7>()
                };

            PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], Projectile.Center, Vector2.Zero, default, 0.3f);



        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Get the tile coordinates at the projectile's position
            int i = (int)(Projectile.Center.X / 16f);
            int j = (int)(Projectile.Center.Y / 16f);

            // Check if the tile is a Deathweed Herb
            ushort tileType = Main.tile[i, j].TileType;
            if (tileType == TileID.Plants && Main.tile[i, j].TileFrameX == 90 * 18)
            {
                // Example: spawn an NPC at the projectile's position (replace NPCID.Bunny with your desired NPC)
                NPC.NewNPC(Entity.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<NightmareRoseBoss>());
                WorldGen.KillTile(i, j, false, false, true);
            }

            return true; // Return true to destroy the projectile on tile collision
        }


	}

}