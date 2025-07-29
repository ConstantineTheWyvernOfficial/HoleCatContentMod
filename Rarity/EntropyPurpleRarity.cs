using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace DestroyerTest.Rarity
{
    public class EntropyPurpleRarity : ModRarity
    {
        public override Color RarityColor => new Color(190, 0, 255);

        public override int GetPrefixedRarity(int offset, float valueMult) => Type;
    }
}