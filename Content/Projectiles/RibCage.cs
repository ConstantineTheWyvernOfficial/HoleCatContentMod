using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DestroyerTest.Content.Projectiles
{
    public class RibCage : ModProjectile
    {
        private NPC targetNPC;

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;  // This can be changed based on the type of projectile you want
            Projectile.penetrate = -1;  // So it doesn't disappear after hitting an NPC
            Projectile.timeLeft = 300;  // Adjust lifespan as needed
            Projectile.netImportant = true;
			Projectile.netUpdate = true;
        }

        public override void AI()
        {
            // Find the closest NPC
            if (targetNPC == null || targetNPC.boss || !targetNPC.active)
            {
                targetNPC = FindClosestNPC();
            }

            // Lock onto the target NPC's position
            if (targetNPC != null && !targetNPC.boss && targetNPC.active)
            {
                Vector2 targetPosition = targetNPC.Center;
                Vector2 direction = targetPosition - Projectile.Center;
                float distance = direction.Length();

                // Move the projectile towards the NPC
                direction.Normalize();
                Projectile.velocity = direction * 10f;  // Adjust speed as needed

                // Prevent the NPC from moving by freezing its velocity
                targetNPC.velocity = Vector2.Zero;

                // Once the projectile hits the NPC, stop its movement
                if (distance < 20f)  // When close enough
                {
                    targetNPC.velocity = Vector2.Zero; // Ensure NPC remains frozen
                    // Additional effect: Maybe apply debuffs or effects
                    targetNPC.AddBuff(BuffID.Confused, 300);  // Example: Confuse debuff
                }
            }
        }

        private NPC FindClosestNPC()
        {
            NPC closestNPC = null;
            float closestDistance = float.MaxValue;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.friendly) continue;

                float distance = Vector2.Distance(Projectile.Center, npc.Center);
                if (distance < closestDistance)
                {
                    closestNPC = npc;
                    closestDistance = distance;
                }
            }

            return closestNPC;
        }

    #pragma warning disable CS0672 // Member overrides obsolete member
        public override void Kill(int timeLeft)
    #pragma warning restore CS0672 // Member overrides obsolete member
        {
            // Clean up, remove any effect on the NPC
            if (targetNPC != null && targetNPC.active)
            {
                targetNPC.velocity = targetNPC.oldVelocity;
            }
        }
    }
}
