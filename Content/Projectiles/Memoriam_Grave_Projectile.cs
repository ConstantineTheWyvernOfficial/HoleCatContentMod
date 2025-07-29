using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Projectiles
{
    public class Memoriam_Grave_Projectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Obelisk);
            Projectile.aiStyle = ProjectileID.Obelisk;
        }
        
    }
}