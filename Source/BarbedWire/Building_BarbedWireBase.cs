using System;
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
        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000009 RID: 9 RVA: 0x00002234 File Offset: 0x00000434
        public virtual bool Armed => true;

        // Token: 0x0600000A RID: 10 RVA: 0x00002247 File Offset: 0x00000447
        public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look(ref touchingPawns, "testees", LookMode.Reference, new object[0]);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000226C File Offset: 0x0000046C
		public override void Tick()
		{
			var armed = Armed;
			if (armed)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (var i = 0; i < thingList.Count; i++)
				{
					var pawn = thingList[i] as Pawn;
					var flag = pawn != null && !touchingPawns.Contains(pawn);
					if (flag)
					{
						touchingPawns.Add(pawn);
						CheckSpring(pawn);
					}
				}
			}
			for (var j = 0; j < touchingPawns.Count; j++)
			{
				Pawn pawn2 = touchingPawns[j];
				var flag2 = !pawn2.Spawned || pawn2.Position != base.Position;
				if (flag2)
				{
					touchingPawns.Remove(pawn2);
				}
			}
			base.Tick();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000235C File Offset: 0x0000055C
		protected virtual float SpringChance(Pawn p)
		{
			var flag = KnowsOfTrap(p);
			float num;
			if (flag)
			{
				num = KnowerSpringChance;
			}
			else
			{
				num = this.GetStatValue(StatDefOf.TrapSpringChance, true);
			}
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
			var flag = Rand.Value < SpringChance(p);
			if (flag)
			{
				Spring(p);
				var flag2 = p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer;
				if (flag2)
				{
					Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(new object[]
					{
						p.NameShortColored
					}), "LetterFriendlyTrapSprung".Translate(new object[]
					{
						p.NameShortColored
					}), LetterDefOf.NegativeEvent, new TargetInfo(base.Position, base.Map, false), null);
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002480 File Offset: 0x00000680
		public bool KnowsOfTrap(Pawn p)
		{
			var flag = p.Faction != null && !p.Faction.HostileTo(base.Faction);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				var flag2 = p.Faction == null && p.RaceProps.Animal && !p.InAggroMentalState;
				if (flag2)
				{
					result = true;
				}
				else
				{
					var flag3 = p.guest != null && p.guest.Released;
					if (flag3)
					{
						result = true;
					}
					else
					{
						Lord lord = p.GetLord();
						result = p.RaceProps.Humanlike && lord != null && lord.LordJob is LordJob_FormAndSendCaravan;
					}
				}
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002530 File Offset: 0x00000730
		public override ushort PathFindCostFor(Pawn p)
		{
			var flag = !Armed;
			ushort result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				var flag2 = KnowsOfTrap(p);
				if (flag2)
				{
					result = KnowerPathFindCost;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000256C File Offset: 0x0000076C
		public override ushort PathWalkCostFor(Pawn p)
		{
			var flag = !Armed;
			ushort result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				var flag2 = KnowsOfTrap(p);
				if (flag2)
				{
					result = KnowerPathWalkCost;
				}
				else
				{
					result = 0;
				}
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
			var flag = !text.NullOrEmpty();
			if (flag)
			{
				text += "\n";
			}
			var armed = Armed;
			if (armed)
			{
				text += Translator.Translate("TrapArmed");
			}
			else
			{
				text += Translator.Translate("TrapNotArmed");
			}
			return text;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002630 File Offset: 0x00000830
		public void Spring(Pawn p)
		{
			SoundDef.Named("DeadfallSpring").PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			SpringSub(p);
		}

		// Token: 0x06000014 RID: 20
		protected abstract void SpringSub(Pawn p);

		// Token: 0x04000006 RID: 6
		private List<Pawn> touchingPawns = new List<Pawn>();

		// Token: 0x04000007 RID: 7
		private const float KnowerSpringChance = 0.004f;

		// Token: 0x04000008 RID: 8
		private const ushort KnowerPathFindCost = 800;

		// Token: 0x04000009 RID: 9
		private const ushort KnowerPathWalkCost = 30;

		// Token: 0x0400000A RID: 10
		private const float AnimalSpringChanceFactor = 0.1f;
	}
}
