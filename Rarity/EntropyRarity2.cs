using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace DestroyerTest.Rarity
{
    public class EntropyRarity2 : ModRarity
    {
        // Dark Blue is Rarity 14
        public override Color RarityColor => new Color(106, 40, 190);

        public override int GetPrefixedRarity(int offset, float valueMult) => offset switch
        {
            -2 => ItemRarityID.Blue,
            -1 => ItemRarityID.Purple,
            1 => ModContent.RarityType<EntropyRarity2>(),
            2 => ModContent.RarityType<EntropyRarity2>(),
            _ => Type,
        };
    }
}