using DestroyerTest.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


public class BookUIOpener : BaseLoreBook
{
    public override string Texture => "DestroyerTest/Content/SummonItems/VanxarkBuff";
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 30;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useTurn = true;
        Item.UseSound = SoundID.Item1;
        Item.rare = ItemRarityID.Blue;
        Item.maxStack = 1;
    }

    public override bool? UseItem(Player player)
    {
        if (PageReader.Visible)
            return true;

        ModContent.GetInstance<TextReaderSystem>().OpenLocalizedBook(GetBookKey(), GetMaxCharsPerPage(), GetPageCount());
        return true;
    }
}
