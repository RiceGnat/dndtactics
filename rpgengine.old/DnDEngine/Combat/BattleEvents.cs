using System;
using System.Collections.Generic;
using RPGEngine;

namespace DnDEngine.Combat
{
    public class BattleEventArgs : EventArgs
    {
        new public static readonly BattleEventArgs Empty;

        public CombatEntity ActiveUnit { get; private set; }
        public IEnumerable<CombatEntity> Targets { get; private set; }

        public ILoggable Message { get; private set; }

        public BattleEventArgs(CombatEntity activeUnit)
        {
            ActiveUnit = activeUnit;
        }

        public BattleEventArgs(ILoggable message)
        {
            Message = message;
        }

        public BattleEventArgs(string message)
        {
            Message = new LoggableString(message);
        }

        public BattleEventArgs(CombatEntity activeUnit, IEnumerable<CombatEntity> targets, ILoggable message)
            : this(activeUnit)
        {
            Targets = targets;
            Message = message;
        }
    }

    public delegate void BattleEventHandler(BattleInstance sender, BattleEventArgs e);

    public partial class BattleInstance
    {
        public event BattleEventHandler BattleInitialized;
        public event BattleEventHandler BattleStarted;

        public event BattleEventHandler TurnStarted;

        public event BattleEventHandler ActionStarted;
        public event BattleEventHandler ActionEnded;

        public event BattleEventHandler MoveStarted;
        public event BattleEventHandler MoveEnded;

        public event BattleEventHandler AttackStarted;
        public event BattleEventHandler AttackEnded;

        public event BattleEventHandler SpellStarted;
        public event BattleEventHandler SpellEnded;

        public event BattleEventHandler Delayed;

        public event BattleEventHandler TurnEnded;

        public event BattleEventHandler BattleEnded;

        public event BattleEventHandler MessageLogged;

        private void TriggerEvent(BattleEventHandler handler, BattleEventArgs e = null)
        {
            if (handler != null) handler(this, e ?? BattleEventArgs.Empty);
        }
    }
}
