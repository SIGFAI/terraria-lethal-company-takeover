using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Close-up camera so the monsters loom large, plus drifting facility fog.</summary>
public class FacilityZoom : ModSystem
{
    public override void PostUpdateEverything()
    {
        if (Main.dedServ) return;
        Main.GameZoomTarget = 1.7f;
        if (Main.GameUpdateCount % 3 == 0 && Mix.Host != null)
        {
            var p = Mix.Host.Center + new Vector2(Main.rand.Next(-700, 700), Main.rand.Next(-300, 200));
            var d = Dust.NewDustPerfect(p, DustID.Smoke, new Vector2(Main.rand.NextFloat(-1.2f, 1.2f), Main.rand.NextFloat(-0.3f, 0.3f)), 200, new Color(120, 150, 130), 2.4f);
            d.noGravity = true; d.fadeIn = 1.1f;
        }
    }
}
