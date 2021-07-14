using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
    // Token: 0x02000002 RID: 2
    public class Building_BarbedWire : Building_BarbedWireBase
    {
        // Token: 0x04000004 RID: 4
        private static readonly FloatRange TrapDamageFactor = new FloatRange(0.7f, 1.3f);

        // Token: 0x04000005 RID: 5
        private static readonly IntRange DamageCount = new IntRange(1, 2);

        // Token: 0x04000002 RID: 2
        private bool armedInt = true;

        // Token: 0x04000001 RID: 1
        private bool autoRearm;

        // Token: 0x04000003 RID: 3
        private Graphic graphicUnarmedInt;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public override bool Armed => armedInt;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
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

        // Token: 0x06000003 RID: 3 RVA: 0x000020BF File Offset: 0x000002BF
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref armedInt, "armed");
            Scribe_Values.Look(ref autoRearm, "autoRearm");
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000020F0 File Offset: 0x000002F0
        protected override void SpringSub(Pawn p)
        {
            armedInt = true;
            if (p != null)
            {
                DamagePawn(p);
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002117 File Offset: 0x00000317
        public void Rearm()
        {
            armedInt = true;
            SoundDef.Named("TrapArm").PlayOneShot(new TargetInfo(Position, Map));
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002148 File Offset: 0x00000348
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
}