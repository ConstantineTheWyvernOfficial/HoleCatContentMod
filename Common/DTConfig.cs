using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace DestroyerTest.Common
{
    public class DTConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [DefaultValue(true)]
        public bool MinionExtrasToggle { get; set; }
        [DefaultValue(false)]
        public bool UnnerfTenebrousKatana { get; set; }
    }
}
