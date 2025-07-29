using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DestroyerTest.Common;
using DestroyerTest.Content.MeleeWeapons;
using Terraria.Audio;
using DestroyerTest.Content.Magic;
using DestroyerTest.Content.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;

namespace DestroyerTest.Content.RangedItems
{
    public class StellarBowHoldout : ModProjectile
    {
        private int aiState = 0; // 0 = Lances, 1 = Stars
        private int stateTimer = 0; // Generic timer used in both states

        public override string Texture => "DestroyerTest/Content/RangedItems/StellarBow";
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 200; // persistent
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D whiteOutline = ModContent.Request<Texture2D>("DestroyerTest/Content/RangedItems/StellarBowOutline").Value;

            Vector2 origin = new(texture.Width * 0.5f, texture.Height * 0.5f);
            SpriteEffects effects = Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            float outlineRotation = Projectile.rotation;
            if (Projectile.direction == -1)
            {
                outlineRotation += MathHelper.Pi; // 180 degrees
            }

            // Draw the outline first
            Main.EntitySpriteDraw(
                whiteOutline,
                Projectile.Center - Main.screenPosition,
                null,
                ColorLib.StellarColor,
                outlineRotation,
                origin,
                Projectile.scale,
                effects,
                0
            );

            return true; // Let the base projectile texture draw as usual
        }


        

        int lanceTimer = 0;
        int starTimer = 0;


        public int LancesShot = 0;

        public int StarsShot = 0;
        
        SoundStyle Shoot1 = new SoundStyle($"DestroyerTest/Assets/Audio/ConstitutionBoss/ConstitutionBossShootStars3") with
            {
                //Volume = 0.5f,
                PitchVariance = 1.0f,
            };
            SoundStyle Shoot2 = new SoundStyle($"DestroyerTest/Assets/Audio/StarShot") with
            {
                Volume = 2f,
                PitchVariance = 1.0f,
                MaxInstances = 100
            };

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.HeldItem.type == ModContent.ItemType<StellarBow>() && player.channel)
            {
                // Lock projectile to player/cursor
                float holdDistance = 15f;
                Vector2 mountedCenter = player.MountedCenter;
                Vector2 toCursor = Main.MouseWorld - mountedCenter;
                toCursor.Normalize();
                Vector2 desiredPos = mountedCenter + toCursor * holdDistance;

                Projectile.Center = desiredPos;
                Projectile.rotation = toCursor.ToRotation();
                Projectile.direction = toCursor.X > 0 ? 1 : -1;

                // State Machine
                switch (aiState)
                {
                    case 0: // Lance Firing
                        stateTimer++;
                        if (stateTimer >= 30 && LancesShot < 3)
                        {
                            stateTimer = 0;
                            LancesShot++;
                            SoundEngine.PlaySound(Shoot1);

                            Projectile.NewProjectile(
                                Projectile.GetSource_FromThis(),
                                Projectile.Center,
                                toCursor * 20f,
                                ModContent.ProjectileType<GalantineLanceFriendly>(),
                                60,
                                2f,
                                player.whoAmI
                            );
                        }
                        if (LancesShot >= 3)
                        {
                            aiState = 1;
                            stateTimer = 0;
                        }
                        break;

                    case 1: // Star Helix Firing
                        stateTimer++;
                        if (stateTimer >= 5 && StarsShot < 10)
                        {
                            stateTimer = 0;
                            StarsShot++;
                            SoundEngine.PlaySound(Shoot2);

                            float[] helixOffsets = { -1f, -0.5f, 0f, 0.5f, 1f };
                            int step = StarsShot % helixOffsets.Length;

                            float helixRadius = 32f;
                            Vector2 perp = new Vector2(-toCursor.Y, toCursor.X);
                            perp.Normalize();
                            Vector2 spawnOffset = perp * helixOffsets[step] * helixRadius;

                            Projectile.NewProjectileDirect(
                                Projectile.GetSource_FromThis(),
                                Projectile.Center + spawnOffset,
                                toCursor * 10f,
                                ProjectileID.Starfury,
                                60,
                                2f,
                                player.whoAmI
                            );
                        }
                        if (StarsShot >= 10)
                        {
                            LancesShot = 0;
                            StarsShot = 0;
                            aiState = 0;
                            stateTimer = 0;
                        }
                        break;
                }
            }
            else
            {
                Projectile.Kill();
            }
        }


    }
}