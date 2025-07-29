using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;

namespace DestroyerTest.Content.Projectiles
{
    public class RiftScytheEnergy : ModProjectile
    {

        public int TileCollisions = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.Magic;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

    

        public override void AI()
        {
            Projectile.velocity *= 0.9f;

            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha -= 5;
            }
            
            

            
            Player player = Main.player[Projectile.owner];

            // Always spinning
            Projectile.rotation += 0.4f * Projectile.direction;

              // Generate flying dust effect
            if (Main.rand.NextBool(3)) // 33% chance per tick
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Lava, Projectile.velocity * 0.2f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            
        }






        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DaylightOverloadFriendly>(), 120);
            SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
           
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            SoundStyle Break = new SoundStyle("DestroyerTest/Assets/Audio/TO_Break") with
            {
            PitchVariance = 0.5f
            };
            // Play impact sound and spawn tile hit effects
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            TileCollisions += 1;
            if (TileCollisions > 5)
            {
                SoundEngine.PlaySound(Break);
                Projectile.Kill();
                TileCollisions = 0;
            }

            // Create a burst of dust on impact
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.position, DustID.Lava, oldVelocity.RotatedByRandom(MathHelper.PiOver4) * 0.5f, 150, default, 1.5f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }

            return false; // Prevents the projectile from being destroyed on collision
        }

    }

}

