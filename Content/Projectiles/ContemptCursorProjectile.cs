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
using DestroyerTest.Content.Magic;
using InnoVault.PRT;
using DestroyerTest.Content.Particles;
using System.Text;

namespace DestroyerTest.Content.Projectiles
{
    public class ContemptCursorProjectile : ModProjectile
    {



        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.penetrate = -1; // Infinite pierce
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180; // 10 seconds max lifespan
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.netUpdate = true;
            Projectile.alpha = 255;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            sb.End(); // End vanilla drawing
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, 
                    DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            DrawSigil(sb);

            sb.End(); // End additive
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                    DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public void DrawSigil(SpriteBatch sb)
        {
            Texture2D glowTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Extras/CorruptSigil").Value;

            Main.EntitySpriteDraw(
                glowTexture,
                Projectile.Center - Main.screenPosition,
                null,
                ColorLib.CursedFlames,
                Projectile.rotation,
                glowTexture.Size() / 2,
                Projectile.scale * 0.4f,
                SpriteEffects.None,
                0
            );
        }


        public bool HasSpawned = false;

        public Vector2 CurrentCenter;

        public float ProjSpawnTimer = 0;

        public override void AI()
        {
            ProjSpawnTimer++;
            Player player = Main.player[Projectile.owner];
            if (!player.channel || player.dead || player.CCed)
            {
                Projectile.Kill(); // Stop when player stops channeling, dies, or is crowd controlled
                return;
            }

            CurrentCenter = Projectile.Center;

            
          

            if (player.HeldItem.type == ModContent.ItemType<Contempt>() && player.channel)
            {

                Projectile.timeLeft = 120;
                Projectile.Center = Main.MouseWorld;
                if (HasSpawned == false)
                {
                    PRTLoader.NewParticle(PRTLoader.GetParticleID<RuneCircle1>(), Projectile.Center, Projectile.velocity, ColorLib.CursedFlames, 0.4f);
                    HasSpawned = true;
                }




                float rad = 1000;
                Vector2 Spawn = Projectile.Center + Main.rand.NextVector2CircularEdge(rad, rad);
                Vector2 toOrigin = CurrentCenter - Spawn;
                toOrigin = toOrigin.SafeNormalize(Vector2.UnitY); // fallback to downwards if zero

                if (ProjSpawnTimer >= 30)
                {
                    player.statMana -= player.HeldItem.mana;
                    SoundEngine.PlaySound(SoundID.Item20);

                    for (int a = 0; a < 12; a++)
                    {
                        Spawn = Projectile.Center + Main.rand.NextVector2CircularEdge(rad, rad);
                        toOrigin = CurrentCenter - Spawn;
                        toOrigin = toOrigin.SafeNormalize(Vector2.UnitY);
                        Projectile Flames = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), Spawn, toOrigin * 20f, ModContent.ProjectileType<CursedFlamesFriendly>(), 40, 2, Main.LocalPlayer.whoAmI);
                        if (Flames.Center == Projectile.Center)
                        {
                            Flames.Kill();
                        }
                    }
                    PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp1>(), Projectile.Center, Projectile.velocity, ColorLib.CursedFlames, 0.4f);
                    ProjSpawnTimer = 0;
                }



            }


        }






        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
          
           
        }

       

    }

}

