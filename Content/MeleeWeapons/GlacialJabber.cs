using DestroyerTest.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.MeleeWeapons
{
    public class GlacialJabber : ModItem
        {
            SoundStyle Jab = new SoundStyle($"DestroyerTest/Assets/Audio/FrigidHalberdJab") {
				Volume = 1.0f, 
				Pitch = 0.0f, 
				PitchVariance = 0.5f, 
			}; 
            //Weapon Properties
            public override void SetStaticDefaults() {
                ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // This skips use animation-tied sound playback, so that we're able to make it be tied to use time instead in the UseItem() hook.
                ItemID.Sets.Spears[Item.type] = true; // This allows the game to recognize our new item as a spear.
            }

            public override void SetDefaults() {
                Item.width = 108; // The item texture's width.
                Item.height = 106; // The item texture's height.
                // Common Properties
                Item.rare = ItemRarityID.Pink; // Assign this item a rarity level of Pink
                Item.value = Item.sellPrice(silver: 10); // The number and type of coins item can be sold for to an NPC

                // Use Properties
                Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
                Item.useAnimation = 18; // The length of the item's use animation in ticks (60 ticks == 1 second.)
                Item.useTime = 18; // The length of the item's use time in ticks (60 ticks == 1 second.)
                Item.UseSound = Jab;
                Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

                // Weapon Properties
                Item.damage = 16;
                Item.knockBack = 6.5f;
                Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
                Item.DamageType = DamageClass.Melee;
                Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.

                // Projectile Properties
                Item.shootSpeed = 3.7f; // The speed of the projectile measured in pixels per frame.
                Item.shoot = ModContent.ProjectileType<GlacialJabberJabProjectile>(); // The projectile that is fired from this weapon
            }
            public override bool AltFunctionUse(Player player) {
                return true;
            }

            
            public override bool CanUseItem(Player player) {
                // SWING
                if (player.altFunctionUse == 2) {
                        Item.width = 108; // The item texture's width.
                        Item.height = 106; // The item texture's height.

                        Item.useStyle = ItemUseStyleID.Shoot; // The useStyle of the Item.
                        Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
                        Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
                        Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

                        Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
                        Item.damage = 46; // The damage your item deals.
                        Item.knockBack = 6; // The force of knockback of the weapon. Maximum is 20
                        Item.crit = 6; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

                        Item.value = Item.buyPrice(gold: 1); // The value of the weapon in copper coins.
                        Item.UseSound = Jab;
                        Item.shoot = ModContent.ProjectileType<GlacialJabberSwingProjectile>(); // The projectile that is fired from this weapon
                        
                }
                // STAB AI
                else {
                    Item.width = 108; // The item texture's width.
                    Item.height = 106; // The item texture's height.
                    // Common Properties
                Item.rare = ItemRarityID.Pink; // Assign this item a rarity level of Pink
                Item.value = Item.sellPrice(silver: 10); // The number and type of coins item can be sold for to an NPC

                // Use Properties
                Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
                Item.useAnimation = 18; // The length of the item's use animation in ticks (60 ticks == 1 second.)
                Item.useTime = 18; // The length of the item's use time in ticks (60 ticks == 1 second.)
                Item.UseSound = Jab;
                Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

                // Weapon Properties
                Item.damage = 26;
                Item.knockBack = 6.5f;
                Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
                Item.DamageType = DamageClass.Melee;
                Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.

                // Projectile Properties
                Item.shootSpeed = 3.7f; // The speed of the projectile measured in pixels per frame.
                Item.shoot = ModContent.ProjectileType<GlacialJabberJabProjectile>(); // The projectile that is fired from this weapon
                }
                return base.CanUseItem(player);
            }

            public override bool? UseItem(Player player)
            {
                SoundEngine.PlaySound(Jab, player.Center);
                return base.UseItem(player);
            }
             public override void AddRecipes() {
			CreateRecipe()
                .AddIngredient<FrigidHalberd>(1)
                .AddIngredient(ItemID.FrostCore, 5)
                .AddIngredient(ItemID.LivingFrostFireBlock, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		    }
        }
}

		