using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using DestroyerTest.Content.Dusts;
using System;
using Terraria.DataStructures;
using System.IO;
using DestroyerTest.Content.Projectiles.ParentClasses;

namespace DestroyerTest.Content.Projectiles
{
    public class ChlorophyteScepterThrown : ThrownScepter
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
            Player player = Main.player[Projectile.owner];
              // Generate flying dust effect
            if (Main.rand.NextBool(3)) // 33% chance per tick
            {
                
    
                 // Create AmbientSpore projectile at the same position but with zero velocity
                Projectile newProjectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), 
                Projectile.Center, Vector2.Zero, ProjectileID.SporeGas, 30, 0, player.whoAmI);
                newProjectile.friendly = true; // If it shouldn't harm the player, for example
            }
            if (Main.rand.NextBool(6) && player.statLife == player.statLifeMax) // 33% chance per tick
            {
               
                Projectile newProjectile2 = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), 
                Projectile.Center, Projectile.velocity, ModContent.ProjectileType<NatureShot>(), 15, 0, player.whoAmI);
                newProjectile2.friendly = true; // If it shouldn't harm the player, for example
                
            }
            base.AI();
        }



        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            target.AddBuff(BuffID.Poisoned, 120);

            Vector2 launchVelocity = new Vector2(-8, 0); // Create a velocity moving the left.
            for (int i = 0; i < 8; i++)
            {
                // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ProjectileID.SporeGas, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            

            Vector2 launchVelocity = new Vector2(-8, 0); // Create a velocity moving the left.
                for (int i = 0; i < 8; i++) {
                    // Every iteration, rotate the newly spawned projectile by the equivalent 1/4th of a circle (MathHelper.PiOver4)
                    // (Remember that all rotation in Terraria is based on Radians, NOT Degrees!)
                    launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);

                    // Spawn a new projectile with the newly rotated velocity, belonging to the original projectile owner. The new projectile will inherit the spawning source of this projectile.
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, launchVelocity, ProjectileID.SporeGas, Projectile.damage, Projectile.knockBack, Projectile.owner);
                }


            base.OnTileCollide(oldVelocity);

            return false; // Prevents the projectile from being destroyed on collision
        }

    }
}

