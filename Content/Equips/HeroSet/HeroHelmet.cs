using System;
using System.Configuration;
using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.Particles;
using InnoVault.PRT;

namespace DestroyerTest.Content.Equips.HeroSet
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class HeroHelmet : ModItem
	{
		public int ParticleSpawnTimer = 0;
		public override void SetStaticDefaults() {
			// If your head equipment should draw hair while drawn, use one of the following:

		}

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<GoliathRarity>(); // The rarity of the item
			Item.defense = 3; // The amount of defense the item will give when equipped
		}

        public override void UpdateEquip(Player player) {
            player.GetDamage(DamageClass.Melee) += 0.12f; // 12% more melee damage
			
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<HeroBreastplate>() && legs.type == ModContent.ItemType<HeroGreaves>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
			ParticleSpawnTimer++; // Increment the timer each frame, used to control projectile spawn timing
			player.addDPS(25); // Increase dealt damage for all weapon classes by 25%
            player.moveSpeed *= 0.75f;
			bool isHoldingGoliath = player.HeldItem.type == ModContent.ItemType<Goliath>();
			bool isHoldingGargantua = player.HeldItem.type == ModContent.ItemType<Gargantua>();

			if ((isHoldingGoliath || isHoldingGargantua) && ParticleSpawnTimer > 60) {
				PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRing>(), player.Center, Vector2.Zero, Color.SkyBlue, 1);
				ParticleSpawnTimer = 0; // Reset the timer after spawning the projectile
			}
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			HeroHelmetPlayer modPlayer = player.GetModPlayer<HeroHelmetPlayer>();
			modPlayer.HasHeroHelmet = true;  // Set a flag indicating the helmet is equipped
		}
	
	}
	public class HeroHelmetPlayer : ModPlayer
    {
		public bool HasHeroHelmet;
		public bool IsHeroHelmetGuarding;

		public override void ResetEffects()
		{
			HasHeroHelmet = false;
			IsHeroHelmetGuarding = false;
		}
        public override void ProcessTriggers(TriggersSet triggersSet)
				{
					if (DestroyerTestMod.HeroHelmetKeybind.JustPressed && Player.GetModPlayer<HeroHelmetPlayer>().HasHeroHelmet)
					{
						HeroHelmetPlayer modPlayer = Player.GetModPlayer<HeroHelmetPlayer>();
						modPlayer.IsHeroHelmetGuarding = !modPlayer.IsHeroHelmetGuarding;

						SoundStyle toggleSound = new SoundStyle($"DestroyerTest/Assets/Audio/HeroHelmetToggle");
						SoundEngine.PlaySound(toggleSound);

						if (modPlayer.IsHeroHelmetGuarding)
						{
							Player.head = HeroHelmetHeadSlots.Guarding;
							Player.statDefense += 4;
						}
						else
						{
							Player.head = HeroHelmetHeadSlots.Default;
							Player.statDefense -= 4;
						}
					}
				}
    }
		public static class HeroHelmetHeadSlots
		{
			public static int Default => ModContent.GetModHeadSlot("DestroyerTest/Content/Equips/HeroSet/HeroHelmet_Head");
			public static int Guarding => ModContent.GetModHeadSlot("DestroyerTest/Content/Equips/HeroSet/HeroHelmetGuarding_Head");
		}

}
