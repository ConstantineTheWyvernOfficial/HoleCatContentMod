using System;
using System.Configuration;
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
using DestroyerTest.Content.Tiles;
using InnoVault.PRT;

namespace DestroyerTest.Content.Equips.SaviorSet
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class SaviorHelmet : ModItem
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
			Item.defense = 8; // The amount of defense the item will give when equipped
		}

        public override void UpdateEquip(Player player) {
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<SaviorPlatemail>() && legs.type == ModContent.ItemType<SaviorGreaves>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
			ParticleSpawnTimer++; // Increment the timer each frame, used to control projectile spawn timing
			player.addDPS(35); // Increase dealt damage for all weapon classes by 25%
			player.moveSpeed *= 1.25f;

			bool isHoldingGoliath = player.HeldItem.type == ModContent.ItemType<Goliath>();
			bool isHoldingGargantua = player.HeldItem.type == ModContent.ItemType<Gargantua>();

			if ((isHoldingGoliath || isHoldingGargantua) && ParticleSpawnTimer > 60) {
				PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRing>(), player.Center, Vector2.Zero, Color.SkyBlue, 1);
				ParticleSpawnTimer = 0; // Reset the timer after spawning the projectile
			}

			// Kill the projectile if neither Goliath nor Gargantua is held
			if (!isHoldingGoliath && !isHoldingGargantua) {
				foreach (Projectile proj in Main.projectile) {
					if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<SaviorEmpowermentDrawEntity>()) {
						proj.Kill();
					}
				}
			}
		}

        public override void ArmorSetShadows(Player player)
        {
           player.armorEffectDrawOutlinesForbidden = true; // or whatever action you're trying to trigger
        }
	
	}
}
