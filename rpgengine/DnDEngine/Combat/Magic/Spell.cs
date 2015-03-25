using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGEngine;

namespace DnDEngine.Combat.Magic
{
    public abstract partial class Spell : ICatalogable
    {
        public enum MagicType
        {
            Arcane,
            Divine
        }

        public enum SchoolType
        {
            None,
            Abjuration,
            Conjuration,
            Divination,
            Enchantment,
            Evocation,
            Illusion,
            Necromancy,
            Transmutation
        }

        public enum CastType
        {
            Self,
            Target,
            Touch,
            Ray,
            Line,
            Area
        }

        public enum SaveType
        {
            None,
            Reflex,
            Fortitude,
            Will
        }

        public enum EnergyType
        {
            None,
            Fire,
            Electricity,
            Sonic,
            Cold,
            Acid,
            Positive,
            Negative
        }

        public enum EnhancementType
        {
            None,
            Empowered,
            Extended,
            Quickened,
            Enlarged,
            Prolonged,
            Scorching,
            Shocking,
            Chilling,
            Corroding
        }

        public uint ID { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public MagicType Magic { get; protected set; }
        public SchoolType School { get; protected set; }
        public CastType CastMethod { get; protected set; }
        public SaveType Save { get; protected set; }

        public ushort MPCost { get; protected set; }
        public byte Range { get; protected set; }
        public byte Radius { get; protected set; }

        public Dice.Type DamageDie { get; protected set; }
        public Dice.Type HealDie { get; protected set; }
        public EnergyType Energy { get; protected set; }

        //public ICollection<EnhancementType> AvailableEnhancements { get; protected set; }

        /// <summary>
        /// Cast this spell on the specified target(s)
        /// <para>Override to add spell effects</para>
        /// </summary>
		/// <param name="caster">The casting unit</param>
		/// <param name="targets">Targets of the spell</param>
        public virtual ILoggable Cast(CombatEntity caster, IEnumerable<CombatEntity> targets)
        {
            CastDetails cast = new CastDetails(ID, caster, targets.Cast<IUnit>());
            CombatEntity[] targetArray = targets.ToArray();

            cast.HitMod = caster.Stats[DerivedStats.Type.HIT];
            cast.SaveDC = (short)(caster.Stats[CoreStats.Type.LVL] + 10);

            // Get number of damage/heal dice
            int numDice = (caster.Stats[Magic == MagicType.Arcane ? CoreStats.Type.INT : CoreStats.Type.WIS] - 10) / 2;


            // Loop through all targets
            for (int i = 0; i < targetArray.Length; i++)
            {
                CombatEntity target = targetArray[i];

                // Roll hit for rays and touch spells
                if (CastMethod == CastType.Ray || CastMethod == CastType.Touch)
                {
                    Dice d20 = new Dice(Dice.Type.D20);

                    cast.HitRoll[i] = d20.Roll();
                    cast.DefenderAC[i] = target.Stats[DerivedStats.Type.AC_Touch];

                    int roll = cast.HitRoll[i] + cast.HitMod;

                    // Natural 20 crit
                    if (cast.HitRoll[i] == 20)
                    {
                        cast.Hit[i] = true;
                        cast.Crit[i] = true;
                        cast.CritRoll[i] = d20.Roll();
                        cast.CritConfirm[i] = cast.CritRoll[i] == 20;
                    }
                    else if (roll >= cast.DefenderAC[i])
                    {
                        cast.Hit[i] = true;
                    }
                }
                else
                {
                    cast.Hit[i] = true;
                }

                if (cast.Hit[i])
                {
                    // Roll save if applicable
                    if (Save == SaveType.None)
                    {
                        cast.Save[i] = false;
                    }
                    else
                    {
                        Dice saveDie = new Dice(Dice.Type.D20);

                        cast.SaveRoll[i] = saveDie.Roll();
                        cast.SaveMod[i] = (short)(target.Stats[Save == SaveType.Fortitude ? CoreStats.Type.CON : Save == SaveType.Reflex ? CoreStats.Type.DEX : CoreStats.Type.WIS] - 10);

                        cast.Save[i] = cast.SaveRoll[i] == 20 || (cast.SaveRoll[i] + cast.SaveMod[i]) >= (cast.SaveDC);
                    }

                    // Only multi-target targeted and ray spells should roll damage for each target
                    // AoE spells (area, line) should use the same damage for all targets
                    if (CastMethod != CastType.Target && CastMethod != CastType.Ray && CastMethod != CastType.Touch && i > 0)
                    {
                        cast.DamageDealt[i] = cast.DamageDealt[0];
                        cast.Heals[i] = cast.Heals[0];
                    }
                    else
                    {
                        // Roll for damage
                        if (DamageDie != Dice.Type.None)
                        {
                            Dice dice = new Dice(Math.Max(1, numDice), DamageDie);
                            DamageDetails damage = new DamageDetails();

                            // Base spell damage
                            damage.AddRoll(dice.Roll(), dice.ToString(), Energy);

                            // Add bonus damage from enhancements
                            dice = new Dice(Math.Max(1, numDice), Dice.Type.D4);

                            //if ((cast.AppliedEnhancements & EnhancementType.Scorching) == EnhancementType.Scorching)
                            //{
                            //    cast.Damage.Add(EnergyType.Fire, dice.Roll(), dice.ToString());
                            //}

                            //if ((cast.AppliedEnhancements & EnhancementType.Shocking) == EnhancementType.Shocking)
                            //{
                            //    cast.Damage.Add(EnergyType.Electricity, dice.Roll(), dice.ToString());
                            //}

                            //if ((cast.AppliedEnhancements & EnhancementType.Chilling) == EnhancementType.Chilling)
                            //{
                            //    cast.Damage.Add(EnergyType.Cold, dice.Roll(), dice.ToString());
                            //}

                            //if ((cast.AppliedEnhancements & EnhancementType.Corroding) == EnhancementType.Corroding)
                            //{
                            //    cast.Damage.Add(EnergyType.Acid, dice.Roll(), dice.ToString());
                            //}

                            // Get damage values after bonuses
                            foreach (EnergyType energy in damage.Rolls.Keys)
                            {
                                damage.Damages[energy] = CombatManager.CalculateDamage(caster, damage.Rolls[energy], CoreStats.Type.INT) * (cast.CritConfirm[i] ? 2 : 1);
                            }

                            cast.DamageDealt[i] = damage;

                            // Apply effects from enhancements
                            //if (cast.TargetFailedSave[i] || cast.CritConfirm[i])
                            //{
                            //    if ((cast.AppliedEnhancements & EnhancementType.Scorching) == EnhancementType.Scorching)
                            //    {
                            //        target.AddBuff(new Burning((ushort)Math.Floor(caster.Stats.LVL / 3f + 1)));
                            //    }

                            //    if ((cast.AppliedEnhancements & EnhancementType.Shocking) == EnhancementType.Shocking)
                            //    {
                            //        target.AddBuff(new Dazed((ushort)Math.Floor(caster.Stats.LVL / 3f + 1)));
                            //    }

                            //    if ((cast.AppliedEnhancements & EnhancementType.Chilling) == EnhancementType.Chilling)
                            //    {
                            //        target.AddBuff(new Frostbite((ushort)Math.Floor(caster.Stats.LVL / 3f + 1)));
                            //    }

                            //    if ((cast.AppliedEnhancements & EnhancementType.Corroding) == EnhancementType.Corroding)
                            //    {
                            //        target.AddBuff(new Poisoned(10, (ushort)(caster.Stats.LVL + 10)));
                            //    }
                            //}

                        }

                        // Roll heals
                        if (HealDie != Dice.Type.None)
                        {
                            Dice dice = new Dice(Math.Max(1, numDice), HealDie);
                            cast.Heals[i] = new DamageDetails(dice.Roll(), dice.ToString(), CombatManager.CalculateDamage(caster, dice.Result, CoreStats.Type.WIS), Spell.EnergyType.Positive);
                            
                        }
                    }
                    
                    if (cast.DamageDealt[i] != null)
                    {
                        // Apply damage to target and get total damage taken after reductions
                        cast.DamageTaken[i] = CombatManager.ApplyDamage(target, cast.DamageDealt[i], cast.Save[i] ? 0.5f : 1f);
                        cast.RemainingHP[i] = target.CurrentHP;
                    }

                    if (cast.Heals[i] != null)
                    {
                        // Apply heals to target
                        short oldHP = target.CurrentHP;
                        cast.RemainingHP[i] = target.CurrentHP += (short)cast.Heals[i].Total;
                        cast.HealingDone[i] = target.CurrentHP - oldHP;
                    }
                }
            }


            return cast;
        }
    }
}
