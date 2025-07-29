using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Dusts
{
    public class RiftCloudUpper : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f; // Slow down initial velocity
            dust.noGravity = true; // No gravity
            dust.noLight = true; // No light emission
            int frameHeight = 32; // Adjust based on your texture
            dust.frame = new Rectangle(0, Main.rand.Next(4) * frameHeight, frameHeight, frameHeight);
            
            

            // Spawn the outline dust first
            int lowerIndex = Dust.NewDust(dust.position, 0, 0, ModContent.DustType<RiftCloudLower>(), dust.velocity.X, dust.velocity.Y);
            Dust lower = Main.dust[lowerIndex];

            // Spawn the upper dust
            int upperIndex = Dust.NewDust(dust.position, 0, 0, ModContent.DustType<RiftCloudUpper>(), dust.velocity.X, dust.velocity.Y);
            Dust upper = Main.dust[upperIndex];

            // Swap indices if necessary to ensure correct rendering order
            if (upperIndex < lowerIndex)
            {
                Main.dust[lowerIndex] = upper;
                Main.dust[upperIndex] = lower;
            }
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale *= 0.99f;

            float light = 0.35f * dust.scale;
            Lighting.AddLight(dust.position, light, light, light);

            if (dust.scale < 0.5f)
            {
                dust.active = false;
            }

            return false; // Prevent vanilla behavior
        }
    }

    public class RiftCloudLower : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
            // Manually setting the dust frame (assuming an 8x8 frame size)
            int frameHeight = 32; // Adjust based on your texture
            dust.frame = new Rectangle(0, Main.rand.Next(4) * frameHeight, frameHeight, frameHeight);
        }

        public override bool Update(Dust dust)
        {
            // Get linked upper dust
            if (dust.customData is Dust upperDust && upperDust.active)
            {
                dust.position = upperDust.position;
                dust.velocity = upperDust.velocity;
                dust.rotation = upperDust.rotation;
                dust.scale = upperDust.scale * 1.2f; // Slightly larger for outline effect
            }

            dust.scale *= 0.99f; // Fades over time

            float light = 0.2f * dust.scale; // Dimmer light than upper dust
            Lighting.AddLight(dust.position, light, light, light);

            if (dust.scale < 0.5f)
            {
                dust.active = false;
            }

            return false; // Prevent vanilla behavior
        }
    }
}
