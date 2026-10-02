using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class SigfMod : ModSystem
{
    static readonly string[] Lines =
    {
        "Employee, the quota will not meet itself.",
        "Remember: the Company is not liable for dismemberment.",
        "Please do not look at the Coil-Head. Or do.",
        "Hoarding Bugs are valuable company assets. Hit them.",
        "Your family has been notified of your bonus.",
    };
    static int line;

    static int Count() => Mix.NpcsNear(Mix.Host.Center, 40, n => n.ModNPC is HoardingBug or CoilHead or Bracken).Count;

    static void Wave()
    {
        if (Mix.Host == null || Count() >= 10) return;
        int r = Main.rand.Next(7);
        var pos = Mix.Ahead(Main.rand.Next(4, 8) * (Main.rand.NextBool() ? 1 : -1));
        if (r < 3) Mix.Spawn<HoardingBug>(pos);
        else if (r < 5) Mix.Spawn<CoilHead>(pos);
        else Mix.Spawn<Bracken>(pos);
    }

    public override void OnWorldLoad()
    {
        Mix.Every(1.5, Wave);
        Mix.Every(1, () => Mix.Shake(2.5f, 1.0));
        Mix.Every(14, () =>
        {
            Mix.Sound("horn", Mix.Host.Center, 0.6f);
            Mix.Title("MESSAGE FROM THE COMPANY", Lines[line++ % Lines.Length], 4);
        });
        Mix.After(1, () => Mix.Arm<Shovel>());
        Mix.After(1, () => { Mix.Sound("horn", Mix.Host.Center); Mix.Shake(8, 0.6); Mix.Title("LETHAL COMPANY", "Meet the quota. Do not look at the Coil-Head.", 4); });
        Mix.After(1.5, () => { Mix.Spawn<HoardingBug>(Mix.Ahead(4)); Mix.Spawn<HoardingBug>(Mix.Ahead(-5)); Mix.Spawn<CoilHead>(Mix.Ahead(7)); });
        Mix.After(5, () => Mix.Spawn<Bracken>(Mix.Ahead(-7)));

        Mix.Demo(30, () => { Mix.Arm<StopSign>(); Mix.Title("STOP SIGN", "Standard company issue.", 3); });
    }
}
