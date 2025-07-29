using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ModLoader;

namespace DestroyerTest.Content.BossBar
{
	// Showcases very basic code for a custom boss bar style that is selectable in the menu in "Interface"
	// If you want custom NPC selection code for which boss bars to display, return true for PreventUpdate, and implement your own code in the Update hook
	public class RiftBarStyle : ModBossBarStyle
	{
		public override bool PreventDraw => true; // Prevents the default drawing code

		public override void Draw(SpriteBatch spriteBatch, IBigProgressBar currentBar, BigProgressBarInfo info) {
			if (currentBar == null) {
				return;
				// Only draw if vanilla decided to draw one (we let it update because we didn't override PreventUpdate to return true)
			}

			if (currentBar is not CommonBossBigProgressBar) {
				// If this is a regular bar without any special features, we draw our own thing. 
				NPC npc = Main.npc[info.npcIndexToAimAt];
				float lifePercent = Utils.Clamp(npc.life / (float)npc.lifeMax, 0f, 1f);

				// Pre-calculate bar dimensions once
				Rectangle barDimensions = Utils.CenteredRectangle(
					Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f),
					new Vector2(400f, 20f)
				);

				// Draw background
				Texture2D texture = ModContent.Request<Texture2D>("DestroyerTest/Content/BossBar/RiftBossBar").Value;
				spriteBatch.Draw(texture, barDimensions, default);

				// Draw foreground scaled to life
				Rectangle foreground = new Rectangle(barDimensions.X, barDimensions.Y, (int)(barDimensions.Width * lifePercent), barDimensions.Height);
				spriteBatch.Draw(texture, foreground, default);

				// Optionally draw the text on top
				if (info.showText && BigProgressBarSystem.ShowText) {
					BigProgressBarHelper.DrawHealthText(spriteBatch, barDimensions, 2 * Vector2.UnitY, npc.life, npc.lifeMax);
				}
			}

			else {
				// If a bar with special behavior is currently selected, draw it instead because we don't have access to its special features

				currentBar.Draw(ref info, spriteBatch);
			}
		}
	}
}