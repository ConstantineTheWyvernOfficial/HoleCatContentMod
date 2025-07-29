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

namespace DestroyerTest.Content.Projectiles
{
    public class Rift_Katana_Clone : ModProjectile
    {
        

        public override void SetStaticDefaults()
        {
        }

        public Vector2 start;
        public Vector2 control1;
        public Vector2 control2;
        public Vector2 end;

        private float bezierT = 0f;


        public override void SetDefaults()
        {
            Projectile.width = 74;
            Projectile.height = 122;
            Projectile.friendly = true;
            Projectile.penetrate = 1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
        }
        
        public static Vector2 CubicBezier(Vector2 start, Vector2 control1, Vector2 control2, Vector2 end, float t)
		{
			float u = 1 - t;
			return (u * u * u * start) + (3 * u * u * t * control1) + (3 * u * t * t * control2) + (t * t * t * end);
		}



        public override void AI()
        {

            Player player = Main.player[Projectile.owner];



            // Always spinning
            if (player.direction == 1)
            {
                Projectile.rotation += 0.5f * Projectile.direction;
            }
            if (player.direction == -1)
            {
                Projectile.rotation -= 0.5f * Projectile.direction;
            }
            

            // Generate flying dust effect
                if (Main.rand.NextBool(3)) // 33% chance per tick
                {
                    int dustIndex = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.TintableDustLighted, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 0, ColorLib.Rift, 1.2f);
                    Dust dust = Main.dust[dustIndex];
                    dust.noGravity = true;
                    dust.fadeIn = 1.5f;
                    // Generate flying dust effect


                }
            // Update position along the curve
            float speed = 0.02f; // Adjust for faster/slower movement
            bezierT += speed;

            if (bezierT > 1f)
            {
                bezierT = 1f;
                // Optionally kill the projectile or switch to straight homing
            }

            Projectile.Center = CubicBezier(start, control1, control2, end, bezierT);

            // Optional: Rotate the projectile to face the direction of travel
            if (bezierT < 1f)
            {
                Vector2 nextPoint = CubicBezier(start, control1, control2, end, bezierT + speed);
                //Projectile.rotation = (nextPoint - Projectile.Center).ToRotation();
                
            }
        }
    }
}

