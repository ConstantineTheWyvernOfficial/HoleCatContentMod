using System;
using DestroyerTest.Content.MeleeWeapons;
using DestroyerTest.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Equips
{
    public class Connie_Doll : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 2500;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 26;
            Item.maxStack = 1;
            Item.value = 100000;
            Item.rare = ItemRarityID.White;

        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Player player = Main.LocalPlayer;
            if (Item.shimmerWet || Item.CanShimmer())
            {
                SoundEngine.PlaySound(new SoundStyle("DestroyerTest/Assets/Audio/ConnieDollScream"), Item.position); // explosion sound
                Projectile.NewProjectile(
                    Item.GetSource_FromThis(),
                    Item.Center,
                    player.Center - Item.Center,
                    ModContent.ProjectileType<ConstantineDollProjectile>(),
                    50000,
                    20,
                    Main.myPlayer
                );
                Item.NewItem(Item.GetSource_FromThis(), player.Center, ModContent.ItemType<ConstantineScythe>());
                Item.active = false;
            }
        }   
        

        



    }

    public class CD_DROP_NPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {

            if (npc.type == NPCID.MoonLordCore)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Connie_Doll>(), 1, 1, 5));
            }

        }
    }
    
   

}
