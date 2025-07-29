using DestroyerTest.Content.Buffs;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Content.SummonItems;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System;
using InnoVault.PRT;

namespace DestroyerTest.Content.Equips.AuraThiefSet
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class AuraThiefHeadgear : ModItem
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
			Item.width = 36; // Width of the item
			Item.height = 28; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ModContent.RarityType<LifeEchoRarity>(); // The rarity of the item
			Item.defense = 4; // The amount of defense the item will give when equipped
		}

	
		/*
        public override void EquipFrameEffects(Player player, EquipType type)
        {
            if (player.armor[0].type == ModContent.ItemType<AuraThiefHeadgear>()) // Ensure player is wearing the headgear
			{
				Texture2D glowmaskTexture = ModContent.Request<Texture2D>("DestroyerTest/Content/Equips/AuraThiefSet/AuraThiefHeadgear_Head_Highlight").Value;

				Vector2 position = player.headPosition + new Vector2(player.width / 2, player.height / 2) - Main.screenPosition;

				// Draw the glowmask
				Main.spriteBatch.Draw(
					glowmaskTexture,
					position,
					null,
					Color.White,
					player.headRotation,
					new Vector2(glowmaskTexture.Width / 2, glowmaskTexture.Height / 2),
					1f,
					SpriteEffects.None,
					0f
				);
			}
        }
		*/

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AuraThiefBreastplate>() && legs.type == ModContent.ItemType<AuraThiefCuisses>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
			ParticleSpawnTimer++; // Increment the timer each frame, used to control projectile spawn timing
            bool isHoldingSoulEdge = player.HeldItem.type == ModContent.ItemType<SoulEdge>();
			bool isHoldingTrueSoulEdge = player.HeldItem.type == ModContent.ItemType<TrueSoulEdge>();

			if ((isHoldingSoulEdge|| isHoldingTrueSoulEdge) && ParticleSpawnTimer > 60) {
				PRTLoader.NewParticle(PRTLoader.GetParticleID<BloomRing>(), player.Center, Vector2.Zero, Color.SkyBlue, 1);
				ParticleSpawnTimer = 0; // Reset the timer after spawning the projectile
			}
			
		}

         public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<LifeEcho>(16)
                .AddIngredient(ItemID.Wood, 10)
                .AddIngredient(ItemID.FlinxFur, 10)
				.AddTile(TileID.Anvils)
				.Register();
        }
	}
}