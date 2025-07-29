using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Content.Tiles;
using DestroyerTest.Content.Tiles.RiftConfigurator;
using DestroyerTest.Content.Tiles.Riftplate;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.MeleeWeapons.SwordLineage
{

    public class GenesisButterfly : ModItem
    {

        public override void SetDefaults()
        {
            // Common Properties
            Item.width = 66;
            Item.height = 66;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item100;


            // Use Properties
            // Note that useTime and useAnimation for this item don't actually affect the behavior because the held projectile handles that. 
            // Each attack takes a different amount of time to execute
            // Conforming to the item useTime and useAnimation makes it much harder to design
            // It does, however, affect the item tooltip, so don't leave it out.
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;

            // Weapon Properties
            Item.autoReuse = true;
            Item.knockBack = 7;  // The knockback of your sword, this is dynamically adjusted in the projectile code.
            Item.damage = 22; // The damage of your sword, this is dynamically adjusted in the projectile code.
            Item.DamageType = DamageClass.Generic; // Deals melee damage

            Item.shoot = ModContent.ProjectileType<Projectiles.GenesisButterfly>();
            Item.shootSpeed = 23f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            damageDone = (int)(damageDone * 2.30f);
        }

        public override bool MeleePrefix()
        {
            return true; // return true to allow weapon to have melee prefixes (e.g. Legendary)
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddIngredient(ItemID.CrystalShard, 9)
                .AddIngredient(ItemID.PurpleEmperorButterfly, 1)
                .AddCondition(Condition.DownedEmpressOfLight)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    
    public class GB_DROP_NPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {

            if (npc.type == NPCID.EmpressButterfly)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GenesisButterfly>(), 1, 1, 1));
            }

        }
    }
}