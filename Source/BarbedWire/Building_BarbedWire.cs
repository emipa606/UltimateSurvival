using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000002 RID: 2
	public class Building_BarbedWire : Building_BarbedWireBase
	{
        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public override bool Armed => armedInt;

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
        public override Graphic Graphic
		{
			get
			{
				var flag = armedInt;
				Graphic graphic;
				if (flag)
				{
					graphic = base.Graphic;
				}
				else
				{
					var flag2 = graphicUnarmedInt == null;
					if (flag2)
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
			Scribe_Values.Look(ref armedInt, "armed", false, false);
			Scribe_Values.Look(ref autoRearm, "autoRearm", false, false);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F0 File Offset: 0x000002F0
		protected override void SpringSub(Pawn p)
		{
			armedInt = true;
			var flag = p != null;
			if (flag)
			{
				DamagePawn(p);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002117 File Offset: 0x00000317
		public void Rearm()
		{
			armedInt = true;
			SoundDef.Named("TrapArm").PlayOneShot(new TargetInfo(base.Position, base.Map, false));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002148 File Offset: 0x00000348
		private void DamagePawn(Pawn p)
		{
			BodyPartHeight height = (Rand.Value >= 0.666f) ? BodyPartHeight.Middle : BodyPartHeight.Top;
			var num = Mathf.RoundToInt(this.GetStatValue(StatDefOf.TrapMeleeDamage, true) * Building_BarbedWire.TrapDamageFactor.RandomInRange);
			var randomInRange = Building_BarbedWire.DamageCount.RandomInRange;
			for (var i = 0; i < randomInRange; i++)
			{
				var flag = num <= 0;
				if (flag)
				{
					break;
				}
				var num2 = Mathf.Max(1, Mathf.RoundToInt(Rand.Value * (float)num));
				num -= num2;
				var damageInfo = new DamageInfo(DamageDefOf.Cut, num2, -1f, -1, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
				damageInfo.SetBodyRegion(height, BodyPartDepth.Outside);
				p.TakeDamage(damageInfo);
			}
		}

		// Token: 0x04000001 RID: 1
		private bool autoRearm;

		// Token: 0x04000002 RID: 2
		private bool armedInt = true;

		// Token: 0x04000003 RID: 3
		private Graphic graphicUnarmedInt;

		// Token: 0x04000004 RID: 4
		private static readonly FloatRange TrapDamageFactor = new FloatRange(0.7f, 1.3f);

		// Token: 0x04000005 RID: 5
		private static readonly IntRange DamageCount = new IntRange(1, 2);
	}
}
