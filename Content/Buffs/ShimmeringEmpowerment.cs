using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Equips;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Terraria.DataStructures;
using DestroyerTest.Common;
using InnoVault.PRT;

namespace DestroyerTest.Content.Buffs
{
    public class ShimmeringEmpowerment : ModBuff
    {
        // Some constants we define to make our life easier.
		public static readonly int FrameCount = 9; // Amount of frames we have on our animation spritesheet.
		public static readonly int AnimationSpeed = 4; // In ticks.
		public static readonly string AnimationSheetPath = "DestroyerTest/Content/Buffs/ShimmeringEmpowermentAnim";

		public static readonly int DamageBonus = 10;

		private Asset<Texture2D> animatedTexture;

		public override LocalizedText Description => Language.GetText("Mods.DestroyerTest.Buffs.ShimmeringEmpowerment.Description");

        public override string ToString()
        {
            return string.Format(Description.Value, CrazyText.scrambledString);
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            tip = string.Format(Language.GetTextValue("Mods.DestroyerTest.Buffs.ShimmeringEmpowerment.Description"), CrazyText.scrambledString);
        }



        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            animatedTexture = ModContent.Request<Texture2D>(AnimationSheetPath);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams) {
			// You can use this hook to make something special happen when the buff icon is drawn (such as reposition it, pick a different texture, etc.).

			// We draw our special texture here with a specific animation.

			// Use our animation spritesheet.
			Texture2D ourTexture = animatedTexture.Value;
			// Choose the frame to display, here based on constants and the game's tick count.
			Rectangle ourSourceRectangle = ourTexture.Frame(verticalFrames: FrameCount, frameY: (int)Main.GameUpdateCount / AnimationSpeed % FrameCount);

			// Other stuff you can do in this hook
			/*
			// Here we make the icon have a lime green tint.
			drawParams.drawColor = Color.LimeGreen * Main.buffAlpha[buffIndex];
			*/

			// Be aware of the fact that drawParams.mouseRectangle exists: it defaults to the size of the autoloaded buffs' sprite,
			// it handles mouseovering and clicking on the buff icon. Since our frame in the animation is 32x32 (same as the autoloaded sprite),
			// and we don't change drawParams.position, we don't have to do anything. If you offset the position, or have a non-standard size, change it accordingly.

			// We have two options here:
			// Option 1 is the recommended one, as it requires less code.
			// Option 2 allows you to customize drawing even more, but then you are on your own.

			// For demonstration, both options' codes are written down, but the latter is commented out using /* and */.

			// OPTION 1 - Let the game draw it for us. Therefore we have to assign our variables to drawParams:
			drawParams.Texture = ourTexture;
			drawParams.SourceRectangle = ourSourceRectangle;
			// Return true to let the game draw the buff icon.
			return true;

			/*
			// OPTION 2 - Draw our buff manually:
			spriteBatch.Draw(ourTexture, drawParams.position, ourSourceRectangle, drawParams.drawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			// Return false to prevent drawing the icon, since we have already drawn it.
			return false;
			*/
		}

        public int GlyphRelease = 0;
        public int GlyphReleaseSmall = 0;

        public static bool RingActive = false;

        public override void Update(Player player, ref int buffIndex)
        {


            // Check if the player is still wearing the full armor set
            bool hasFullArmorSet = player.armor[0].type == ModContent.ItemType<TenebrousArchmageHat>() &&
                                player.armor[1].type == ModContent.ItemType<TenebrousArchmageCoat>() &&
                                player.armor[2].type == ModContent.ItemType<TenebrousArchmagePants>();

            if (hasFullArmorSet)
            {
                // If the projectile is already present, keep the buff going
                if (RingActive == false)
                {
                    PRTLoader.NewParticle(PRTLoader.GetParticleID<RuneCircle2>(), player.Center, new Vector2(0.01f, 0.01f), ColorLib.TenebrisGradient, 1);
                    RingActive = true;
                }
            }
            else
            {
                // If the armor set is not equipped, let the buff expire naturally
                player.DelBuff(buffIndex); // Remove the buff immediately
            }

            GlyphRelease++;

            if (player.HasBuff(ModContent.BuffType<ShimmeringEmpowerment>()) && GlyphRelease >= 120)
            {
                CombatText.NewText(player.Hitbox, Color.SkyBlue, CrazyText.scrambledString_Small, true);
                GlyphRelease = 0;
            }
            if (player.HasBuff(ModContent.BuffType<ShimmeringEmpowerment>()) && GlyphReleaseSmall >= 60)
            {
                CombatText.NewText(player.Hitbox, Color.Navy, CrazyText.scrambledString_Single, true);
                GlyphReleaseSmall = 0;
            }

        }

    }
}