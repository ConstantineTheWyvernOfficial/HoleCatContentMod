using System.Collections.Generic;
using DestroyerTest.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Lorebooks
{
    public class LivingShadowLoreBook : BaseLoreBook
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
                Language.GetText("Mods.DestroyerTest.BookText.LivingShadowLoreBook.Page1").Value,
                Language.GetText("Mods.DestroyerTest.BookText.LivingShadowLoreBook.Page2").Value,
                Language.GetText("Mods.DestroyerTest.BookText.LivingShadowLoreBook.Page3").Value,
            };

            ModContent.GetInstance<TextReaderSystem>().OpenLocalizedBook("LivingShadowLoreBook", 2000, pages.Count);
            return true;
        }

    }
}
