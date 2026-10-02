using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Creeps when watched, lunges when your back is turned.</summary>
public class Bracken : ModNPC
{
    bool watched, wasWatched;

    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 2;

    public override void SetDefaults()
    {
        NPC.width = 50; NPC.height = 100;
        NPC.lifeMax = 160; NPC.damage = 32; NPC.defense = 8; NPC.knockBackResist = 0.2f; NPC.value = 200;
        NPC.aiStyle = NPCAIStyleID.Fighter; AIType = NPCID.Zombie;
        NPC.scale = 1.4f; NPC.width = (int)(NPC.width * 1.4f); NPC.height = (int)(NPC.height * 1.4f); NPC.lifeMax = (int)(NPC.lifeMax * 1.6f);
        NPC.HitSound = Mix.Style("growl", 0.8f); NPC.DeathSound = Mix.Style("growl", 1f, -0.4f);
    }

    public override void PostAI()
    {
        NPC.timeLeft = 600;
        var p = Main.player[NPC.target];
        float dx = NPC.Center.X - p.Center.X;
        watched = System.Math.Abs(dx) < 900 && System.Math.Sign(dx) == p.direction;
        if (watched) NPC.velocity.X *= 0.4f;
        else if (NPC.velocity.Y == 0f && System.Math.Abs(NPC.velocity.X) < 3.4f) NPC.velocity.X *= 1.08f;
        if (!watched && wasWatched && Main.rand.NextBool(2)) Mix.Sound("growl", NPC.Center, 0.6f);
        // sneak attack: leaps at a player who is not looking
        if (!watched && NPC.velocity.Y == 0f && System.Math.Abs(dx) < 260 && Main.rand.NextBool(40))
        {
            NPC.velocity = new Vector2(-System.Math.Sign(dx) * 7f, -7f);
            Mix.Sound("growl", NPC.Center, 0.9f);
            Mix.Shake(6, 0.3);
        }
        wasWatched = watched;
        if (!watched) Lighting.AddLight(NPC.Top + new Vector2(0, 8), 0.5f, 0.45f, 0.05f);   // glowing eyes
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.spriteDirection = NPC.direction;
        NPC.frameCounter += System.Math.Abs(NPC.velocity.X) + 0.3f;
        NPC.frame.Y = (int)(NPC.frameCounter / 8 % 2) * frameHeight;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        for (int i = 0; i < (NPC.life <= 0 ? 24 : 7); i++)
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection * 2.5f, -2f);
    }

    public override void OnKill()
    {
        Company.Earn(NPC.Center, 120);
        Mix.Drop(ItemID.GoldCoin, NPC.Center, 2);
        if (Main.rand.NextBool(2)) Mix.Drop<RubberDuck>(NPC.Center);
    }
}
