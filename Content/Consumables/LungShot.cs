using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace DestroyerTest.Content.Consumables
{
	public class LungShot : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 20;

			// Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
			ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
				new Color(255, 155, 0),
				new Color(0, 0, 0),
				new Color(140, 140, 140)
			};
		}

        public static int BuffTimeInt = 4005;

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 15;
			Item.useTime = 90;
			Item.useTurn = true;
			Item.UseSound = new SoundStyle ("DestroyerTest/Assets/Audio/StoneLungsSound");
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 1);
			Item.buffType = ModContent.BuffType<Buffs.StoneLungs>(); // Specify an existing buff to be applied when used.
			Item.buffTime = BuffTimeInt; // The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
		}
	}
}