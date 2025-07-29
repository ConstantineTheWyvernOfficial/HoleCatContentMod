using System.Collections.Generic;
using DestroyerTest.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Lorebooks
{
    public class ShadeSeedBook : BaseLoreBook
    {

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override bool? UseItem(Player player)
        {
            if (PageReader.Visible)
                return true;

            var pages = new List<string>()
            {
                Language.GetText("Mods.DestroyerTest.BookText.ShadeSeedBook.Page1").Value,
                Language.GetText("Mods.DestroyerTest.BookText.ShadeSeedBook.Page2").Value,
            };

            ModContent.GetInstance<TextReaderSystem>().OpenLocalizedBook("ShadeSeedBook", 2000, pages.Count);
            return true;
        }
    }
}
