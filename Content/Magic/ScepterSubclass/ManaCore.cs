using DestroyerTest.Common;
using DestroyerTest.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Content.Magic.ScepterSubclass
{
	public class ManaCore : ModItem
	{

		//private float previousDamageMultiplier = 1f;

		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 32;
			Item.value = Item.sellPrice(gold: 86);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player) {
			var modPlayer = player.GetModPlayer<OldManaPlayer>();
			player.GetDamage<ScepterClass>() += 0.02f;
			ScepterClassStats.Range += 10;
			// Only store mana once when the buff is first applied
            if (modPlayer.StoredMana == -1)
            {
                modPlayer.StoredMana = player.statMana;
            }
		}

		public void ActivateManaBurst(Player player) {
			if (!player.HasBuff(ModContent.BuffType<ManaBurst>())) {
				SoundStyle ManaBurstSound = new SoundStyle("DestroyerTest/Assets/Audio/ManaBurst");
				SoundEngine.PlaySound(ManaBurstSound);

				player.AddBuff(ModContent.BuffType<ManaBurst>(), 120);
				player.statMana = 0;
			}
		}
	}

	public class ManaCorePlayer : ModPlayer
	{
		public override void ProcessTriggers(TriggersSet triggersSet) {
			if (DestroyerTestMod.ManaBurstKeybind.JustPressed) {
				// Check if the player has ManaCore equipped
				for (int i = 3; i < 8 + Player.extraAccessorySlots; i++) { // Accessories start at index 3
					if (Player.armor[i].type == ModContent.ItemType<ManaCore>()) {
						ManaCore manaCore = (ManaCore)Player.armor[i].ModItem;
						manaCore.ActivateManaBurst(Player);
					}
				}
			}
		}

		public override void PostUpdate() {
			// Update the Mana Burst timer every frame
			for (int i = 3; i < 8 + Player.extraAccessorySlots; i++) {
				if (Player.armor[i].type == ModContent.ItemType<ManaCore>()) {
					ManaCore manaCore = (ManaCore)Player.armor[i].ModItem;
				}
			}
		}
	}

	public class OldManaPlayer : ModPlayer
        {
            public int StoredMana = -1; // -1 indicates uninitialized state

            public override void ResetEffects()
            {
                // Reset when the buff is gone
                if (!Player.HasBuff(ModContent.BuffType<ManaBurst>()))
                {
                    StoredMana = -1;
                }
            }
        }
}
