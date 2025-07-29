using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using DestroyerTest.Content.Projectiles.Pets;

namespace DestroyerTest.Content.SummonItems
{
    public class VanxarkBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VanxarkSummon>()] > 0)
            {
                player.buffTime[buffIndex] = 18000; // Keep the buff active
            }
            else
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ModContent.ProjectileType<VanxarkSummon>(), 0, 0f, player.whoAmI);
            }
        }
    }
}