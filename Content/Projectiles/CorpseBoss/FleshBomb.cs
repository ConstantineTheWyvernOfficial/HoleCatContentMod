using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Projectiles.CorpseBoss;
using DestroyerTest.Content.Projectiles.VampireBoss;
using DestroyerTest.Content.RiftArsenal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles.CorpseBoss
{
    public class FleshBomb : ModProjectile
    {
        public SoundStyle BombPlant = new SoundStyle("DestroyerTest/Assets/Audio/Corpse/FleshBombSpawn") with { PitchVariance = 1.0f };
        public SoundStyle BombBlow = new SoundStyle("DestroyerTest/Assets/Audio/Corpse/FleshBombExplode") with { PitchVariance = 1.0f };

        public override void SetDefaults()
        {
            Projectile.width = 24; // The width of projectile hitbox
            Projectile.height = 24; // The height of projectile hitbox

            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = true; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 1f; // How much light emit around the projectile
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(BombPlant, Projectile.Center);
        }

        public override void AI()
        {
            Vector2 ToPlayer = Projectile.Center - Main.LocalPlayer.Center;
            Projectile.velocity *= 0.999f;
            Projectile.rotation += Main.rand.NextFloat(-1f, 1.1f) * 0.1f;
            Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Blood, 0, 0, 70, default, 1.0f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.moveSpeed *= 0.6f;
        }

        public override void OnKill(int timeLeft)
        {
            Vector2 ToPlayer = Projectile.Center - Main.LocalPlayer.Center;
            SoundEngine.PlaySound(BombBlow, Projectile.Center);
            var launchVelocity = new Vector2(-8, 0); // Create a velocity moving the left.
                
            for (int i = 0; i < 8; i++)
            {
                launchVelocity = launchVelocity.RotatedBy(MathHelper.PiOver4);
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, launchVelocity, ProjectileID.GoldenShowerHostile, 35, 1);
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, launchVelocity, ModContent.ProjectileType<BloodProjectile>(), 35, 1);
            }
        }
    }
}