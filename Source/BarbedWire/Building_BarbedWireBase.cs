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

    public virtual bool Armed => true;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref touchingPawns, "testees", LookMode.Reference);
    }

    public override void Tick()
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
                CheckSpring(pawn);
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
        var num = KnowsOfTrap(p) ? KnowerSpringChance : this.GetStatValue(StatDefOf.TrapSpringChance);

        num *= GenMath.LerpDouble(0.4f, 0.8f, 0f, 1f, p.BodySize);
        var animal = p.RaceProps.Animal;
        if (animal)
        {
            num *= AnimalSpringChanceFactor;
        }

        return Mathf.Clamp01(num);
    }

    private void CheckSpring(Pawn p)
    {
        if (!(Rand.Value < SpringChance(p)))
        {
            return;
        }

        Spring(p);
        if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
        {
            Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(p.NameShortColored),
                "LetterFriendlyTrapSprung".Translate(p.NameShortColored), LetterDefOf.NegativeEvent,
                new TargetInfo(Position, Map));
        }
    }

    public bool KnowsOfTrap(Pawn p)
    {
        bool result;
        if (p.Faction != null && !p.Faction.HostileTo(Faction))
        {
            result = true;
        }
        else
        {
            if (p.Faction == null && p.RaceProps.Animal && !p.InAggroMentalState)
            {
                result = true;
            }
            else
            {
                if (p.guest is { Released: true })
                {
                    result = true;
                }
                else
                {
                    var lord = p.GetLord();
                    result = p.RaceProps.Humanlike && lord is { LordJob: LordJob_FormAndSendCaravan };
                }
            }
        }

        return result;
    }

    public override ushort PathFindCostFor(Pawn p)
    {
        ushort result;
        if (!Armed)
        {
            result = 0;
        }
        else
        {
            result = KnowsOfTrap(p) ? KnowerPathFindCost : (ushort)0;
        }

        return result;
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
            result = KnowsOfTrap(p) ? KnowerPathWalkCost : (ushort)0;
        }

        return result;
    }

    public override bool IsDangerousFor(Pawn p)
    {
        return Armed && KnowsOfTrap(p);
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

    public void Spring(Pawn p)
    {
        SoundDef.Named("DeadfallSpring").PlayOneShot(new TargetInfo(Position, Map));
        SpringSub(p);
    }

    protected abstract void SpringSub(Pawn p);
}