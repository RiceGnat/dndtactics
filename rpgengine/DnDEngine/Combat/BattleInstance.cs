using System;
using System.Collections.Generic;
using System.Linq;
using RPGEngine;

namespace DnDEngine.Combat
{
    public partial class BattleInstance
    {
        public IList<ILoggable> BattleLog { get; private set; }

        public bool IsReady { get; private set; }
        public bool IsStarted { get; private set; }
        public int Round { get; private set; }

        public IList<CombatEntity> Participants { get; private set; }
        public IList<CombatEntity> TurnOrder { get; private set; }
        public CombatEntity ActiveUnit { get { return TurnOrder[0]; } }

        public IList<CombatEntity> Team(int teamId)
        {
            return Participants.Where(unit => unit.TeamID == teamId).ToList();
        }

        public void AddUnit(IUnit unit, bool inanimate = false)
        {
            AddUnit(new CombatEntity(unit, !inanimate));
        }

        public void AddUnit(CombatEntity unit)
        {
            Participants.Add(unit);

            if (IsReady)
            {
                unit.Initialize();
                unit.Initiative = InitiativeDetails.Zero(unit);
                TurnOrder.Add(unit);
            }
        }

        public void RemoveUnit(CombatEntity unit)
        {
            Participants.Remove(unit);
            TurnOrder.Remove(unit);
        }

        #region Battle control
        public void Initialize()
        {
            Log("Preparing battle...");

            // Get units that can take actions
            List<CombatEntity> turnOrder = Participants.Where(unit => unit.CanAct).ToList();

            // Roll initiatives
            foreach (CombatEntity unit in turnOrder)
            {
                unit.Initialize();
                Log(unit.Initiative = CombatManager.RollInitiative(unit));
            }

            // Sort into turn order
            turnOrder.Sort(CombatEntity.Compare);

            // Assign to property
            TurnOrder = turnOrder;

            // Battle is ready to start
            IsReady = true;

            // BattleInitialized event
            TriggerEvent(BattleInitialized);
        }

        public void Start()
        {
            // Battle has started
            IsStarted = true;
            Log("The battle has started");

            // BattleStarted event
            TriggerEvent(BattleStarted);

            // Begin first turn
            BeginTurn();
        }

        public void NextTurn()
        {
            Log(String.Format("{0} ends {1} turn", ActiveUnit.Name, Constants.PossessivePronoun(ActiveUnit.Details.Gender)));

            // TurnEnded event
            TriggerEvent(TurnEnded, new BattleEventArgs(ActiveUnit));

            // Cycle turn order
            TurnOrder.Add(TurnOrder.First());
            TurnOrder.RemoveAt(0);

            // Begin the next turn
            BeginTurn();
        }

        private void BeginTurn()
        {
            Log(String.Format("{0}'s turn begins", ActiveUnit.Name));

            // Unit upkeep actions
            foreach (var buff in ActiveUnit.Buffs.All)
            {
                Log(buff.Upkeep(ActiveUnit));

                if (buff.Remaining <= 0) ActiveUnit.Buffs.RemoveBuffFromUnit(buff);
            }

            // TurnStarted event
            TriggerEvent(TurnStarted, new BattleEventArgs(ActiveUnit));
        }

        public void End()
        {
            // Battle has ended
            IsStarted = false;
            IsReady = false;

            // BattleEnded event
            TriggerEvent(BattleEnded);

            // Cleanup actions (gain exp, etc)
        }
        #endregion

        #region Unit actions
        public ILoggable Attack(CombatEntity target)
        {
            // AttackStarted event
            TriggerEvent(AttackStarted);

            ILoggable msg = Log(CombatManager.RollAttack(ActiveUnit, target));

            // AttackEnded event
            TriggerEvent(AttackEnded);

            return msg;
        }

        public ILoggable CastSpell(uint spellId, IEnumerable<CombatEntity> targets)
        {
            // SpellStarted event
            TriggerEvent(SpellStarted);

            ILoggable msg = Log(CombatManager.CastSpell(spellId, ActiveUnit, targets));

            // SpellEnded event
            TriggerEvent(SpellEnded);

            return null;
        }

        public ILoggable Delay(int moveAfter)
        {
            ILoggable msg = Log(String.Format("{0} delays {1} turn to move after {2}", ActiveUnit.Name, Constants.PossessivePronoun(ActiveUnit.Details.Gender), TurnOrder[moveAfter]));

            // Move active unit to after target index
            TurnOrder.Insert(moveAfter + 1, ActiveUnit);
            TurnOrder.RemoveAt(0);

            // Delayed event
            TriggerEvent(Delayed);

            return msg;
        }
        #endregion

        #region Logging
        public ILoggable Log(ILoggable message)
        {
            if (message != null)
            {
                // Add to battle log
                BattleLog.Add(message);

                // MessageLogged event
                TriggerEvent(MessageLogged, new BattleEventArgs(message));
            }

            return message;
        }

        public ILoggable Log(string message)
        {
            return Log(new LoggableString(message));
        }
        #endregion

        public BattleInstance()
        {
            BattleLog = new List<ILoggable>();
            IsReady = false;
            IsStarted = false;
            Participants = new List<CombatEntity>();
        }
    }
}
