using System.IO;
using DestroyerTest.Common;
using DestroyerTest.Content.Consumables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Common.Systems
{
    public class CursedStarProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.FallingStar);
            Projectile.width = 68;
            Projectile.height = 96;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }

        private void AnimateProjectile()
        {
            // Loop through the frames, assuming each frame lasts 5 ticks
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    
        public float ItemTexRot;

        public override void AI()
        {
            AnimateProjectile();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            ItemTexRot = Projectile.rotation += 0.4f * Projectile.direction;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, Projectile.velocity * 0.2f, 100, ColorLib.StellarColor, 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 1.5f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.NewText("A suspicious star has invaded from the heavens...", ColorLib.StellarColor);
            Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y - 2000), new Vector2(0, -1), ProjectileID.DD2ExplosiveTrapT3Explosion, 18, 5);
            Item.NewItem(Entity.GetSource_FromThis(), Projectile.getRect(), ModContent.ItemType<CursedStar>(), 1, true, 0);
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {


            lightColor = ColorLib.StellarColor;
            SpriteBatch SB = Main.spriteBatch;
            Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D ItemTexture = TextureAssets.Item[75].Value;


            SB.End();
            SB.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            int frameHeight = projectileTexture.Height / Main.projFrames[Projectile.type];
            Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, projectileTexture.Width, frameHeight);

            Main.EntitySpriteDraw(
                projectileTexture,
                Projectile.Center - Main.screenPosition,
                sourceRect,
                lightColor,
                Projectile.rotation,
                new Vector2(projectileTexture.Width / 2f, frameHeight / 2f),
                Projectile.scale,
                SpriteEffects.None,
                0
            );
            SB.End();
            SB.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(ItemTexture, Projectile.Center - Main.screenPosition, null, Color.Black, ItemTexRot, new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}