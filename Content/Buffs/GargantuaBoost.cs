
using DestroyerTest.Common;
using DestroyerTest.Content.Dusts;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.Particles;
using Humanizer;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Buffs
{
	// This class serves as an example of a debuff that causes constant loss of life
	// See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
	public class GargantuaBoost : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
            
        }

        // Allows you to make this buff give certain effects to the given player
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HeldItem.ModItem is not Gargantua)
            {
                player.GetModPlayer<GargantuaBoostPlayer>().lifeRegenDebuff = true;
            }
            if (player.HeldItem.ModItem is Gargantua)
            {
                player.GetModPlayer<GargantuaBoostPlayer>().AttackBuff = true;
            }

		}
	}



    public class GargantuaBoostPlayer : ModPlayer
    {

        // Flag checking when life regen debuff should be activated
        public bool lifeRegenDebuff;
        public bool AttackBuff;

        public override void ResetEffects()
        {
            lifeRegenDebuff = false;
            AttackBuff = false;
            HasUpdated = false;
        }

        private bool HasUpdated = false;
        public override void PostUpdateBuffs()
        {
            if (AttackBuff)
            {
                if (HasUpdated == false)
                {
                    Player.itemTimeMax = 8;
                    Player.itemAnimationMax = 8;
                    Player.GetDamage(DamageClass.Melee) *= 1.25f;
                    HasUpdated = true;
                }
            }
        }

        // Allows you to give the player a negative life regeneration based on its state (for example, the "On Fire!" debuff makes the player take damage-over-time)
        // This is typically done by setting player.lifeRegen to 0 if it is positive, setting player.lifeRegenTime to 0, and subtracting a number from player.lifeRegen
        // The player will take damage at a rate of half the number you subtract per second
        public override void UpdateBadLifeRegen()
        {
            Player player = Main.LocalPlayer;
            if (lifeRegenDebuff)
            {
                int[] types = new int[]
                {
                    PRTLoader.GetParticleID<BlackFire1>(),
                    PRTLoader.GetParticleID<BlackFire2>(),
                    PRTLoader.GetParticleID<BlackFire3>(),
                    PRTLoader.GetParticleID<BlackFire4>(),
                    PRTLoader.GetParticleID<BlackFire5>(),
                    PRTLoader.GetParticleID<BlackFire6>(),
                    PRTLoader.GetParticleID<BlackFire7>()
                };

                PRTLoader.NewParticle(types[Main.rand.Next(types.Length)], Main.rand.NextVector2FromRectangle(player.getRect()), Vector2.Zero, Color.Red, 1f);
                // These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                // Player.lifeRegenTime used to increase the speed at which the player reaches its maximum natural life regeneration
                // So we set it to 0, and while this debuff is active, it never reaches it
                Player.lifeRegenTime = 0;
                // lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second
                Player.lifeRegen -= 160;
            }
        }
        
	}
}