using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Resources;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;
using Microsoft.Xna.Framework.Input;
using Terraria.GameInput;
using Terraria.Audio;
using System.Linq;
using Microsoft.Xna.Framework;
using DestroyerTest.Content.Projectiles;
using DestroyerTest.Content.Resources.Cloths;
using DestroyerTest.Content.Buffs;

namespace DestroyerTest.Content.Equips
{
    // This item is meant to mirror the effects of the Hallowed Plate Mail, which equips a Cape without needing a separate cape Item. 

    [AutoloadEquip(EquipType.Body)] // As usual, we must tell the game what part of the body the item will be equipped on.
    public class HoleCatShawl : ModItem
    {
        public int equipBack = -1; // It would be best not to tamper with this.

        public override void Load() // This fetches the texture we need

        {
            if (Main.netMode != NetmodeID.Server)
            {
                equipBack = EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Back}", EquipType.Back, this);
            }
        }

        public override void SetStaticDefaults() // These will display the texture we fetched, and are specifically for this purpose.
        {
            ArmorIDs.Body.Sets.IncludedCapeBack[Item.bodySlot] = equipBack;
            ArmorIDs.Body.Sets.IncludedCapeBackFemale[Item.bodySlot] = equipBack;
        }
        public override void SetDefaults() // Simple item properties. Nothing new here.
        {
            Item.width = 34;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Expert; // The rarity of the item
            Item.defense = 40;
            Active = false;
        }

        

        public bool Active = false;
        public override void UpdateEquip(Player player)
        {
            Active = true;
            
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 22)
                .AddIngredient(ItemID.GoldBar, 20)
                .AddIngredient<WhiteCloth>(45)
                .AddIngredient<Tenebris>(15)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
    }
    
    
    public class HCSPlayer : ModPlayer
    {
        public int oilFireCooldown = 0;
        public const int MaxCooldown = 120; // 2 seconds at 60fps
        public const int SpamPenaltyDebuffTime = 80; // 1.33 seconds
        public bool SoundPlayed = false;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (oilFireCooldown > 0)
                oilFireCooldown--;

            if (oilFireCooldown <= 0)
            {
                if (SoundPlayed == false)
                {
                    SoundEngine.PlaySound(SoundID.Item35, Player.Center);
                    SoundPlayed = true;
                }
            }

            foreach (Item accessory in Player.armor)
            {
                if (accessory.ModItem is HoleCatShawl shawl)
                {
                    if (DestroyerTestMod.OilTentacleKeybind.JustPressed && shawl.Active)
                    {
                        // Check for cooldown abuse
                        if (oilFireCooldown > 0)
                        {
                            // Apply debuff if the player is spamming
                            oilFireCooldown = MaxCooldown;
                            Player.AddBuff(ModContent.BuffType<MagicFatigue>(), SpamPenaltyDebuffTime);
                            SoundEngine.PlaySound(SoundID.Item42, Player.Center); // Optional "failure" sound
                            SoundPlayed = false;
                            return; // cancel firing
                        }

                        oilFireCooldown = MaxCooldown; // reset cooldown

                        Vector2 ToCursor = (Main.MouseWorld - Player.Center);
                        ToCursor.SafeNormalize(Vector2.UnitX);
                        Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, ToCursor.ToRotation());

                        SoundEngine.PlaySound(new SoundStyle("DestroyerTest/Assets/Audio/NightmareRose/SoulSummon"), Player.Center);

                        for (int r = 0; r < 6; r++)
                        {
                            Vector2 direction = Main.MouseWorld - Player.Center;
                            direction *= 0.025f;
                            direction = direction.RotatedBy(Main.rand.NextFloat(-MathHelper.ToRadians(15), MathHelper.ToRadians(15)));

                            Projectile.NewProjectile(
                                Entity.GetSource_Accessory(Player.armor.FirstOrDefault(item => item.type == ModContent.ItemType<HoleCatShawl>())),
                                Player.Center,
                                direction,
                                ModContent.ProjectileType<OilProjectile>(),
                                160,
                                4,
                                Main.myPlayer,
                                default,
                                default,
                                1
                            );
                        }
                    }
                }
            }
        }
    }
}