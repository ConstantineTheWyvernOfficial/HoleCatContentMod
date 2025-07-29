using DestroyerTest.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Content.Tiles.RiftConfigurator;

namespace DestroyerTest.Content.MeleeWeapons
{
    public class RiftShiv : ModItem
        {

            //Weapon Properties
            public override void SetStaticDefaults() {
                ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // This skips use animation-tied sound playback, so that we're able to make it be tied to use time instead in the UseItem() hook.
                ItemID.Sets.Spears[Item.type] = true; // This allows the game to recognize our new item as a spear.
            }

            public override void SetDefaults() {
                Item.width = 38; // The item texture's width.
                Item.height = 38; // The item texture's height.
                // Common Properties
                Item.rare = ModContent.RarityType<RiftRarity1>(); // Assign this item a rarity level of Pink
                Item.value = Item.sellPrice(silver: 10); // The number and type of coins item can be sold for to an NPC

                // Use Properties
                Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
                Item.useAnimation = 18; // The length of the item's use animation in ticks (60 ticks == 1 second.)
                Item.useTime = 18; // The length of the item's use time in ticks (60 ticks == 1 second.)
                Item.UseSound = SoundID.Item101;
                Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

                // Weapon Properties
                Item.damage = 60;
                Item.knockBack = 6.5f;
                Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
                Item.DamageType = DamageClass.Melee;
                Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.

                // Projectile Properties
                Item.shootSpeed = 1.0f; // The speed of the projectile measured in pixels per frame.
                Item.shoot = ModContent.ProjectileType<RiftShivProjectile>(); // The projectile that is fired from this weapon
            }
            public override bool? UseItem(Player player)
            {
                SoundEngine.PlaySound(SoundID.Item101, player.Center);
                return base.UseItem(player);
            }
            public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<Item_Riftplate>(12)
				.AddTile<Tile_RiftConfiguratorWeaponry>()
				.Register();
		    }

        }
}

		