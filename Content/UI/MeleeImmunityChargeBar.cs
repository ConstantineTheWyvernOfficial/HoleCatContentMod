using DestroyerTest.Common;
using DestroyerTest.Content.RiftArsenal;
using DestroyerTest.Content.MeleeWeapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace DestroyerTest.Content.UI
{
	// This custom UI will show whenever the player is holding the ExampleCustomResourceWeapon item and will display the player's custom resource amounts that are tracked in ExampleResourcePlayer
	internal class MeleeImmunityChargeBar : UIState
	{
		// For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
		// Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
		private UIText text;
		private UIElement area;
		private UIImage barFrame;
		private Color gradientA;
		private Color gradientB;

		public override void OnInitialize() {
			// Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
			// UIElement is invisible and has no padding.
            area = new UIElement();
            // Position the bar above the player's head.
            // We'll update its position dynamically in Update() to follow the player.
            area.Width.Set(105, 0f);
            area.Height.Set(22, 0f);

			barFrame = new UIImage(ModContent.Request<Texture2D>("DestroyerTest/Assets/Textures/MeleeImmunityChargeFrame")); // Frame of our resource bar
			barFrame.Left.Set(22, 0f);
			barFrame.Top.Set(0, 0f);
			barFrame.Width.Set(72, 0f);
			barFrame.Height.Set(22, 0f);

			text = new UIText("0/0", 0.8f); // text to show stat
			text.Width.Set(72, 0f);
			text.Height.Set(22, 0f);
			text.Top.Set(40, 0f);
			text.Left.Set(0, 0f);

			gradientA = Color.White;
			gradientB = Color.Bisque; 
            
			area.Append(text);
			area.Append(barFrame);
			Append(area);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			
            if (!IsHoldingMelee())
				return;

			base.Draw(spriteBatch);
		}

		// Here we draw our UI
		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);

			var modPlayer = Main.LocalPlayer.GetModPlayer<MeleeImmunityPlayer>();
			// Calculate quotient
			float quotient = (float)modPlayer.Timer / modPlayer.ImmunityThreshold2; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
			quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

			// Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
			Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
			hitbox.X += 8;
			hitbox.Width -= 16;
			hitbox.Y += 4;
			hitbox.Height -= 10;

			// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);
			for (int i = 0; i < steps; i += 1) {
				// float percent = (float)i / steps; // Alternate Gradient Approach
				float percent = (float)i / (right - left);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
			}
		}

        public override void Update(GameTime gameTime) {
            // This prevents updating unless we are using one of the specified items
            if (!IsHoldingMelee())
                return;

            var modPlayer = Main.LocalPlayer.GetModPlayer<MeleeImmunityPlayer>();

            // Update the text to show the resource values
            float percentage = (float)modPlayer.Timer / modPlayer.ImmunityThreshold2 * 100;
            text.SetText(Language.GetTextValue("Mods.DestroyerTest.UI.MeleeImmunity", percentage.ToString("0.##"), modPlayer.Timer, modPlayer.ImmunityThreshold2));

            // Position the bar above the player's head
            Player player = Main.LocalPlayer;
            Vector2 playerScreenPos = player.Center - Main.screenPosition;
            float barWidth = area.Width.Pixels;
            float barHeight = area.Height.Pixels;

            // Offset to place the bar above the player's head
            float yOffset = player.gfxOffY - 60f; // adjust as needed for best appearance

            area.Left.Set((playerScreenPos.X - barWidth / 2f) + 60f, 0f);
            area.Top.Set(playerScreenPos.Y + yOffset - barHeight, 0f);

            area.Recalculate();

            base.Update(gameTime);
        }

		private bool IsHoldingMelee() {
            return Main.LocalPlayer.HeldItem.type == ModContent.ItemType<ConstantineScythe>();
		}
	}

	// This class will only be autoloaded/registered if we're not loading on a server
	[Autoload(Side = ModSide.Client)]
	internal class ImmunityBarUISystem : ModSystem
	{
		private UserInterface ResourceBarUserInterface;

		internal MeleeImmunityChargeBar ResourceBar;

		public static LocalizedText MeleeImmunityText { get; private set; }

		public override void Load() {
			ResourceBar = new();
			ResourceBarUserInterface = new();
			ResourceBarUserInterface.SetState(ResourceBar);

			string category = "UI";
			MeleeImmunityText ??= Mod.GetLocalization($"{category}.MeleeImmunity");
		}

		public override void UpdateUI(GameTime gameTime) {
			ResourceBarUserInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1) {
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"DestroyerTest: Melee Immunity Charge Bar",
					delegate {
						ResourceBarUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}