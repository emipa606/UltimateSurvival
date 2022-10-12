using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class Building_BarbedWire : Building_BarbedWireBase
{
    private static readonly FloatRange TrapDamageFactor = new FloatRange(0.7f, 1.3f);

    private static readonly IntRange DamageCount = new IntRange(1, 2);

    private bool armedInt = true;

    private bool autoRearm;

    private Graphic graphicUnarmedInt;

    public override bool Armed => armedInt;

    public override Graphic Graphic
    {
        get
        {
            Graphic graphic;
            if (armedInt)
            {
                graphic = base.Graphic;
            }
            else
            {
                if (graphicUnarmedInt == null)
                {
                    graphicUnarmedInt = def.building.trapUnarmedGraphicData.GraphicColoredFor(this);
                }

                graphic = graphicUnarmedInt;
            }

            return graphic;
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref armedInt, "armed");
        Scribe_Values.Look(ref autoRearm, "autoRearm");
    }

    protected override void SpringSub(Pawn p)
    {
        armedInt = true;
        if (p != null)
        {
            DamagePawn(p);
        }
    }

    public void Rearm()
    {
        armedInt = true;
        SoundDef.Named("TrapArm").PlayOneShot(new TargetInfo(Position, Map));
    }

    private void DamagePawn(Pawn p)
    {
        var height = Rand.Value >= 0.666f ? BodyPartHeight.Middle : BodyPartHeight.Top;
        var num = Mathf.RoundToInt(this.GetStatValue(StatDefOf.TrapMeleeDamage) * TrapDamageFactor.RandomInRange);
        var randomInRange = DamageCount.RandomInRange;
        for (var i = 0; i < randomInRange; i++)
        {
            if (num <= 0)
            {
                break;
            }

            var num2 = Mathf.Max(1, Mathf.RoundToInt(Rand.Value * num));
            num -= num2;
            var damageInfo = new DamageInfo(DamageDefOf.Cut, num2, -1f, -1, this);
            damageInfo.SetBodyRegion(height, BodyPartDepth.Outside);
            p.TakeDamage(damageInfo);
        }
    }
}