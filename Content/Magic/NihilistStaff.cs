using DestroyerTest.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Magic
{
	// ExampleStaff is a typical staff. Staffs and other shooting weapons are very similar, this example serves mainly to show what makes staffs unique from other items.
	// Staff sprites, by convention, are angled to point up and to the right. "Item.staff[Type] = true;" is essential for correctly drawing staffs.
	// Staffs use mana and shoot a specific projectile instead of using ammo. Item.DefaultToStaff takes care of that.
	public class NihilistStaff : ModItem
	{
        public override string Texture => "DestroyerTest/Content/Magic/NihilistStaff_Item";
		public override void SetStaticDefaults() {
			Item.staff[Type] = true; // This makes the useStyle animate as a staff instead of as a gun.
		}

		public override void SetDefaults() {
			// DefaultToStaff handles setting various Item values that magic staff weapons use.
			// Hover over DefaultToStaff in Visual Studio to read the documentation!
			Item.DefaultToStaff(ModContent.ProjectileType<CruiserSausage>(), 16, 25, 12);

			// Customize the UseSound. DefaultToStaff sets UseSound to SoundID.Item43, but we want SoundID.Item20
            SoundStyle SpawnSound = new Terraria.Audio.SoundStyle($"DestroyerTest/Assets/Audio/DevourerDeathImpact");

			Item.UseSound = SpawnSound;
			// Set damage and knockBack
			Item.SetWeaponValues(20, 5);

			// Set rarity and value
			Item.SetShopValues(ItemRarityColor.Green2, 10000);
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.BlackInk, 4)
                .AddIngredient(ItemID.GoldBar, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}