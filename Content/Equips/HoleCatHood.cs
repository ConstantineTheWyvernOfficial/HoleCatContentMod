

using DestroyerTest.Content.Resources;
using DestroyerTest.Content.Resources.Cloths;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Equips
{

	[AutoloadEquip(EquipType.Head)]
	public class HoleCatHood : ModItem
	{
		public static LocalizedText SetBonusText { get; private set; }
		public override void SetStaticDefaults()
		{
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
			SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(DamageBonus, UseSpeedBonus, KnockbackBonus);
			
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 36; 
			Item.value = Item.sellPrice(gold: 70);
			Item.rare = ItemRarityID.Expert;
			Item.defense = 23;
		}

		
		public override bool IsArmorSet(Item head, Item body, Item legs) {
		return body.type == ModContent.ItemType<HoleCatShawl>() && legs.type == ModContent.ItemType<HoleCatSkirt>();
		}
		
		public float DamageBonus = 1.25f;
        public float UseSpeedBonus =1.05f;
        public float KnockbackBonus = 1.10f;


		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = SetBonusText.Value;
			player.GetDamage(DamageClass.Generic) *= DamageBonus;
			player.GetAttackSpeed(DamageClass.Generic) *= UseSpeedBonus;
			player.GetKnockback(DamageClass.Generic) *= KnockbackBonus;
			player.setBonus = SetBonusText.Format(
				DamageBonus - 1f,
				UseSpeedBonus - 1f,
				KnockbackBonus - 1f
			);
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.LunarBar, 15)
				.AddIngredient(ItemID.GoldBar, 17)
				.AddIngredient(ItemID.LargeRuby, 1)
                .AddIngredient<WhiteCloth>(35)
                .AddIngredient<Tenebris>(12)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
