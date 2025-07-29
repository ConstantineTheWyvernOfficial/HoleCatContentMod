
using System.Linq;
using DestroyerTest.Common;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Equips
{
    public class DeadlyBlossom : ModItem
    {
        public static bool CanSpawnBlossom = false;

        public override void SetDefaults()
        {
            Item.width = 26; // Width of the item
            Item.height = 26; // Height of the item
            Item.value = Item.sellPrice(gold: 10); // How many coins the item is worth
            Item.rare = ModContent.RarityType<CorruptionSpecialRarity>(); // The rarity of the item
            Item.vanity = false;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.owner == player.whoAmI && projectile.type == ModContent.ProjectileType<MiniRose>())
                {
                    CanSpawnBlossom = false;
                }
                else 
                {
                    CanSpawnBlossom = true;
                }
            }
            
        }


    }
    
    public class DBPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
		{
			foreach (Projectile projectile in Main.projectile)
			{
				if (DestroyerTestMod.DeadlyBlossomKeybind.JustPressed && DeadlyBlossom.CanSpawnBlossom == true)
				{
					
					SoundEngine.PlaySound(SoundID.Item79, Player.Center);

					Projectile.NewProjectile(Entity.GetSource_Accessory(Player.armor.FirstOrDefault(item => item.type == ModContent.ItemType<DeadlyBlossom>())), Player.Center, Vector2.Zero, ModContent.ProjectileType<MiniRose>(), 0, 0, Main.LocalPlayer.whoAmI);
					
				}
			}
		}
    }
}
