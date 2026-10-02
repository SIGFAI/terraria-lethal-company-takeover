using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class StopSign : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32; Item.height = 32; Item.scale = 1.6f;
        Item.damage = 44; Item.DamageType = DamageClass.Melee; Item.knockBack = 10f;
        Item.useTime = 30; Item.useAnimation = 30; Item.useStyle = ItemUseStyleID.Swing; Item.autoReuse = true;
        Item.UseSound = Mix.Style("clang", 1f, -0.3f); Item.rare = ItemRarityID.Orange; Item.value = 10000;
    }

    public override void MeleeEffects(Player player, Microsoft.Xna.Framework.Rectangle hitbox)
    {
        var d = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Torch, 0f, 0f, 100, default, 1.4f);
        d.noGravity = true; d.velocity *= 0.4f;
        if (Main.rand.NextBool(3)) Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.SilverCoin);
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        Mix.Burst(target.Center, DustID.Torch, 10, 12f, 4f);
        Mix.Shake(4, 0.15);
    }
}
