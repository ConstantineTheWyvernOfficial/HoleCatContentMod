using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hjson;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria;
using Terraria.ModLoader;
using DestroyerTest.Content.MeleeWeapons.SwordLineage;
using DestroyerTest.Content.Dusts;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI;
using DestroyerTest.Content.Magic;
using DestroyerTest.Common;
using ReLogic.Content;
using DestroyerTest.Content.Equips;
using Terraria.DataStructures;

namespace DestroyerTest
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class DestroyerTestMod : Mod
	{
        
        public static DTConfig Config;
        public static ModKeybind StarBlastKeybind { get; private set; }
        public static ModKeybind HeroHelmetKeybind { get; private set; }
        public static ModKeybind RiftTeleportKeybind { get; private set; }

        public static ModKeybind OpenBookKeybind { get; private set; }

        public static ModKeybind ManaBurstKeybind { get; private set; }
        public static ModKeybind TenebrisTeleportKeybind { get; private set; }

        public static ModKeybind DeadlyBlossomKeybind { get; private set; }
        public static ModKeybind OilTentacleKeybind { get; private set; }

        public static int RouletteTokenCurrencyId;
        public override void Load()
        {
            Config = ModContent.GetInstance<DTConfig>();
            // Divider.
            StarBlastKeybind = KeybindLoader.RegisterKeybind(this, "Conclusion Star Blast", "P");
            // Divider.
            HeroHelmetKeybind = KeybindLoader.RegisterKeybind(this, "Hero Helmet Guard", "J");
            // Divider.
            RiftTeleportKeybind = KeybindLoader.RegisterKeybind(this, "Shadow Tome Teleport", "T");
            // Divider.
            OpenBookKeybind = KeybindLoader.RegisterKeybind(this, "Open Achievement Book", "M");
            // Divider.
            ManaBurstKeybind = KeybindLoader.RegisterKeybind(this, "Mana Burst", "C");
            // Divider.
            TenebrisTeleportKeybind = KeybindLoader.RegisterKeybind(this, "Tenebrous Clone Teleort", "L");
            // Divider.
            DeadlyBlossomKeybind = KeybindLoader.RegisterKeybind(this, "Deadly Blossom Spawn", "X");
            // Divider.
            OilTentacleKeybind = KeybindLoader.RegisterKeybind(this, "HoleCat Oil Tentacle", "OemTab");
            // Divider.
            RouletteTokenCurrencyId = CustomCurrencyManager.RegisterCurrency(new Content.Magic.RouletteToken(ModContent.ItemType<Content.Magic.RouletteTokenItem>(), 99L, "Mods.DestroyerTest.Content.Magic.RouletteToken"));
           
           
        }





        public override void Unload()
        {

            // Unregister the keybind
            StarBlastKeybind = null;
            HeroHelmetKeybind = null;
            RiftTeleportKeybind = null;
            OpenBookKeybind = null;
            ManaBurstKeybind = null;
            TenebrisTeleportKeybind = null;
            DeadlyBlossomKeybind = null;
            OilTentacleKeybind = null;
        
        }

        

        public class WorldEnterPlayer : ModPlayer
        {
            public bool firstJoin = true;

            public override void OnEnterWorld()
            {
                Main.NewText("CONSTANTINE'S ARSENAL /// [i:DestroyerTest/Crucible] Submit Ideas for new uses for the Alloys here! https://forms.gle/5SgJNiyrBehMkLxZ9", Color.Orange);
                firstJoin = true;
            }

            public override void UpdateDead()
            {
                // Ensures the flag doesn't reset on respawn
                firstJoin = false;
            }
        }

        
        
        
        

        


        
		public class DestroyerTestSystem : ModSystem
        {
        }

        internal class GlobalNPC_Folder
        {
            internal class DTGlobal
            {
            }
        }
       
    }

}
