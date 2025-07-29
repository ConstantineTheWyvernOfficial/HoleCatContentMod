using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using DestroyerTest.Common;
using DestroyerTest.Content.SummonItems;
using DestroyerTest.Content.MeleeWeapons;

using Terraria.Localization;
using DestroyerTest.Content.Entity;
using System.Linq;
using DestroyerTest.Content.Projectiles;

namespace DestroyerTest.Content.SummonItems
{
    public class PossessedDartRifleItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;

            ItemID.Sets.StaffMinionSlotsRequired[Type] = 10f; // The default value is 1, but other values are supported. See the docs for more guidance. 
        }

        public override void SetDefaults()
        {
            Item.damage = 220;
            Item.knockBack = 0f;
            Item.mana = 30; // mana cost
            Item.width = 62;
            Item.height = 32;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.RaiseLamp; // how the player's arm moves when using the item
            Item.value = 18000;
            Item.rare = ModContent.RarityType<CorruptionSpecialRarity>(); // The rarity of the item
            Item.UseSound = SoundID.Item64;
            // These below are needed for a minion weapon
            Item.noMelee = true; // this item doesn't do any melee damage
            Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
            Item.buffType = ModContent.BuffType<DartRifleMinionBuff>();
            // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
            Item.shoot = ModContent.ProjectileType<PossessedDartRifle>(); // This item creates the minion projectile
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
            position = Main.MouseWorld;
        }


        // Define minionTypes as a class field so both methods can access it

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Apply the buff to the player to keep the minion alive
            player.AddBuff(Item.buffType, 2);

            // Spawn the minion projectile
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<PossessedDartRifle>(), damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            // Prevent the game from spawning another projectile automatically
            return false;
        }
    }
}