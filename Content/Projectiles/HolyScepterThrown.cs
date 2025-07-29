using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using DestroyerTest.Content.Consumables;
using DestroyerTest.Common;
using Terraria.DataStructures;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Buffs;
using System.IO;
using InnoVault.PRT;
using DestroyerTest.Content.Projectiles.ParentClasses;

namespace DestroyerTest.Content.Projectiles
{
    public class HolyScepterThrown : ThrownScepter
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
            target.StrikeNPC(hit); // This bypasses i-frames
            target.AddBuff(ModContent.BuffType<PowerTrade>(), 120);
            SoundEngine.PlaySound(SoundID.Item113, Projectile.position);
            HitCount += 1;
            PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRingSharp1>(), target.Center, Vector2.Zero, Color.Red, 1);
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}

