using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DestroyerTest.Content.SummonItems
{
    public class RiftBallBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<RiftBall>()] > 0)
            {
                player.buffTime[buffIndex] = 18000; // Keep the buff active
            }
            else
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<RiftBall>(), 0, 0f, player.whoAmI);
            }
        }
    }
}