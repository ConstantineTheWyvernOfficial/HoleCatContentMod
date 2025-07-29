using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.CheatItems
{
	// ExampleStaff is a typical staff. Staffs and other shooting weapons are very similar, this example serves mainly to show what makes staffs unique from other items.
	// Staff sprites, by convention, are angled to point up and to the right. "Item.staff[Type] = true;" is essential for correctly drawing staffs.
	// Staffs use mana and shoot a specific projectile instead of using ammo. Item.DefaultToStaff takes care of that.
	public class BewitchingScepter : ModItem
	{
        public override string Texture => "DestroyerTest/Content/CheatItems/BewitchingScepter";
		public override void SetStaticDefaults() {
			Item.staff[Type] = true; // This makes the useStyle animate as a staff instead of as a gun.
		}

		public override void SetDefaults() {
			// DefaultToStaff handles setting various Item values that magic staff weapons use.
			// Hover over DefaultToStaff in Visual Studio to read the documentation!
			Item.DefaultToStaff(ProjectileID.NebulaBolt, 16, 25, 12);
            Item.width = 90;
            Item.height = 90;

			Item.UseSound = SoundID.MaxMana;
			// Set damage and knockBack
			Item.damage = 0;
            Item.autoReuse = true;
            Item.useTime = 10;

			// Set rarity and value
			Item.SetShopValues(ItemRarityColor.Green2, 10000);
		}
        public override bool? UseItem(Player player)
        {
            var modPlayer = player.GetModPlayer<MinionSlotIncrementPlayer>();

            modPlayer.customMinionCap++; // Increase the stored value

            if (modPlayer.customMinionCap > 100)
            {
                modPlayer.customMinionCap = 1; // Reset to 1 if over 100
            }

            player.maxMinions = modPlayer.customMinionCap; // Apply the change to the actual game property

            CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.Yellow, 
                $"Minion Slots: {player.slotsMinions}/{modPlayer.customMinionCap}");

            return true;
        }


		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.RoyalScepter)
                .AddIngredient<Item_TemperedObsidian>(4)
				.AddTile(TileID.BewitchingTable)
				.Register();
		}
	}

    public class MinionSlotIncrementPlayer : ModPlayer
    {
        public int customMinionCap = 1; // This stores the persistent minion cap

        public override void ResetEffects()
        {
            Player.maxMinions = customMinionCap; // Apply the stored value
        }
    }

   
}