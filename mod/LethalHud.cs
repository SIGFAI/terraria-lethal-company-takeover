using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace Sigf.Content;

/// <summary>Facility darkness, flickering ship alarm lights, and the big Company terminal readout.</summary>
public class LethalHud : ModSystem
{
    int tick;

    public override void OnWorldLoad() => Company.Reset();

    public override void PostUpdateEverything() => tick++;

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        layers.Insert(0, new LegacyGameInterfaceLayer("Sigf: Vignette", () => { DrawVignette(tick); return true; }, InterfaceScaleType.UI));
        layers.Add(new LegacyGameInterfaceLayer("Sigf: Terminal", () => { DrawTerminal(); return true; }, InterfaceScaleType.UI));
    }

    static void DrawVignette(int tick)
    {
        var sb = Main.spriteBatch; var px = TextureAssets.MagicPixel.Value;
        int w = Main.screenWidth, h = Main.screenHeight;
        // sickly facility tint over the whole world
        float flick = 0.5f + 0.5f * (float)Math.Sin(tick * 0.11) * (float)Math.Sin(tick * 0.037);
        sb.Draw(px, new Rectangle(0, 0, w, h), new Color(10, 40, 30) * (0.30f + 0.06f * flick));
        // heavy dark edges
        const int steps = 16;
        for (int i = 0; i < steps; i++)
        {
            float k = 1f - i / (float)steps;
            var c = new Color(0, 4, 3) * (0.85f * k * k);
            int t = 28, o = i * t;
            sb.Draw(px, new Rectangle(0, o, w, t), c);
            sb.Draw(px, new Rectangle(0, h - o - t, w, t), c);
            sb.Draw(px, new Rectangle(o, 0, t, h), c);
            sb.Draw(px, new Rectangle(w - o - t, 0, t, h), c);
        }
        // ship alarm: red pulse every few seconds
        float al = (float)Math.Max(0, Math.Sin(tick * 0.06)) * (tick / 60 % 8 < 3 ? 1f : 0f);
        if (al > 0) sb.Draw(px, new Rectangle(0, 0, w, h), new Color(190, 0, 0) * (0.22f * al));
        // scanlines
        for (int y = (tick / 2) % 6; y < h; y += 6) sb.Draw(px, new Rectangle(0, y, w, 1), Color.Black * 0.10f);
    }

    static void DrawTerminal()
    {
        var sb = Main.spriteBatch; var px = TextureAssets.MagicPixel.Value;
        int bw = 520, bh = 150;
        var box = new Rectangle(Main.screenWidth - bw - 20, Main.screenHeight - bh - 20, bw, bh);
        sb.Draw(px, box, new Color(0, 20, 5) * 0.85f);
        sb.Draw(px, new Rectangle(box.X, box.Y, box.Width, 3), Color.Orange);
        sb.Draw(px, new Rectangle(box.X, box.Bottom - 3, box.Width, 3), Color.Orange);
        var amber = new Color(255, 150, 20);
        Utils.DrawBorderString(sb, "THE COMPANY  -  DAY " + Company.Day, new Vector2(box.X + 16, box.Y + 10), amber, 1.2f);
        Utils.DrawBorderString(sb, "QUOTA: $" + Company.Cash + " / $" + Company.Quota, new Vector2(box.X + 16, box.Y + 54), Color.White, 1.35f);
        float f = Utils.Clamp(Company.Cash / (float)Company.Quota, 0f, 1f);
        sb.Draw(px, new Rectangle(box.X + 16, box.Y + 110, bw - 32, 20), new Color(60, 30, 0));
        sb.Draw(px, new Rectangle(box.X + 16, box.Y + 110, (int)((bw - 32) * f), 20), amber);
    }
}
