using System.Collections.Generic;
using System.IO;
using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Projectiles.CorpseBoss;
using DestroyerTest.Content.RiftArsenal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles.CorpseBoss.Organs
{
    public abstract class OrganProjectile : ModProjectile
    {
        public override string Texture => "DestroyerTest/Content/Particles/ParticleDrawEntity";


        public override void SetDefaults()
        {
            Projectile.width = 20; // The width of projectile hitbox
            Projectile.height = 20; // The height of projectile hitbox

            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = false; // Can the projectile deal damage to enemies?
            Projectile.hostile = true; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.light = 1f; // How much light emit around the projectile
            Projectile.timeLeft = 120; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }
        
        public List<Vector2> PlayerOldPos = new List<Vector2>();
        
        public Vector2 toPlayer;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(toPlayer);
            writer.WriteVector2(Projectile.Center);
            writer.WriteVector2(PlayerOldPos.Count > 0 ? PlayerOldPos[0] : Vector2.Zero);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            toPlayer = reader.ReadVector2();
            Projectile.Center = reader.ReadVector2();
            if (PlayerOldPos.Count > 0)
            {
                PlayerOldPos[0] = reader.ReadVector2();
            }
        }
        public override void AI()
        {

            PlayerOldPos.Add(Main.LocalPlayer.Center);
            if (PlayerOldPos.Count > 35)
            {
                PlayerOldPos.RemoveAt(0);
            }
            Vector2 ToPlayer = Projectile.Center - Main.LocalPlayer.Center;
            Projectile.velocity *= 0.999f;
            Projectile.rotation += Main.rand.NextFloat(-1f, 1.1f) * 0.1f;
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Blood, 0, 0, 70, default, 1.0f);
            }


            if (PlayerOldPos.Count > 4)
            {
                toPlayer = PlayerOldPos[4] - Projectile.Center;
                // Your aiming code here
            }
            else
            {
                // Not enough data yet, maybe use the current player position instead:
                toPlayer = Main.LocalPlayer.Center - Projectile.Center;
            }

        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);
            DrawTelegraph(Projectile.Center, Main.LocalPlayer.Center, ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/LaserGlow").Value);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.moveSpeed *= 0.6f;
        }
        


        public override void OnKill(int timeLeft)
        {


            SoundEngine.PlaySound(SoundID.NPCDeath47, Projectile.Center);
            Projectile.NewProjectile(Entity.GetSource_Death(), Projectile.Center, toPlayer * 0.2f, ProjectileID.GoldenShowerHostile, 16, 1);
        }

        public void DrawTelegraph(Vector2 start, Vector2 end, Texture2D texture)
        {
            Vector2 direction = end - start;
            float length = direction.Length();
            direction.Normalize();
            texture ??= ModContent.Request<Texture2D>("DestroyerTest/Content/Particles/LaserGlow").Value;
            SpriteBatch spriteBatch = Main.spriteBatch;

            float rotation = direction.ToRotation();

            // Assuming your texture is a chain segment, like 16px long
            float segmentLength = texture.Height * 0.75f; // or Width, depending on the texture orientation
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            for (float i = 0; i < length; i += segmentLength)
            {
                Vector2 position = start + direction * i;

                Main.spriteBatch.Draw(
                    texture,
                    position - Main.screenPosition,
                    null,
                    Color.Goldenrod * 0.2f,
                    rotation + MathHelper.PiOver2, // Adjust if your texture points upward
                    new Vector2(texture.Width / 2f, texture.Height / 2f), // Origin at center
                    1f, // Scale
                    SpriteEffects.None,
                    0f
                );
            }
            
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }       
    }

    public class OrganProjectile_Variant1 : OrganProjectile
    {
        public override string Texture => "DestroyerTest/Content/Projectiles/CorpseBoss/Organs/OrganProjectile_Variant1";

    }

    public class OrganProjectile_Variant2 : OrganProjectile
    {
        public override string Texture => "DestroyerTest/Content/Projectiles/CorpseBoss/Organs/OrganProjectile_Variant2";

    }

    public class OrganProjectile_Variant3 : OrganProjectile
    {
        public override string Texture => "DestroyerTest/Content/Projectiles/CorpseBoss/Organs/OrganProjectile_Variant3";

    }
    
    public class OrganProjectile_Variant4 : OrganProjectile
	{
		public override string Texture => "DestroyerTest/Content/Projectiles/CorpseBoss/Organs/OrganProjectile_Variant4";
		
	}
}