using Characters.Actions;
using Characters.CharacterControls.Attack;
using Characters.CharacterControls.AttackEvents;
using Characters.CharacterControls.HealthEvents;
using Characters.CharacterControls.Movement;
using Characters.CharacterControls.MovementEvents;
using Characters.HealthControls;
using Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using Tiles;
using TMPro;
using UnityEngine;

namespace Characters.CharacterControls
{
    #region Required Components
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(CharacterMovementController))]
    [RequireComponent(typeof(CharacterAttackController))]
    [DisallowMultipleComponent]
    #endregion
    public class CharacterBase : MonoBehaviour, ICharacter
    {
        [SerializeField] protected CharacterStatsBase p_stats;
        public CharacterStatsBase Stats => p_stats;

        public CharacterHealthEvents HealthEvents { get; private set; }
        public CharacterMovementEvents MovementEvents { get; private set; }
        public CharacterAttackEvents AttackEvents { get; private set; }

        protected Health p_health;
        protected CharacterMovementController p_movementController;
        protected CharacterAttackController p_attackController;

        protected OverlayTile p_standingOnTile;
        protected CombatAction p_selectedAction;

        protected int _availableActionsCount;

        protected virtual void Awake()
        {
            p_health = GetComponent<Health>();
            p_movementController = GetComponent<CharacterMovementController>();
            p_attackController = GetComponent<CharacterAttackController>();

            HealthEvents = new CharacterHealthEvents();
            AttackEvents = new CharacterAttackEvents();
            MovementEvents = new CharacterMovementEvents();
        }

        protected virtual void Start()
        {
            _availableActionsCount = p_stats.ActionsPerTurnCount;
            p_health.Initialize(this, p_stats.Health, HealthEvents);
            p_movementController.Initialize(this, p_stats.MovementStats, MovementEvents);
            p_attackController.Initialize(this, p_stats.AttackStats, AttackEvents);
        }

        public void SetPosition(OverlayTile tile)
        {
            transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
            p_standingOnTile = tile;
            p_standingOnTile.Occupy(this);

            if (p_standingOnTile.Previous != null)
            {
                p_standingOnTile.Previous.ResetTile();
            }
        }

        public int GetAvailableActionPoints()
        {
            return _availableActionsCount;
        }

        public void UseActionPoint()
        {
            _availableActionsCount--;
        }

        public List<CombatAction> GetAvailableActions()
        {
            return p_stats.CombatActions.Where(x => _availableActionsCount - 1 >= 0).ToList();
        }

        public bool TryGetNextTurn(out CombatAction combatAction)
        {
            combatAction = null;

            foreach (var action in p_stats.CombatActions)
            {
                if (_availableActionsCount - 1 >= 0)
                {
                    combatAction = action;
                    return true;
                }
            }

            return false;
        }

        public void ResetAction()
        {
            p_selectedAction = null;
            _availableActionsCount = p_stats.ActionsPerTurnCount;
        }

        public int GetRemainingActionsCount()
        {
            return _availableActionsCount;
        }

        public int GetActionsPerTurnCount()
        {
            return p_stats.ActionsPerTurnCount;
        }

        public OverlayTile GetStandingTile()
        {
            return p_standingOnTile;
        }

        protected bool CanTakeAction(OverlayTile selectedTile)
        {
            if (selectedTile == null || p_selectedAction == null)
            {
                return false;
            }

            if (_availableActionsCount - 1 < 0)
            {
                return false;
            }

            return true;
        }

        public int GetHealth()
        {
            return p_health.GetCurrentHealth();
        }

        public void TakeDamage(int ammount)
        {
            p_health.TakeDamage(ammount);
        }
    }
}