using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.Magic;
using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Projectiles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Particles
{
    // Our first PRT particle, pretty cool right? It is generated in VaultSword, 
    // so grab the sword and check out the effect.
    internal class RuneCircle2 : BasePRT
    {

        // The Texture property doesn't need to be overridden, as BasePRT has an automatic loading mechanism.
        // It automatically loads a .png file with the same name in the same directory.
        // This is similar to how ModProjectile works.
        // So, let's prepare a .png file called "ExamplePRT", which is an image with the same name as the class.
        // public override string Texture => base.Texture;

        // Override this function, it will be called once when the particle is generated.
        // PRT entities are independent instances, so the settings in this function
        // can also be applied to each instance individually, similar to ModProjectile.SetDefaults.
        public int MaxLifetime => 60;

        public override void SetProperty()
        {
            // PRTDrawMode determines which rendering mode the instance will be batched into.
            // This sets the color blending mode for the particle's rendering.
            // Here, we set it to additive blending mode. The effect brought by this field is real-time,
            // and it will batch all PRT instances in each draw call.
            PRTDrawMode = PRTDrawModeEnum.AdditiveBlend;
            Lifetime = MaxLifetime; // Lifetime of 220 to 360 ticks.
            Color = ColorLib.TenebrisGradient;
            //Rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);


        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;
            if (player.HasBuff<ShimmeringEmpowerment>() && player.active)
            {
                Lifetime = MaxLifetime;
            }



            Rotation += 0.4f;

            Position = Main.LocalPlayer.Center;

            // Adjust the rotation according to the movement direction.
            //Rotation += Main.rand.NextFloat(-0.1f, 0.1f);



            //// Relative position change
            Position += Main.LocalPlayer.velocity;


        }

        // Override this drawing function. If you want to customize the drawing, return false here,
        // and the default drawing will not be applied.
        public override bool PreDraw(SpriteBatch spriteBatch) => true;
    }
    
    internal class RuneCircle1 : BasePRT
    {

        // The Texture property doesn't need to be overridden, as BasePRT has an automatic loading mechanism.
        // It automatically loads a .png file with the same name in the same directory.
        // This is similar to how ModProjectile works.
        // So, let's prepare a .png file called "ExamplePRT", which is an image with the same name as the class.
        // public override string Texture => base.Texture;

        // Override this function, it will be called once when the particle is generated.
        // PRT entities are independent instances, so the settings in this function
        // can also be applied to each instance individually, similar to ModProjectile.SetDefaults.
        public int MaxLifetime => 100;

        public override void SetProperty()
        {
            // PRTDrawMode determines which rendering mode the instance will be batched into.
            // This sets the color blending mode for the particle's rendering.
            // Here, we set it to additive blending mode. The effect brought by this field is real-time,
            // and it will batch all PRT instances in each draw call.
            PRTDrawMode = PRTDrawModeEnum.AdditiveBlend;
            Lifetime = MaxLifetime; // Lifetime of 220 to 360 ticks.
            
            //Rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);


        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<ContemptCursorProjectile>())
                {
                    Lifetime = MaxLifetime;
                }
                else
                {
                    Kill();
                }
            }

          

            Rotation += 0.4f;

            Position = Main.MouseWorld;

            // Adjust the rotation according to the movement direction.
            //Rotation += Main.rand.NextFloat(-0.1f, 0.1f);



            //// Relative position change
            Position += Main.LocalPlayer.velocity;

            
        }

        // Override this drawing function. If you want to customize the drawing, return false here,
        // and the default drawing will not be applied.
        public override bool PreDraw(SpriteBatch spriteBatch) => true;
    }

    
}