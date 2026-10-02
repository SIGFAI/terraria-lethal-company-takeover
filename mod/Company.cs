using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Sigf.Content;

/// <summary>The Company profit quota: every monster killed turns into scrap value.</summary>
public static class Company
{
    public static int Cash;
    public static int Quota = 600;
    public static int Day = 1;

    public static void Earn(Vector2 pos, int value)
    {
        Cash += value;
        Mix.Popup(pos + new Vector2(0, -30), "+$" + value, Color.LimeGreen);
        if (Cash >= Quota)
        {
            Cash -= Quota;
            Quota += 300;
            Day++;
            Mix.Sound("ding", Mix.Host?.Center);
            Mix.Title("QUOTA MET!", "The Company is pleased. New quota: $" + Quota, 3);
            if (Mix.Host != null) Mix.Drop(ItemID.GoldCoin, Mix.Host.Center + new Vector2(0, -40), 3);
        }
    }

    public static void Reset() { Cash = 0; Quota = 600; Day = 1; }
}
