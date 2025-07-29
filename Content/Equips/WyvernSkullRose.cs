
using System.Linq;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.Riftplate;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Projectiles;

namespace DestroyerTest.Content.Equips
{
	public class WyvernSkullRose : ModItem
	{

		public override void SetStaticDefaults() {
			
		}

		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 66;
			Item.value = 10000;
			Item.rare = ModContent.RarityType<CrimsonSpecialRarity>(); // The rarity of the item
			Item.accessory = true;
		}
        
        public override void UpdateAccessory(Player player, bool hideVisual)
            {
                player.GetModPlayer<WyvernSkullRoseDash>().DashAccessoryEquipped = true;
            }

		

    public class WyvernSkullRoseDash : ModPlayer
        {
            bool HasWyvernSkullRoseEquipped()
            {
                for (int i = 3; i < Player.armor.Length; i++)
                {
                    if (Player.armor[i].type == ModContent.ItemType<WyvernSkullRose>())
                    {
                        return true;
                    }
                }
                return false;
            }
            // These indicate what direction is what in the timer arrays used
            public const int DashDown = 0;
            public const int DashUp = 1;
            public const int DashRight = 2;
            public const int DashLeft = 3;

            public const int DashCooldown = 50; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
            public const int DashDuration = 35; // Duration of the dash afterimage effect in frames

            // The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
            public const float DashVelocity = 50f;

            // The direction the player has double tapped.  Defaults to -1 for no dash double tap
            public int DashDir = -1;

            // The fields related to the dash accessory
            public bool DashAccessoryEquipped;
            public int DashDelay = 0; // frames remaining till we can dash again
            public int DashTimer = 6; // frames remaining in the dash

            public override void ResetEffects()
            {
                // Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
                DashAccessoryEquipped = true;

                // ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
                // When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
                // If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
                if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15 && HasWyvernSkullRoseEquipped())
                {
                    DashDir = DashDown;
                }
                else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15 && HasWyvernSkullRoseEquipped())
                {
                    DashDir = DashUp;
                }
                else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15 && HasWyvernSkullRoseEquipped())
                DashDir = DashRight;

                else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15 && HasWyvernSkullRoseEquipped())
                {
                    DashDir = DashLeft;
                }
                else
                {
                    DashDir = -1;
                }
            }

            public int FleshBombTimer = 0;

            // This is the perfect place to apply dash movement, it's after the vanilla movement code, and before the player's position is modified based on velocity.
            // If they double tapped this frame, they'll move fast this frame
            public override void PreUpdateMovement()
            {

                // if the player can use our dash, has double tapped in a direction, and our dash isn't currently on cooldown
                if (CanUseDash() && DashDir != -1 && DashDelay == 0)
                {
                    Vector2 newVelocity = Player.velocity;

                    switch (DashDir)
                    {
                        // Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                        case DashUp when Player.velocity.Y > -DashVelocity:
                        case DashDown when Player.velocity.Y < DashVelocity:
                            {
                                // Y-velocity is set here
                                // If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
                                // This adjustment is roughly 1.3x the intended dash velocity
                                float dashDirection = DashDir == DashDown ? 1 : -1.3f;
                                newVelocity.Y = dashDirection * DashVelocity;
                                break;
                            }
                        case DashLeft when Player.velocity.X > -DashVelocity:
                        case DashRight when Player.velocity.X < DashVelocity:
                            {
                                // X-velocity is set here
                                float dashDirection = DashDir == DashRight ? 1 : -1;
                                newVelocity.X = dashDirection * DashVelocity;
                                break;
                            }
                        default:
                            return; // not moving fast enough, so don't start our dash
                    }

                    // start our dash
                    DashDelay = DashCooldown;
                    DashTimer = DashDuration;
                    Player.velocity = newVelocity;

                    // Here you'd be able to set an effect that happens when the dash first activates
                    // Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
                    Dust.NewDust(newVelocity, 15, 15, DustID.Blood, 6f, 0f, 0, default, 10f);
                    SoundEngine.PlaySound(new SoundStyle($"DestroyerTest/Assets/Audio/Corpse/FleshBombSpawn"));

                }

                if (DashDelay > 0)
                    DashDelay--;
                

                if (DashTimer > 0)
                { // dash is active
                  // This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
                  // Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
                  // Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect
                    Player.eocDash = DashTimer;
                    Player.armorEffectDrawShadowEOCShield = true;
                    FleshBombTimer++;
                    if (FleshBombTimer == 5)
                    {
                        Item skull = Player.armor.FirstOrDefault(item => item.type == ModContent.ItemType<WyvernSkullRose>());
                        if (skull != null)
                        {
                            Projectile.NewProjectile(
                                Player.GetSource_Accessory(skull),
                                Player.oldPosition, Vector2.Zero,
                                ModContent.ProjectileType<FleshBombFriendly>(), 46, 1, Main.LocalPlayer.whoAmI);
                        }

                        FleshBombTimer = 0;
                    }


                    // count down frames remaining
                    DashTimer--;
                }
            }

            private bool CanUseDash()
            {
                return DashAccessoryEquipped
                    && Player.dashType == DashID.None // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
                    && HasWyvernSkullRoseEquipped()
                    && !Player.setSolar // player isn't wearing solar armor
                    && !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
            }
        }

		
	}
}