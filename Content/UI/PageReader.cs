using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace DestroyerTest.Content.UI
{
    public class PageReader : UIState
    {
        private UIPanel panel;
        private UIText pageText;
        private UIText pageNumberText;
        private UIImageButton nextButton;
        private UIImageButton prevButton;
        private UIImageButton CloseButton;
        private bool dragging = false;
        private Vector2 offset;

        private List<string> pages = new List<string>();
        private int currentPage = 0;

        public static bool Visible = false;

        public override void OnInitialize()
        {
            if (Main.dedServ)
                return;

            panel = new UIPanel();
            panel.Width.Set(600f, 0f);
            panel.Height.Set(300f, 0f);
            panel.Left.Set(200f, 0f);
            panel.Top.Set(100f, 0f);
            panel.BackgroundColor = new Color(20, 20, 40, 200);

            panel.OnLeftMouseDown += StartDrag;
            panel.OnLeftMouseUp += EndDrag;
            panel.OnMouseOver += EnableMouseInterface;
            Append(panel);

            pageText = new UIText("", 1.0f, false);
            //pageText.Width.Set(-40f, 1f);
            //pageText.Height.Set(-60f, 1f);
            pageText.Width.Set(0f, 1f); // Full width of parent
            pageText.Height.Set(0f, 1f); // Full height of parent
            pageText.Left.Set(20f, 0f);
            pageText.Top.Set(20f, 0f);
            pageText.TextColor = Color.White;
            panel.Append(pageText);

            pageNumberText = new UIText("Page 1", 0.7f);
            pageNumberText.Top.Set(270f, 0f);
            pageNumberText.Left.Set(260f, 0f);
            panel.Append(pageNumberText);

            nextButton = new UIImageButton(ModContent.Request<Texture2D>("DestroyerTest/Content/Extras/ArrowRight"));
            nextButton.Width.Set(24f, 0f);
            nextButton.Height.Set(24f, 0f);
            nextButton.Left.Set(560f, 0f);
            nextButton.Top.Set(270f, 0f);
            nextButton.OnLeftClick += NextPage;
            panel.Append(nextButton);

            prevButton = new UIImageButton(ModContent.Request<Texture2D>("DestroyerTest/Content/Extras/ArrowLeft"));
            prevButton.Width.Set(24f, 0f);
            prevButton.Height.Set(24f, 0f);
            prevButton.Left.Set(10f, 0f);
            prevButton.Top.Set(270f, 0f);
            prevButton.OnLeftClick += PrevPage;
            panel.Append(prevButton);

            CloseButton = new UIImageButton(ModContent.Request<Texture2D>("DestroyerTest/Content/Extras/CloseButton"));
            CloseButton.Width.Set(24f, 0f);
            CloseButton.Height.Set(24f, 0f);
            CloseButton.Left.Set(8f, 0f);
            CloseButton.Top.Set(8f, 0f);
            CloseButton.OnLeftClick += Close;
            panel.Append(CloseButton);
        }

        public override void OnActivate()
        {
  
            
        }

        public void OpenBook(string bookKey, int maxCharsPerPage, int? pageCount = null)
        {
            LoadLocalizedBook(bookKey, maxCharsPerPage, pageCount);
            Visible = true;
        }

        public void LoadLocalizedBook(string bookKey, int maxCharsPerPage, int? pageCount = null)
        {
            pages.Clear();
            currentPage = 0;

            if (pageCount.HasValue)
            {
                // Static page count: load each page by key
                for (int i = 1; i <= pageCount.Value; i++)
                {
                    string localizationKey = $"Mods.DestroyerTest.BookText.{bookKey}.Page{i}";
                    if (Language.Exists(localizationKey))
                        pages.Add(Language.GetTextValue(localizationKey));
                    else
                        pages.Add("Page missing.");
                }
            }
            else
            {
                // Dynamic: load all text and split
                string localizationKey = $"Mods.DestroyerTest.BookText.{bookKey}";
                string rawText = Language.Exists(localizationKey) ? Language.GetTextValue(localizationKey) : "No content found.";
                LoadText(rawText, maxCharsPerPage);
                return;
            }

            UpdatePageText();
        }

        public void LoadText(string rawText, int maxCharsPerPage)
        {
            pages.Clear();
            string[] words = rawText.Split(' ');
            string currentPageText = "";

            foreach (var word in words)
            {
                if ((currentPageText.Length + word.Length + 1) < maxCharsPerPage)
                {
                    currentPageText += word + " ";
                }
                else
                {
                    pages.Add(currentPageText.Trim());
                    currentPageText = word + " ";
                }
            }

            if (!string.IsNullOrWhiteSpace(currentPageText))
                pages.Add(currentPageText.Trim());

            currentPage = 0;
            UpdatePageText();
        }

        private void StartDrag(UIMouseEvent evt, UIElement listeningElement)
        {
            dragging = true;
            offset = new Vector2(evt.MousePosition.X - panel.Left.Pixels, evt.MousePosition.Y - panel.Top.Pixels);
            // evt.StopPropagation(); // Prevents weird event bubbling
        }

        private void EndDrag(UIMouseEvent evt, UIElement listeningElement)
        {
            dragging = false;
        }

        private void EnableMouseInterface(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.LocalPlayer.mouseInterface = true;
        }


        private void NextPage(UIMouseEvent evt, UIElement listeningElement)
        {
            if (currentPage < pages.Count - 1)
            {
                currentPage++;
                UpdatePageText();
            }
        }

        private void PrevPage(UIMouseEvent evt, UIElement listeningElement)
        {
            if (currentPage > 0)
            {
                currentPage--;
                UpdatePageText();
            }
        }

        private void UpdatePageText()
        {
            if (pages.Count > 0)
            {
                pageText.SetText(pages[currentPage]);
                pageNumberText.SetText($"Page {currentPage + 1} / {pages.Count}");
            }
            else
            {
                pageText.SetText("No content loaded.");
                pageNumberText.SetText("");
            }
        }


        // Old LoadText. Kept in case we need to backtrack.
        /*
        public void LoadText(string rawText)
        {
            pages.Clear();
            const int maxCharsPerPage = 2000;

            string[] words = rawText.Split(' ');
            string currentPageText = "";

            foreach (var word in words)
            {
                if ((currentPageText.Length + word.Length + 1) < maxCharsPerPage)
                {
                    currentPageText += word + " ";
                }
                else
                {
                    pages.Add(currentPageText.Trim());
                    currentPageText = word + " ";
                }
            }

            if (!string.IsNullOrWhiteSpace(currentPageText))
                pages.Add(currentPageText.Trim());

            currentPage = 0;
            UpdatePageText();
        }
        */

        public override void Update(GameTime gameTime)
        {
            if (Visible)
            {
                base.Update(gameTime);
            }

            if (Visible)
            {
                if (dragging)
                {
                    Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY);
                    panel.Left.Set(mouse.X - offset.X, 0f);
                    panel.Top.Set(mouse.Y - offset.Y, 0f);
                    panel.Recalculate(); // Important to update its internal layout
                }
            }

            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

        public void ToggleVisibility(string bookKey)
        {
            if (!Visible)
            {
                LoadLocalizedBook(bookKey);
            }

            Visible = !Visible;
        }

        public void Close(UIMouseEvent evt, UIElement listeningElement)
        {
            Visible = false;
        }

        public void LoadLocalizedBook(string bookKey)
        {
            pages.Clear();
            currentPage = 0;

            int pageIndex = 1;
            while (true)
            {
                string localizationKey = $"Mods.DestroyerTest.BookText.{bookKey}.Page{pageIndex}";
                if (!Language.Exists(localizationKey))
                    break;

                pages.Add(Language.GetTextValue(localizationKey));
                pageIndex++;
            }

            if (pages.Count == 0)
            {
                pages.Add("No pages found for this book.");
            }

            UpdatePageText();
        }
    }
}
