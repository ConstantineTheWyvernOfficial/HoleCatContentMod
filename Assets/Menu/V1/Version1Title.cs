using DetroyerTest.Content.RiftBiome;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Assets.Menu.V1
{
	public class Version1Title : ModMenu
	{
		

		private Asset<Texture2D> sunTexture;
		private Asset<Texture2D> moonTexture;

		public override void Load() {
      BackgroundTextureLoader.AddBackgroundTexture(Mod, "DestroyerTest/Assets/Textures/Backgrounds/ConstitutionProgressionFar");
			sunTexture = ModContent.Request<Texture2D>($"DestroyerTest/Assets/Textures/Backgrounds/DTV1Sun");
			moonTexture = ModContent.Request<Texture2D>($"DestroyerTest/Assets/Textures/Backgrounds/DTV1Sun");
		}

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("DestroyerTest/Assets/Textures/Backgrounds/V1Logo");

		public override Asset<Texture2D> SunTexture => sunTexture;

		public override Asset<Texture2D> MoonTexture => moonTexture;

		/*
		In ExampleMod we preload all "extra" textures, as recommended in https://github.com/tModLoader/tModLoader/wiki/Assets#asset-loading-timing.
		It is possible to load textures on demand instead, which might be useful in rare situations such as rarely used large textures. That would look like this:
		private Asset<Texture2D> moonTexture;
		public override Asset<Texture2D> MoonTexture => moonTexture ??= ModContent.Request<Texture2D>($"{menuAssetPath}/ExampliumMoon");
		*/

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/V1_Title");

		

		public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<V1BGS>();

		public override string DisplayName => "Update 1.0: Constitution Lineage";

		public override void OnSelected() {
			SoundEngine.PlaySound(SoundID.Thunder); // Plays a thunder sound when this ModMenu is selected
		}

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor) {
			drawColor = Main.DiscoColor; // Changes the draw color of the logo
			return true;
		}
	}
}