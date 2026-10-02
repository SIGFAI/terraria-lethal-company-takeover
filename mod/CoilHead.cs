using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Moves only while nobody is looking at it. Look at it and it freezes.</summary>
public class CoilHead : ModNPC
{
    bool frozen;

    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 2;

    public override void SetDefaults()
    {
        NPC.width = 40; NPC.height = 84;
        NPC.lifeMax = 110; NPC.damage = 26; NPC.defense = 6; NPC.knockBackResist = 0.3f; NPC.value = 120;
        NPC.aiStyle = NPCAIStyleID.Fighter; AIType = NPCID.Zombie;
        NPC.scale = 1.4f; NPC.width = (int)(NPC.width * 1.4f); NPC.height = (int)(NPC.height * 1.4f); NPC.lifeMax = (int)(NPC.lifeMax * 1.6f);
        NPC.HitSound = Mix.Style("boing"); NPC.DeathSound = Mix.Style("clang");
    }

    static bool Watched(NPC npc)
    {
        var p = Main.player[npc.target];
        if (!p.active) return false;
        float dx = npc.Center.X - p.Center.X;
        return System.Math.Abs(dx) < 900 && System.Math.Sign(dx) == p.direction;
    }

    public override bool PreAI()
    {
        NPC.timeLeft = 600;
        NPC.TargetClosest(false);
        bool was = frozen;
        frozen = Watched(NPC);
        if (!frozen && was) Mix.Sound("boing", NPC.Center, 0.7f);
        if (frozen)
        {
            NPC.velocity.X = 0;
            NPC.velocity.Y = System.Math.Min(NPC.velocity.Y + 0.3f, 10f);
            if (NPC.direction == 0) NPC.direction = 1;
            Lighting.AddLight(NPC.Center, 0.2f, 0.2f, 0.4f);
            return false;
        }
        return true;
    }

    public override void PostAI()
    {
        if (NPC.velocity.Y == 0f && System.Math.Abs(NPC.velocity.X) < 4.5f) NPC.velocity.X *= 1.12f;   // rushes when unseen
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.spriteDirection = NPC.direction;
        if (frozen) { NPC.frame.Y = 0; return; }
        NPC.frameCounter += 1;
        NPC.frame.Y = (int)(NPC.frameCounter / 6 % 2) * frameHeight;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        for (int i = 0; i < (NPC.life <= 0 ? 20 : 5); i++)
            Dust.NewDust(NPC.position, NPC.width, NPC.height, i % 2 == 0 ? DustID.SilverCoin : DustID.Smoke, hit.HitDirection * 2f, -2f);
    }

    public override void OnKill() => Company.Earn(NPC.Center, 70);
}
