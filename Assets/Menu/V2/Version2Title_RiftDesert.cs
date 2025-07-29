using DetroyerTest.Content.RiftBiome;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Assets.Menu.V2
{
	public class Version2Title_RiftDesert : ModMenu
	{
		

		private Asset<Texture2D> sunTexture;
		private Asset<Texture2D> moonTexture;

		public override void Load() {
     
		}

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("DestroyerTest/Assets/Textures/Backgrounds/RiftLogo");

		public override Asset<Texture2D> SunTexture => sunTexture;

		public override Asset<Texture2D> MoonTexture => moonTexture;

		/*
		In ExampleMod we preload all "extra" textures, as recommended in https://github.com/tModLoader/tModLoader/wiki/Assets#asset-loading-timing.
		It is possible to load textures on demand instead, which might be useful in rare situations such as rarely used large textures. That would look like this:
		private Asset<Texture2D> moonTexture;
		public override Asset<Texture2D> MoonTexture => moonTexture ??= ModContent.Request<Texture2D>($"{menuAssetPath}/ExampliumMoon");
		*/

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/RiftV1");

		

		public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<RiftDesertBackgroundStyle>();

		public override string DisplayName => "Update 2.1.4: Rift (Desert)";

		public override void OnSelected() {
			SoundEngine.PlaySound(new SoundStyle("DestroyerTest/Assets/Audio/RiftSwordMinionTeleport")); // Plays a thunder sound when this ModMenu is selected
		}
	}
}