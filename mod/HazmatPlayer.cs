using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Every player wears the Company orange suit and a headlamp.</summary>
public class HazmatPlayer : ModPlayer
{
    static readonly Color Orange = new Color(255, 120, 10);

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        drawInfo.colorShirt = Orange; drawInfo.colorUnderShirt = new Color(255, 150, 30);
        drawInfo.colorPants = new Color(230, 95, 5); drawInfo.colorShoes = new Color(50, 40, 30);
    }

    public override void PostUpdate()
    {
        Lighting.AddLight(Player.Center + new Vector2(Player.direction * 60, -4), 1.0f, 0.95f, 0.8f);
        Lighting.AddLight(Player.Center + new Vector2(Player.direction * 140, -4), 0.8f, 0.75f, 0.6f);
    }
}
