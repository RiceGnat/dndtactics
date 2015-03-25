using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGEngine;

namespace DnDEngine.Combat.Magic
{
    [Serializable]
    public class CastDetails : ILoggable
    {
        public string Caster { get; private set; }
        public string[] Targets { get; private set; }

        public uint SpellID { get; private set; }
        public Spell.CastType CastMethod { get; private set; }
        public Spell.SaveType SpellSaveType { get; private set; }
        public ICollection<Spell.EnhancementType> AppliedEnhancements { get; private set; }

        public short[] DefenderAC { get; set; }

        public bool[] Hit { get; set; }
        public ushort[] HitRoll { get; set; }
        public short HitMod { get; set; }

        public bool[] Crit { get; set; }
        public ushort[] CritRoll { get; set; }
        public bool[] CritConfirm { get; set; }

        public short SaveDC { get; set; }
        public bool[] Save { get; set; }
        public ushort[] SaveRoll { get; set; }
        public short[] SaveMod { get; set; }

        public DamageDetails[] DamageDealt { get; set; }
        public int[] DamageTaken { get; set; }

        public DamageDetails[] Heals { get; set; }

        public short[] RemainingHP { get; set; }
        public int[] HealingDone { get; set; }

        public CastDetails(uint spellId, IUnit caster, IEnumerable<IUnit> targets)
        {
            Caster = caster.Name;
            SpellID = spellId;
            Targets = targets.Select(unit => unit.Name).ToArray();
            CastMethod = Spell.Compendium.GetSpell(SpellID).CastMethod;
            SpellSaveType = Spell.Compendium.GetSpell(SpellID).Save;

            DefenderAC = new short[Targets.Length];
            Hit = new bool[Targets.Length];
            HitRoll = new ushort[Targets.Length];
            Crit = new bool[Targets.Length];
            CritRoll = new ushort[Targets.Length];
            CritConfirm = new bool[Targets.Length];
            SaveRoll = new ushort[Targets.Length];
            SaveMod = new short[Targets.Length];
            Save = new bool[Targets.Length];
            DamageDealt = new DamageDetails[Targets.Length];
            DamageTaken = new int[Targets.Length];
            Heals = new DamageDetails[Targets.Length];
            HealingDone = new int[Targets.Length];
            RemainingHP = new short[Targets.Length];
        }

        public string Summary
        {
            get
            {
                StringBuilder head = new StringBuilder();
                StringBuilder body = new StringBuilder();
                head.AppendFormat("{0} casts {1}", Caster, Spell.Compendium.GetSpell(SpellID).Name);
                for (int i = 0; i < Targets.Length; i++)
                {
                    if (DamageDealt[i] != null)
                    {
                        body.AppendLine();
                        body.AppendFormat("{0} takes {1} damage", Targets[i], DamageTaken[i]);
                    }
                    if (Heals[i] != null)
                    {
                        body.AppendLine();
                        body.AppendFormat("{0} is healed for {1} HP", Targets[i], Heals[i].Total);
                    }
                }
                return head.ToString() + body.ToString();
            }
        }

        public string Full
        {
            get
            {
                StringBuilder head = new StringBuilder();
                StringBuilder body = new StringBuilder();

                head.AppendFormat("{0} casts {1}", Caster, Spell.Compendium.GetSpell(SpellID).Name);
                if (CastMethod == Spell.CastType.Target) head.Append(" on ");
                else if (CastMethod == Spell.CastType.Ray) head.Append(" at ");
                else if (CastMethod == Spell.CastType.Touch) head.Append(" and attempts to touch ");
                else
                {
                    if (DamageDealt[0] != null)
                    {
                        body.AppendLine();
                        body.AppendFormat("{0} rolls {1}, dealing {2} after bonuses", Caster, String.Join("+", DamageDealt[0].Breakdowns.Values.ToArray()), DamageDealt[0].Full);
                    }

                    if (Heals[0] != null)
                    {
                        body.AppendLine();
                        body.AppendFormat("{0} rolls {1}, healing for {2} HP after bonuses", Caster, String.Join("+", Heals[0].Breakdowns.Values.ToArray()), Heals[0].Total);
                    }
                }

                for (int i = 0; i < Targets.Length; i++)
                {
                    if (CastMethod == Spell.CastType.Target || CastMethod == Spell.CastType.Ray || CastMethod == Spell.CastType.Touch)
                    {
                        head.AppendFormat("{0}{1}", Targets[i], i == Targets.Length - 2 ? " and " : i < Targets.Length - 1 ? ", " : "");

                        if (CastMethod == Spell.CastType.Ray || CastMethod == Spell.CastType.Touch)
                        {
                            body.AppendLine();
                            body.AppendFormat("{0} rolls {1} (<{2}>{3}), {4} {5}'s touch AC of {6}", Caster, HitRoll[i] + HitMod, HitRoll[i], HitMod.ToString(Constants.ModifierFormat), Hit[i] ? "hitting" : "missing", Targets[i], DefenderAC[i]);
                            if (Crit[i])
                            {
                                body.AppendLine();
                                body.AppendFormat("{0} rolls <{1}> and {2}", Caster, CritRoll[i], CritConfirm[i] ? "confirms crit!" : "fails to confirm crit");
                            }
                        }

                        if (DamageDealt[i] != null)
                        {
                            body.AppendLine();
							body.AppendFormat("{0} rolls {1}, dealing {2} to {3} after bonuses", Caster, String.Join("+", DamageDealt[0].Breakdowns.Values.ToArray()), DamageDealt[0].Full, Targets[i]);
                        }

                        if (Heals[i] != null)
                        {
                            body.AppendLine();
							body.AppendFormat("{0} rolls {1}, healing {3} for {2} HP after bonuses", Caster, String.Join("+", Heals[0].Breakdowns.Values.ToArray()), Heals[0].Total, Targets[i]);
                        }
                    }

                    if (DamageDealt[i] != null)
                    {
                        body.AppendLine();
                        body.Append(Targets[i]);
                        if (SpellSaveType != Spell.SaveType.None)
                            body.AppendFormat(" rolls {0} (<{1}>{2}), {3} the save and taking ", SaveRoll[i] + SaveMod[i], SaveRoll[i], SaveMod[i].ToString(Constants.ModifierFormat), Save[i] ? "succeeding" : "failing");
                        else
                            body.Append(" takes ");
                        body.AppendFormat("{0} damage after reductions", DamageTaken[i]);
                    }

                    if (Heals[i] != null)
                    {
                        body.AppendLine();
                        body.AppendFormat("{0} is healed for {1} HP", Targets[i], Heals[i].Total);
                    }
                }

                if (SpellSaveType != Spell.SaveType.None)
                    head.AppendFormat(" with a DC {0} {1} save", SaveDC, SpellSaveType);

                return head.ToString() + body.ToString();
            }
        }

        public override string ToString()
        {
            return Summary;
        }
    }
}
