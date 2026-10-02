using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class HoardingBug : ModNPC
{
    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 2;

    public override void SetDefaults()
    {
        NPC.width = 68; NPC.height = 44;
        NPC.lifeMax = 45; NPC.damage = 14; NPC.defense = 2; NPC.knockBackResist = 0.7f; NPC.value = 60;
        NPC.aiStyle = NPCAIStyleID.Fighter; AIType = NPCID.Zombie;
        NPC.scale = 1.4f; NPC.width = (int)(NPC.width * 1.4f); NPC.height = (int)(NPC.height * 1.4f); NPC.lifeMax = (int)(NPC.lifeMax * 1.6f);
        NPC.HitSound = Mix.Style("chirp"); NPC.DeathSound = Mix.Style("squeak");
    }

    public override void PostAI()
    {
        NPC.timeLeft = 600;
        // hoarding bugs scuttle fast
        if (NPC.velocity.Y == 0f && System.Math.Abs(NPC.velocity.X) < 3.2f)
            NPC.velocity.X *= 1.06f;
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter += System.Math.Abs(NPC.velocity.X) * 0.6f + 0.2f;
        NPC.frame.Y = (int)(NPC.frameCounter / 4 % 2) * frameHeight;
        NPC.spriteDirection = NPC.direction;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        int n = NPC.life <= 0 ? 18 : 6;
        for (int i = 0; i < n; i++)
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Honey, hit.HitDirection * 2f, -1.5f);
        if (NPC.life <= 0)
            for (int i = 0; i < 8; i++) Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, 0, -1f);
    }

    public override void OnKill()
    {
        Company.Earn(NPC.Center, 30);
        if (Main.rand.NextBool(2)) Mix.Drop<RubberDuck>(NPC.Center);
    }
}
