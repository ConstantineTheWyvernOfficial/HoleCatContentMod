using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using DestroyerTest.Content.Projectiles; // Add this line if CT3_Swing is in the Projectiles namespace
using DestroyerTest.Rarity;
using DestroyerTest.Common;
using System;

namespace DestroyerTest.Content.MeleeWeapons
{
	public class ConstantineScythe : ModItem
	{

		//Weapon Properties
		public override void SetDefaults()
		{
			// Common Properties
			Item.width = 56;
			Item.height = 52;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.rare = ModContent.RarityType<TestRarity>();

			// Use Properties
			// Note that useTime and useAnimation for this item don't actually affect the behavior because the held projectile handles that. 
			// Each attack takes a different amount of time to execute
			// Conforming to the item useTime and useAnimation makes it much harder to design
			// It does, however, affect the item tooltip, so don't leave it out.
			Item.useTime = 100;
			Item.useAnimation = 100;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/CS_Slice") with
			{
				Volume = 1.0f,
				Pitch = 0.0f,
				PitchVariance = 0.5f,
			}; // The sound when the weapon is being used.

			// Weapon Properties
			Item.knockBack = 30;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
			Item.autoReuse = true; // This determines whether the weapon has autoswing
			Item.damage = 480; // The damage of your sword, this is dynamically adjusted in the projectile code.
			Item.DamageType = DamageClass.Melee; // Deals melee damage
			Item.crit = 16; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.
			Item.noMelee = true;  // This makes sure the item does not deal damage from the swinging animation
			Item.noUseGraphic = true; // This makes sure the item does not get shown when the player swings his hand

			// Projectile Properties
			Item.shoot = ModContent.ProjectileType<ConstantineScytheProjectile>(); // The sword as a projectile
		}
		/*
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
				float numberProjectiles = 3 + Main.rand.Next(3); // 3, 4, or 5 shots
				float rotation = MathHelper.ToRadians(45);

				position += Vector2.Normalize(velocity) * 45f;
				velocity *= 0.2f; // Slow the projectile down to 1/5th speed so we can see it. This is only here because this example shares ModItem.SetDefaults code with other examples. If you are making your own weapon just change Item.shootSpeed as normal.

				for (int i = 0; i < numberProjectiles; i++) {
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
					Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
				}

				return false; // return false to stop vanilla from calling Projectile.NewProjectile.
			}
		*/
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(2))
			{
				// Emit dusts when the sword is swung
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Granite);
			}
		}

		public SoundStyle Tick = new SoundStyle("DestroyerTest/Assets/Audio/IncreaseTick") with { PitchVariance = 0.7f, Volume = 0.1f, MaxInstances = 1200 };

		public bool hasplayefullsound = false;
		public override void UpdateInventory(Player player)
		{
			var modPlayer = player.GetModPlayer<MeleeImmunityPlayer>();

			if (player.HeldItem == Item)
			{
				if (modPlayer.Timer < modPlayer.ImmunityThreshold2)
				{
					FullCharge = false;

					SoundEngine.PlaySound(Tick, player.Center);
					modPlayer.Timer += (int)(1 * (1f + modPlayer.TimeSpeed));
					modPlayer.Timer = Math.Min(modPlayer.Timer, modPlayer.ImmunityThreshold2);
				}
				if (modPlayer.Timer >= modPlayer.ImmunityThreshold2)
				{
					if (hasplayefullsound == false)
					{
						SoundEngine.PlaySound(SoundID.Item129, player.Center);
						CombatText.NewText(player.getRect(), Color.Aquamarine, "Immunity on next swing!", true, false);
						hasplayefullsound = true;
					}
					FullCharge = true;
				}
			}

			base.UpdateInventory(player);
		}


		public override bool? UseItem(Player player)
		{
			FullChargeImmunity(player);
			return true;
		}

		public bool FullCharge = false;

		public void FullChargeImmunity(Player me)
		{
			var modPlayer = me.GetModPlayer<MeleeImmunityPlayer>();
			if (modPlayer.Timer >= modPlayer.ImmunityThreshold2 && FullCharge == true)
			{

				me.immune = true;
				me.immuneTime = 60;
				modPlayer.Timer = 0;
				FullCharge = false;
				hasplayefullsound = false;
			}
			else
			{
				ResetOnUse(me);
			}
		}

		public void ResetOnUse(Player me)
		{
			
			var modPlayer = me.GetModPlayer<MeleeImmunityPlayer>();
			SoundEngine.PlaySound(SoundID.Item130, me.Center);
			modPlayer.Timer = 0;
		}

    }
} 