

using DestroyerTest.Content.Projectiles;
using DestroyerTest.Rarity;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.SummonItems
{
	public class WyvernTail : ModItem
	{
		public override void SetDefaults() {
			// This method quickly sets the whip's properties.
			// Mouse over to see its parameters.
			Item.DefaultToWhip(ModContent.ProjectileType<WyvernTailProjectile>(), 80, 2, 4);
            Item.width = 36;
            Item.height = 32;
            Item.autoReuse = true;

			Item.rare = ModContent.RarityType<CrimsonSpecialRarity>();
		}

		// Makes the whip receive melee prefixes
		public override bool MeleePrefix() {
			return false;
		}
	}
}