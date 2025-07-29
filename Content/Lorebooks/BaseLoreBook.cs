using DestroyerTest.Content.UI;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

public abstract class BaseLoreBook : ModItem
{
    public virtual string GetBookKey() => "";
    public virtual int GetMaxCharsPerPage() => 2000;
    public virtual int? GetPageCount() => null; // Optional: override for static page count

    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 38;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useTurn = true;
        Item.noUseGraphic = true;
        Item.UseSound = new SoundStyle("DestroyerTest/Assets/Audio/Pageturn");
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