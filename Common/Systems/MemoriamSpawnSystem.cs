using DestroyerTest.Content.MeleeWeapons;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Common.Systems
{
    public class MemoriamSpawn : ModPlayer
    {
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            Player player = Main.player[Player.whoAmI]; // Get the current player instance
            // Roll a random integer between 1 and 2
            if (Main.rand.Next(1, 3) == 1 && Main.hardMode) // 1/2 chance
            {
                // Play a sound effect
                SoundEngine.PlaySound(SoundID.Item4, player.Center);
                Main.NewText($"Something has appeared at your spawn point...", 255, 255, 255); // Notify the player with a message

                // Spawn the "Memoriam" item at the player's spawn point
                int itemType = ModContent.ItemType<Memoriam>(); // Replace with your actual item class
                Item.NewItem(player.GetSource_Death(), player.SpawnX, player.SpawnY, 32, 32, itemType);
            }
        }
    }
}