using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Buffs;

namespace DestroyerTest.Content.MeleeWeapons
{
    public class ContenderSword : ModItem
    {
        private int currentState = 0; // 0 = Jab, 1 = Dart, 2 = Energy

        // Weapon Properties
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 78;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Pink;

            // Base Properties (Default to Dart State)
            SetDartState();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // Cycle to next state on right-click
                currentState = (currentState + 1) % 3;
                SoundEngine.PlaySound(SoundID.Item35);
                SetStateProperties();
                CombatText.NewText(player.getRect(), Color.Red, $"State Changed: {GetStateName()}");
                return false; // Don't use the item immediately after changing state
            }

            SetStateProperties(); // Apply current state properties
            return base.CanUseItem(player);
        }

        private void SetStateProperties()
        {
            switch (currentState)
            {
                case 0:
                    SetJabState();
                    break;
                case 1:
                    SetDartState();
                    break;
                case 2:
                    SetEnergyState();
                    break;
            }
        }

        private void SetJabState()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.shoot = ModContent.ProjectileType<ContenderSwordJab>();
            Item.shootSpeed = 2.0f;
            Item.damage = 5600;
            Item.knockBack = 20;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = false;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item131;
            Item.autoReuse = true;
        }

        private void SetDartState()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.shoot = ModContent.ProjectileType<ContenderSwordShot>();
            Item.shootSpeed = 12f;
            Item.damage = 5000;
            Item.knockBack = 30;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item104;
            Item.autoReuse = true;
        }

        private void SetEnergyState()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.shoot = ModContent.ProjectileType<ContenderSwordEnergy>();
            Item.shootSpeed = 36f;
            Item.damage = 5600;
            Item.knockBack = 35;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (currentState == 0 && hit.Crit) // Jab State: Healing on hit
            {
                target.AddBuff(ModContent.BuffType<HateShock>(), 240);
            }
            else if (currentState == 1)
            {
                target.AddBuff(ModContent.BuffType<HateWither>(), 240);
            }
            else if (currentState == 2)
            {
                target.AddBuff(ModContent.BuffType<HateWither>(), 240);
            }
        }

        private string GetStateName()
        {
            return currentState switch
            {
                0 => "Jab State",
                1 => "Dart State",
                2 => "Energy State",
                _ => "Unknown State"
            };
        }

    }
}
