using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.Audio;
using Terraria.ModLoader.UI;
using Terraria.ID;
using System.Collections.Generic;
using DestroyerTest.Content.UI;
using System;
public class TextReaderSystem : ModSystem
{
    internal PageReader bookUI;
    private UserInterface bookInterface;

    public override void Load()
    {
        if (!Main.dedServ)
        {
            bookUI = new PageReader();
            bookUI.Activate();
            bookInterface = new UserInterface();
            bookInterface.SetState(bookUI);
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (!Main.dedServ)
        {
            if (PageReader.Visible)
            {
                bookInterface?.Update(gameTime);
            }
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
        if (inventoryIndex != -1 && PageReader.Visible)
        {
            layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
                "DestroyerTest: Book UI",
                () => { bookInterface.Draw(Main.spriteBatch, new GameTime()); return true; },
                InterfaceScaleType.UI)
            );
        }
    }


    public void OpenLocalizedBook(string bookKey, int maxCharsPerPage, int? pageCount = null)
    {
        bookUI.OpenBook(bookKey, maxCharsPerPage, pageCount);
    }

}
