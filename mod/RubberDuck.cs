using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Scrap. Worthless to anyone but the Company. Squeaks when you use it.</summary>
public class RubberDuck : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32; Item.height = 32; Item.maxStack = 99;
        Item.useStyle = ItemUseStyleID.HoldUp; Item.useTime = 15; Item.useAnimation = 15;
        Item.UseSound = Mix.Style("squeak"); Item.rare = ItemRarityID.Green; Item.value = Item.sellPrice(silver: 40);
    }
}
