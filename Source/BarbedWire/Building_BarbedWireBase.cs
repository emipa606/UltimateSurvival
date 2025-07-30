using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld;

public abstract class Building_BarbedWireBase : Building
{
    private const float KnowerSpringChance = 0.004f;

    private const ushort KnowerPathFindCost = 800;

    private const ushort KnowerPathWalkCost = 30;

    private const float AnimalSpringChanceFactor = 0.1f;

    private List<Pawn> touchingPawns = [];

    protected virtual bool Armed => true;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref touchingPawns, "testees", LookMode.Reference);
    }

    protected override void Tick()
    {
        var armed = Armed;
        if (armed)
        {
            var thingList = Position.GetThingList(Map);
            foreach (var thing in thingList)
            {
                if (thing is not Pawn pawn || touchingPawns.Contains(pawn))
                {
                    continue;
                }

                touchingPawns.Add(pawn);
                checkSpring(pawn);
            }
        }

        for (var j = 0; j < touchingPawns.Count; j++)
        {
            var pawn2 = touchingPawns[j];
            if (!pawn2.Spawned || pawn2.Position != Position)
            {
                touchingPawns.Remove(pawn2);
            }
        }

        base.Tick();
    }

    protected virtual float SpringChance(Pawn p)
    {
        var num = knowsOfTrap(p) ? KnowerSpringChance : this.GetStatValue(StatDefOf.TrapSpringChance);

        num *= GenMath.LerpDouble(0.4f, 0.8f, 0f, 1f, p.BodySize);
        var animal = p.RaceProps.Animal;
        if (animal)
        {
            num *= AnimalSpringChanceFactor;
        }

        return Mathf.Clamp01(num);
    }

    private void checkSpring(Pawn p)
    {
        if (!(Rand.Value < SpringChance(p)))
        {
            return;
        }

        spring(p);
        if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
        {
            Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(p.NameShortColored),
                "LetterFriendlyTrapSprung".Translate(p.NameShortColored), LetterDefOf.NegativeEvent,
                new TargetInfo(Position, Map));
        }
    }

    private bool knowsOfTrap(Pawn p)
    {
        if (p.Faction != null && !p.Faction.HostileTo(Faction))
        {
            return true;
        }

        if (p.Faction == null && p.RaceProps.Animal && !p.InAggroMentalState)
        {
            return true;
        }

        if (p.guest is { Released: true })
        {
            return true;
        }

        var lord = p.GetLord();
        return p.RaceProps.Humanlike && lord is { LordJob: LordJob_FormAndSendCaravan };
    }

    public override ushort PathWalkCostFor(Pawn p)
    {
        ushort result;
        if (!Armed)
        {
            result = 0;
        }
        else
        {
            result = knowsOfTrap(p) ? KnowerPathWalkCost : (ushort)0;
        }

        return result;
    }

    public override bool IsDangerousFor(Pawn p)
    {
        return Armed && knowsOfTrap(p);
    }

    public override string GetInspectString()
    {
        var text = base.GetInspectString();
        if (!text.NullOrEmpty())
        {
            text += "\n";
        }

        var armed = Armed;
        if (armed)
        {
            text += "TrapArmed".Translate();
        }
        else
        {
            text += "TrapNotArmed".Translate();
        }

        return text;
    }

    private void spring(Pawn p)
    {
        SoundDef.Named("DeadfallSpring").PlayOneShot(new TargetInfo(Position, Map));
        SpringSub(p);
    }

    protected abstract void SpringSub(Pawn p);
}