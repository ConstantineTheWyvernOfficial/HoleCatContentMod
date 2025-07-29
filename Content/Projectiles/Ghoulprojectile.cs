using DestroyerTest.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using DestroyerTest.Content.Buffs;

namespace DestroyerTest.Content.Projectiles
{
public class GhoulProjectile : ModProjectile
		{


			
			public override void SetStaticDefaults() {
				ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
			}

		public override void SetDefaults()
		{
			Projectile.width = 16; // The width of projectile hitbox
			Projectile.height = 16; // The height of projectile hitbox

			Projectile.DamageType = ModContent.GetInstance<ScepterClass>();
			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.light = 1f; // How much light emit around the projectile
			Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.tileCollide = false;
			Projectile.penetrate = 2;
			Projectile.netImportant = true;
			Projectile.netUpdate = true;
			}
		
			// Custom AI
			public override void AI() {

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;


			

				// If found, we rotate the projectile velocity in the direction of the target.
				// We only rotate by 3 degrees an update to give it a smooth trajectory. Increase the rotation speed here to make tighter turns
				float length = Projectile.velocity.Length();
				float targetAngle = Projectile.AngleTo(Main.MouseWorld);
				Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(10)).ToRotationVector2() * length;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                if (Projectile.Center.Distance(Main.MouseWorld) < 20f)
                {
                    Projectile.velocity *= 0.5f;
                }

                Projectile.scale *= 0.99f;

                if (Projectile.scale < 0.1f)
                {
                    Projectile.Kill();
                }

                //Projectile.rotation += 0.4f * Projectile.direction;

			}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff(ModContent.BuffType<PowerTrade>(), 1200);
        }

    }
	
}