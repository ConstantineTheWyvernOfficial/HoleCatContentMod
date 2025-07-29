using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using Terraria.GameContent.Drawing;
using System.IO;
using DestroyerTest.Content.Projectiles.ParentClasses;

namespace DestroyerTest.Content.Projectiles
{
    public class NecroScepterThrown : ThrownScepter
    {
        public override void SetDefaults()
        {
            ThemeColor = Color.White;
            WidthDim = 34;
            HeightDim = 34;
            DustType = DustID.Glass;
            base.SetDefaults();
        }
    

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Main.myPlayer];  // Accessing the current player
            SoundEngine.PlaySound(SoundID.NPCHit2, Projectile.position);
          
            Projectile newProjectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), 
            Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RibCage>(), 0, 0, player.whoAmI);
            newProjectile.friendly = true; // If it shouldn't harm the player, for example
            base.OnHitNPC(target, hit, damageDone);
        }

    }

}

