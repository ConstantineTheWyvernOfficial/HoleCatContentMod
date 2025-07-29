using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Common;
using DestroyerTest.Common.Systems;


namespace DestroyerTest.Content.CheatItems
{
    	public class ShimmeringBossResetter : ModItem
	{
        public override void SetDefaults() {
            Item.UseSound = new SoundStyle ("DestroyerTest/Assets/Audio/TenebrisSpawn");
			Item.width = 32; // The item texture's width.
			Item.height = 40; // The item texture's height.

			Item.useStyle = ItemUseStyleID.HoldUp; // The useStyle of the Item.
			Item.useTime = 120; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 120; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = false; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Generic; // Whether your item is part of the melee class.
			Item.damage = 0; // The damage your item deals.
			Item.knockBack = 0; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 0; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 1); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<ShimmeringRarity>(); // Give this item our custom rarity.
		}

        public override bool? UseItem(Player player)
        {
            TenebrousTrialModDifficulty TenebrousTrial = ModContent.GetInstance<TenebrousTrialModDifficulty>();
            DownedBossSystem downedBossSystem = ModContent.GetInstance<DownedBossSystem>();
            if (TenebrousTrial.IsActive)
            {
				DownedBossSystem.downedEoCBoss = false;
                DownedBossSystem.downedKingSlimeBoss = false;
                DownedBossSystem.downedBoCBoss = false;
                DownedBossSystem.downedEoWBoss = false;
                DownedBossSystem.downedQueenBeeBoss = false;
                DownedBossSystem.downedDeerclopsMiniBoss = false;
                DownedBossSystem.downedSkeletronBoss = false;
                DownedBossSystem.downedWallBoss = false;
                DownedBossSystem.downedQueenSlimeBoss = false;
                DownedBossSystem.downedDestroyerBoss = false;
                DownedBossSystem.downedTwinsBoss = false;
                DownedBossSystem.downedSkeletronPrimeBoss = false;
                DownedBossSystem.downedNautilusMiniBoss = false;
                DownedBossSystem.downedPlanteraBoss = false;
                DownedBossSystem.downedGolemBoss = false;
                DownedBossSystem.downedFishronBoss = false;
                DownedBossSystem.downedEmpressBoss = false;
                DownedBossSystem.downedCultistBoss = false;
                DownedBossSystem.downedLunarBoss = false;

                Main.NewText("All boss defeat flags for Tenebrous Trial have been reset.", ColorLib.TenebrisGradient);
            }
            else
            {
                Main.NewText("Tenebrous Trial is not active. Boss flags cannot be reset.", ColorLib.TenebrisGradient);
            }
            return true;
        }
	}
}