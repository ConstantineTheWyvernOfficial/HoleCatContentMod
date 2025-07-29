using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

public class ScreenshakePlayer : ModPlayer
{
    public int screenshakeTimer;
    public int screenshakeMagnitude;
    public override void ModifyScreenPosition()
    {
        screenshakeTimer--;
        if (screenshakeTimer > 0 )
        {
            Main.screenPosition += new Vector2(Main.rand.Next(screenshakeMagnitude * -1, screenshakeMagnitude + 1), Main.rand.Next(screenshakeMagnitude * -1, screenshakeMagnitude + 1));
        }
    }
}