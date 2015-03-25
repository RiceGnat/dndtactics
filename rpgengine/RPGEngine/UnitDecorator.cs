using System;

namespace RPGEngine
{
    [Serializable]
    public abstract class UnitDecorator : IUnit, IDecorator
    {
        protected IUnit unit;

        string IUnit.Name { get { return unit.Name; } }
        string IUnit.Class { get { return unit.Class; } }
        ushort IUnit.Level { get { return unit.Level; } }

        IStats IUnit.BaseStats { get { return unit.BaseStats; } }
        IStats IUnit.Stats { get { return unit.Stats; } }
        IStats IUnit.StatModsAdditive { get { return unit.StatModsAdditive; } }
        IStats IUnit.StatModsMultiplicative { get { return unit.StatModsMultiplicative; } }

        object IUnit.Extensions { get { return unit.Extensions; } }

        void IUnit.Evaluate() { unit.Evaluate(); }

        public virtual IUnit Bind(IUnit baseUnit)
        {
            unit = baseUnit;
            return this;
        }
    }
}
