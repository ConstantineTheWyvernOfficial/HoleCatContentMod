using System.Collections.Generic;
using DestroyerTest.Content.UI;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Lorebooks
{
    public class BloodGoddessLoreBook : BaseLoreBook
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
                Language.GetText("Mods.DestroyerTest.BookText.BloodGoddessLoreBook.Page1").Value,
                Language.GetText("Mods.DestroyerTest.BookText.BloodGoddessLoreBook.Page2").Value,
                Language.GetText("Mods.DestroyerTest.BookText.BloodGoddessLoreBook.Page3").Value,
                Language.GetText("Mods.DestroyerTest.BookText.BloodGoddessLoreBook.Page4").Value,
            };

            ModContent.GetInstance<TextReaderSystem>().OpenLocalizedBook("BloodGoddessLoreBook", 2000, pages.Count);
            return true;
        }
    }
}
