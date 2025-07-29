using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using DestroyerTest.Content.Particles;
using Terraria.ID;
using DestroyerTest.Common;
using DestroyerTest.Content.Magic.ScepterSubclass;

namespace DestroyerTest.Content.Buffs
{
    public class ManaBurst : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var modPlayer = player.GetModPlayer<OldManaPlayer>();

            int OldMana = modPlayer.StoredMana; // Use stored mana

            Dust.NewDust(player.position, player.width, player.height, DustID.GemSapphire, 0.0f, 0.5f, 0, default, 1);
            player.GetDamage(ModContent.GetInstance<ScepterClass>()) *= OldMana/2;
        }

        


    }
}