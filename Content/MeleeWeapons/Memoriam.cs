
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using DestroyerTest.Content.Projectiles;

namespace DestroyerTest.Content.MeleeWeapons
{
    /// <summary>
    /// This weapon and its corresponding projectile showcase the CloneDefaults() method, which allows for cloning of other items.
    /// For this example, we shall copy the Meowmere and its projectiles with the CloneDefaults() method, while also changing them slightly.
    /// For a more detailed description of each item field used here, check out <see cref="ExampleSword" />.
    /// </summary>
    public class Memoriam : ModItem
    {
        public override void SetDefaults() {
            // This method right here is the backbone of what we're doing here; by using this method, we copy all of
            // the meowmere's SetDefault stats (such as Item.melee and Item.shoot) on to our item, so we don't have to
            // go into the source and copy the stats ourselves. It saves a lot of time and looks much cleaner; if you're
            // going to copy the stats of an item, use CloneDefaults().

            Item.CloneDefaults(ItemID.Meowmere);

            // After CloneDefaults has been called, we can now modify the stats to our wishes, or keep them as they are.
            // For the sake of example, let's swap the vanilla Meowmere projectile shot from our item for our own projectile by changing Item.shoot:

            Item.shoot = ModContent.ProjectileType<SoulOfLight_Projectile>(); // Remember that we must use ProjectileType<>() since it is a modded projectile!
            Item.shoot = ModContent.ProjectileType<SoulOfNight_Projectile>(); // Remember that we must use ProjectileType<>() since it is a modded projectile!
            // Check out ExampleCloneProjectile to see how this projectile is different from the Vanilla Meowmere projectile.

            // While we're at it, let's make our weapon's stats a bit stronger than the Meowmere, which can be done
            // by using math on each given stat.

            Item.damage = 35; // Makes this weapon's damage half the Meowmere's damage.
            Item.shootSpeed = 2f; // Makes this weapon's projectiles shoot 25% faster than the Meowmere's projectiles.
            Item.useTime = 40;
            Item.useAnimation = 40; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
            Item.crit *= 0;
            Item.UseSound = new SoundStyle($"DestroyerTest/Assets/Audio/SOJ-M_Slash") with {
				Volume = 1.0f, 
    			Pitch = 0.0f, 
    			PitchVariance = 1.0f, 
			}; // The sound when the weapon is being used.
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            // Calculate the speed of the projectile
            float speed = velocity.Length();

            // Define the angles based on the player's facing direction
            float angle1, angle2;
            if (player.direction == -1) { // Facing left
                angle1 = 225f;
                angle2 = 140f;
            } else { // Facing right
                angle1 = -45f;
                angle2 = -315f;
            }

            // Calculate the velocity for the first projectile
            Vector2 velocity1 = new Vector2(speed * (float)Math.Cos(MathHelper.ToRadians(angle1)), speed * (float)Math.Sin(MathHelper.ToRadians(angle1)));

            // Calculate the velocity for the second projectile
            Vector2 velocity2 = new Vector2(speed * (float)Math.Cos(MathHelper.ToRadians(angle2)), speed * (float)Math.Sin(MathHelper.ToRadians(angle2)));

            // Fire the first projectile (SoulOfLight_Projectile)
            Projectile.NewProjectile(source, position, velocity1, ModContent.ProjectileType<SoulOfLight_Projectile>(), damage, knockback, player.whoAmI);

            // Fire the second projectile (SoulOfNight_Projectile)
            Projectile.NewProjectile(source, position, velocity2, ModContent.ProjectileType<SoulOfNight_Projectile>(), damage, knockback, player.whoAmI);

            // Return false to prevent the default projectile from being fired
            return false;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Katana, 1)
                .AddIngredient(ItemID.HallowedBar, 15)
                .AddIngredient(ItemID.SoulofLight, 2)
                .AddIngredient(ItemID.SoulofNight, 2)
                .AddIngredient(ItemID.Daybloom, 1)
                .AddIngredient(ItemID.Deathweed, 1)
                .AddIngredient(ItemID.Blinkroot, 1)
                .AddIngredient(ItemID.Waterleaf, 1)
                .AddIngredient(ItemID.Fireblossom, 1)
                .AddIngredient(ItemID.Shiverthorn, 1)
                .AddIngredient(ItemID.Moonglow, 1)
                .AddCondition(Condition.Hardmode)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}