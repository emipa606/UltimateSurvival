using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
    // Token: 0x02000003 RID: 3
    public abstract class Building_BarbedWireBase : Building
    {
        // Token: 0x04000007 RID: 7
        private const float KnowerSpringChance = 0.004f;

        // Token: 0x04000008 RID: 8
        private const ushort KnowerPathFindCost = 800;

        // Token: 0x04000009 RID: 9
        private const ushort KnowerPathWalkCost = 30;

        // Token: 0x0400000A RID: 10
        private const float AnimalSpringChanceFactor = 0.1f;

        // Token: 0x04000006 RID: 6
        private List<Pawn> touchingPawns = new List<Pawn>();

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000009 RID: 9 RVA: 0x00002234 File Offset: 0x00000434
        public virtual bool Armed => true;

        // Token: 0x0600000A RID: 10 RVA: 0x00002247 File Offset: 0x00000447
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref touchingPawns, "testees", LookMode.Reference);
        }

        // Token: 0x0600000B RID: 11 RVA: 0x0000226C File Offset: 0x0000046C
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

        // Token: 0x0600000C RID: 12 RVA: 0x0000235C File Offset: 0x0000055C
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

        // Token: 0x0600000D RID: 13 RVA: 0x000023D4 File Offset: 0x000005D4
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

        // Token: 0x0600000E RID: 14 RVA: 0x00002480 File Offset: 0x00000680
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
                    if (p.guest is {Released: true})
                    {
                        result = true;
                    }
                    else
                    {
                        var lord = p.GetLord();
                        result = p.RaceProps.Humanlike && lord is {LordJob: LordJob_FormAndSendCaravan};
                    }
                }
            }

            return result;
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002530 File Offset: 0x00000730
        public override ushort PathFindCostFor(Pawn p)
        {
            ushort result;
            if (!Armed)
            {
                result = 0;
            }
            else
            {
                result = KnowsOfTrap(p) ? KnowerPathFindCost : (ushort) 0;
            }

            return result;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x0000256C File Offset: 0x0000076C
        public override ushort PathWalkCostFor(Pawn p)
        {
            ushort result;
            if (!Armed)
            {
                result = 0;
            }
            else
            {
                result = KnowsOfTrap(p) ? KnowerPathWalkCost : (ushort) 0;
            }

            return result;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x000025A4 File Offset: 0x000007A4
        public override bool IsDangerousFor(Pawn p)
        {
            return Armed && KnowsOfTrap(p);
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000025C8 File Offset: 0x000007C8
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

        // Token: 0x06000013 RID: 19 RVA: 0x00002630 File Offset: 0x00000830
        public void Spring(Pawn p)
        {
            SoundDef.Named("DeadfallSpring").PlayOneShot(new TargetInfo(Position, Map));
            SpringSub(p);
        }

        // Token: 0x06000014 RID: 20
        protected abstract void SpringSub(Pawn p);
    }
}