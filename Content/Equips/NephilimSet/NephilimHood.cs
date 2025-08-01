using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Content.SummonItems;
using DestroyerTest.Content.Tiles;
using InnoVault.PRT;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Equips.NephilimSet
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class NephilimHood : ModItem
	{
		public int ParticleSpawnTimer = 0;


		public override void SetStaticDefaults() {
			// If your head equipment should draw hair while drawn, use one of the following:
			//ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			// ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			//ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;

		}

		public override void SetDefaults() {
			Item.width = 32; // Width of the item
			Item.height = 28; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 5; // The amount of defense the item will give when equipped
		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<NephilimChestPlate>() && legs.type == ModContent.ItemType<NephilimBoots>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
			ParticleSpawnTimer++; // Increment the timer each frame, used to control projectile spawn timing
			player.meleeEnchant = 4;
			player.armorEffectDrawShadowLokis = true;
			bool isHoldingLaevateinn = player.HeldItem.type == ModContent.ItemType<Laevateinn>();
			bool isHoldingTrueLaevateinn = player.HeldItem.type == ModContent.ItemType<TrueLaevateinn>();

			if ((isHoldingLaevateinn || isHoldingTrueLaevateinn) && ParticleSpawnTimer > 60) {
				PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRing>(), player.Center, Vector2.Zero, Color.SkyBlue, 1);
				ParticleSpawnTimer = 0; // Reset the timer after spawning the projectile
			}
			
		}

         public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }


	}
}