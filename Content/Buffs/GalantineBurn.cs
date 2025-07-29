using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using DestroyerTest.Content.Particles;
using DestroyerTest.Content.Equips;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Terraria.DataStructures;
using DestroyerTest.Common;

namespace DestroyerTest.Content.Buffs
{
	public class GalantineBurn : ModBuff
	{
		// Some constants we define to make our life easier.
		public static readonly int FrameCount = 8; // Amount of frames we have on our animation spritesheet.
		public static readonly int AnimationSpeed = 4; // In ticks.
		public static readonly string AnimationSheetPath = "DestroyerTest/Content/Buffs/GalantineBurnAnim";

		public static readonly int DamageBonus = 10;

		private Asset<Texture2D> animatedTexture;

		public override LocalizedText Description => Language.GetText("Starlight Congregates around you!");



		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			animatedTexture = ModContent.Request<Texture2D>(AnimationSheetPath);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
		{
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



		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<GBPlayer>().lifeRegenDebuff = true;
			player.statManaMax2 -= 20;
			player.GetDamage(DamageClass.Generic) *= 0.9f;


		}

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.TryGetGlobalNPC<GBTarget>(out var modNPC)) {
                modNPC.lifeRegenDebuff = true;
            }
        }

	}

	public class GBPlayer : ModPlayer
	{

		// Flag checking when life regen debuff should be activated
		public bool lifeRegenDebuff;

		public override void ResetEffects()
		{
			lifeRegenDebuff = false;
		}

		// Allows you to give the player a negative life regeneration based on its state (for example, the "On Fire!" debuff makes the player take damage-over-time)
		// This is typically done by setting player.lifeRegen to 0 if it is positive, setting player.lifeRegenTime to 0, and subtracting a number from player.lifeRegen
		// The player will take damage at a rate of half the number you subtract per second
		public override void UpdateBadLifeRegen()
		{
			if (lifeRegenDebuff)
			{
				int amount = 13;
				amount++;
				Dust.NewDust(Player.position, Player.width, Player.height, DustID.TintableDustLighted, 0.0f, 0.5f, 0, ColorLib.StellarColor, 1);
				// These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects
				if (Player.lifeRegen > 0)
					Player.lifeRegen = 0;
				// Player.lifeRegenTime used to increase the speed at which the player reaches its maximum natural life regeneration
				// So we set it to 0, and while this debuff is active, it never reaches it
				Player.lifeRegenTime = 0;
				// lifeRegen is measured in 1/2 life per second. Therefore, this effect causes 8 life lost per second
				Player.lifeRegen -= amount;
			}
		}
	}
	
	public class GBTarget : GlobalNPC
    {
        public override bool InstancePerEntity => true; // Ensures each NPC has its own instance

        public bool lifeRegenDebuff;

        public override void ResetEffects(NPC npc) {
            lifeRegenDebuff = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            if (lifeRegenDebuff) {
                

                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                npc.lifeRegen -= 24;
            }
        }
    }
}