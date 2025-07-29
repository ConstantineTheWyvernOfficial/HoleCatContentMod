using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;

namespace DestroyerTest.Content.RiftBiome
{
public class RiftDarkSky : CustomSky
{
    private bool isActive;
    
    public override void Update(GameTime gameTime) {}

    public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
    {
        if (minDepth >= 0f && maxDepth < 10f)
        {
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);
        }
    }

    public override bool IsActive() => isActive;

    public override void Activate(Vector2 position, params object[] args) => isActive = true;

    public override void Deactivate(params object[] args) => isActive = false;

    public override void Reset() => isActive = false;
}
}