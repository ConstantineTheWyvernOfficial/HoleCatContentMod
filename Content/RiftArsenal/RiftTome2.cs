
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.Riftplate;


namespace DestroyerTest.Content.RiftArsenal
{
    public class RiftTome2 : ModItem
	{
        public override void SetStaticDefaults()
        {
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(2, 7)); 
        }
        public override void SetDefaults() {
            Item.UseSound = SoundID.Item9;
			Item.width = 32; // The item texture's width.
			Item.height = 42; // The item texture's height.

			Item.useStyle = ItemUseStyleID.HoldUp; // The useStyle of the Item.
			Item.useTime = 120; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 120; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.
            Item.mana = 20;

			Item.DamageType = DamageClass.Magic; // Whether your item is part of the melee class.
			Item.damage = 100; // The damage your item deals.
			Item.knockBack = 0; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 4; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.
            Item.channel = true;
            Item.InterruptChannelOnHurt = true;

			Item.value = Item.buyPrice(gold: 1); // The value of the weapon in copper coins.
			Item.rare = ModContent.RarityType<RiftRarity1>(); // Give this item our custom rarity.
            Item.shoot = ModContent.ProjectileType<Shadow>();
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Item_HeliciteCrystal>(12)
				.AddIngredient<Item_Riftplate>(12)
                .AddIngredient(ItemID.SpellTome)
				.AddTile<Tile_RiftCrucible>()
				.Register();
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
	}

    public class RiftTPPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
			
				if (DestroyerTestMod.RiftTeleportKeybind.JustPressed && Player.HeldItem.type == ModContent.ItemType<RiftTome2>() && !Player.HasBuff(BuffID.ChaosState))
				{
					ScreenFlashSystem.FlashIntensity = 0.9f; // Set to full brightness
					SoundStyle TPSound = new SoundStyle($"DestroyerTest/Assets/Audio/RiftMaker_Boom");
					SoundEngine.PlaySound(TPSound);
					
                    Player.Center = Main.MouseWorld;
                    Player.Hurt(PlayerDeathReason.ByCustomReason($"{Player.name} spent too much time warping the fabric of reality."), 25, 0);
                    Player.AddBuff(BuffID.ChaosState, 6000);
				}
			
        }
    }

	
}