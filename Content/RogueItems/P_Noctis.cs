using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.RogueItems
{
	public class P_Noctis : ModItem
	{
		public override void SetDefaults() {
			// Alter any of these values as you see fit, but you should probably keep useStyle on 1, as well as the noUseGraphic and noMelee bools

			// Common Properties
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(silver: 5);
			Item.maxStack = 1;

			// Use Properties
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 30;
			Item.useTime = 30;
            Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/P_Noctis_Throw") {
					Volume = 1.0f, 
					Pitch = 0.0f, 
					PitchVariance = 0.2f, 
				}; 
			Item.autoReuse = true;
			Item.consumable = false;
            Item.crit = 96; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			// Weapon Properties			
			Item.damage = 210;
			Item.knockBack = 200f;
			Item.noUseGraphic = true; // The item should not be visible when used
			Item.noMelee = true; // The projectile will do the damage and not the item
			Item.DamageType = DamageClass.Ranged;

			// Projectile Properties
			Item.shootSpeed = 25f;
			Item.shoot = ModContent.ProjectileType<Projectiles.P_Noctis_Projectile>(); // The projectile that will be thrown
			
		}

		public override void UpdateInventory(Player player) {
			if (player.ZoneGraveyard) {
				Item.damage = (int)(Item.damage * 1.75f);
			}
		}
		
		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Bone, 200)
				.AddIngredient(ItemID.BoneFeather, 5)
				.AddIngredient(ItemID.IceFeather, 5)
				.AddIngredient(ItemID.FireFeather, 5)
				.AddIngredient(ItemID.GiantHarpyFeather, 5)
				.AddIngredient(ItemID.Feather, 5)
                .AddIngredient(ItemID.DayBreak, 1)
                .AddIngredient(ItemID.SoulofFright, 30)
				.AddTile(TileID.BoneWelder)
				.Register();
		}
	}
}