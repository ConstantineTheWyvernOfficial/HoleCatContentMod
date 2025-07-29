using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using Terraria.DataStructures;
using System.IO;
using DestroyerTest.Content.Projectiles.ParentClasses;

namespace DestroyerTest.Content.Projectiles
{
    public class NatureScepterThrown : ThrownScepter
    {
        public override void SetDefaults()
        {
            ThemeColor = Color.White;
            WidthDim = 34;
            HeightDim = 34;
            DustType = DustID.Glass;
            base.SetDefaults();
        }

        public override void AI()
        {


            if (Main.rand.NextBool(3)) // 33% chance per tick
            {

                Projectile newProjectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(),
                Projectile.Center, Vector2.Zero, ProjectileID.SporeGas, 16, 2, Main.LocalPlayer.whoAmI);
                newProjectile.friendly = true; // If it shouldn't harm the player, for example
            }
            base.AI();
        }


        public override bool OnTileCollide(Vector2 oldVelocity) {
            
            // Play impact sound and spawn tile hit effects
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
           

            // Create a burst of dust on impact
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.position, DustID.Chlorophyte, oldVelocity.RotatedByRandom(MathHelper.PiOver4) * 0.5f, 150, default, 1.5f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }


            base.OnTileCollide(oldVelocity);
            return false; // Prevents the projectile from being destroyed on collision
        }

    }
}

