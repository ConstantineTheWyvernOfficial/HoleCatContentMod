using DestroyerTest.Content.Projectiles;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Common
{
    /// <summary>
	/// All the stats that are editable for the <b>Scepter Damage</b> class.
	/// These only apply to items with the <b>Scepter Damage</b> class.
    /// 
    /// <para/> <b>DamageModifier</b> - A value that can be operated on to alter the amount of damage dealt by the projectiles. This does not change the damage dealt by the weapons.
    /// <para/> <b>Range</b> - An integer value that can be altered to increase or decrease the range of a thrown projectile (how far it goes before returning to the player).  
    /// <para/> <b>Note:</b> Because the thrown projectiles run on a timer, range value measures in <b>In-Game ticks</b>. Also, it is recommended to use addition and subtraction for range. It defaults to <b>0</b>, so if you try to divide or multiply, it will cause problems.
    /// <para/> <b>SizeModifier</b> - This float controls the size of the thrown projectiles. This value can tend to be finicky due to the scepters' tile-bouncing AI stopping them from properly reaching the player past a certain size.
    /// <para/> <b>ManaBurstPower</b> - This integer is for a not-yet-implemented mechanic involving the player's mana. This currently does nothing.
	/// </summary>
	public class ScepterClassStats
    {   
        /// <summary>
        /// A value that can be operated on to alter the amount of damage dealt by the projectiles. This does not change the damage dealt by the weapons. Be careful with this value though, as most damage in terrar
        /// </summary>
        public static float DamageModifier { get; set; } = 1.0f;

        /// <summary>
        /// An integer value that can be altered to increase or decrease the range of a thrown projectile (how far it goes before returning to the player).
        /// <para/><b>Note:</b> Because the thrown projectiles run on a timer, range value measures in <b>In-Game ticks</b>. Also, it is recommended to use addition and subtraction for range. It defaults to <b>0</b>, so if you try to divide or multiply, it will cause problems.
        /// </summary>
        public static int Range { get; set; } = 0;

        /// <summary>
        /// This float controls the size of the thrown projectiles. This value can tend to be finicky due to the scepters' tile-bouncing AI stopping them from properly reaching the player past a certain size.
        /// <para/><b>Note:</b> This value is used best as an additive or subtractive value, due to being an integer. If you want a precise multiplication, use SizeMultiplier, SizeModifier's younger cousin.
        /// </summary>
        public static int SizeModifier { get; set; } = 1;

        public static float SizeMultiplier { get; set; } = 1.0f;

        /// <summary>
        /// This integer is for a not-yet-implemented mechanic involving the player's mana. This currently does nothing.
        /// </summary>
        public static int ManaBurstPower { get; set; } = 0;

        /// <summary>
        /// The scepter class has boolean values for certain accessories to more easily determine associated behavior. Some Accessories only alter stats, while others introduce new behaviors and projectiles.
        /// <para/> This boolean is for the Blood Vial accessory.
        /// </summary>
        public static bool BloodVialItem { get; set; } = false;

         /// <summary>
        /// The scepter class has boolean values for certain accessories to more easily determine associated behavior. Some Accessories only alter stats, while others introduce new behaviors and projectiles.
        /// <para/> This boolean is for the Vile Cyst accessory.
        /// </summary>
        public static bool VileCystItem { get; set; } = false;
    }

    public class ApplyAccessoryStats : ModSystem
    {
        public override void PostUpdateTime()
        {
            foreach (var projectile in Main.projectile)
            {
                if (projectile.active && projectile.DamageType == ModContent.GetInstance<ScepterClass>() && Main.rand.NextBool(16))
                {
                    if (ScepterClassStats.BloodVialItem)
                    {
                        ScepterClassStats.VileCystItem = false;
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, projectile.velocity / 4, ModContent.ProjectileType<BloodBlob>(), projectile.damage, 2f);
                    }

                    if (ScepterClassStats.VileCystItem)
                    {
                        ScepterClassStats.BloodVialItem = false;
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position, projectile.velocity / 4 , ModContent.ProjectileType<PusBlob>(), projectile.damage, 2f);
                    }
                }
            }
        }
    }
}
