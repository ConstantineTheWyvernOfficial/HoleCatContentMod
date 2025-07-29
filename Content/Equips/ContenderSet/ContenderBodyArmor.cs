using DestroyerTest.Content.MetallurgySeries;
using DestroyerTest.Content.Resources;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DestroyerTest.Rarity;

namespace DestroyerTest.Content.Equips.ContenderSet
{
// This item is meant to mirror the effects of the Hallowed Plate Mail, which equips a Cape without needing a separate cape Item. 

	[AutoloadEquip(EquipType.Body)] // As usual, we must tell the game what part of the body the item will be equipped on.
    	public class ContenderBodyArmor : ModItem
		{
        public int equipBack = -1; // It would be best not to tamper with this.
        
        public override void Load() // This fetches the texture we need

        { 
            if (Main.netMode != NetmodeID.Server) {
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
			Item.width = 36;
			Item.height = 24; 
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ModContent.RarityType<ContenderRarity>();
			Item.defense = 80;
			// Now, in case you might be asking "Why use that special default when you can just copy what the original Hallowed Plate Mail does?"
			// Unfortunately for you, while cloning the defaults does load a cape on the back, it loads the Hallowed Armor cape, and replaces your body armor textures with the Hallowed Plate Mail Textures.
			//Item.CloneDefaults(ItemID.HallowedPlateMail);
		}

        public override bool IsArmorSet(Item head, Item body, Item legs) {
			return legs.type == ModContent.ItemType<ContenderGreaves>() && body.type == ModContent.ItemType<ContenderBodyArmor>();
		}

        public override void UpdateArmorSet(Player player) {
			player.addDPS(10); // Increase dealt damage for all weapon classes by 25%
            player.GetAttackSpeed<RangedDamageClass>() *= 1.75f;
            player.GetAttackSpeed<MeleeDamageClass>() *= 1.75f;
            player.GetAttackSpeed<MagicDamageClass>() *= 2.5f;
            player.maxMinions += 5;
            player.maxTurrets += 3;
            player.jumpSpeedBoost += 3;
            player.moveSpeed *= 2.3f;
		}

		
	}
}