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

        protected OverlayTile p_standingTile;
        protected CombatActionType p_selectedAction;

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

        public int GetAvailableActionPoints()
        {
            return _availableActionsCount;
        }

        public void UseActionPoint()
        {
            _availableActionsCount--;
        }

        public void ResetSelectedAction()
        {
            p_selectedAction = CombatActionType.None;
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

        public int GetHealth()
        {
            return p_health.GetCurrentHealth();
        }

        public void TakeDamage(int ammount)
        {
            p_health.TakeDamage(ammount);
        }

        public void SetStandingTile(OverlayTile standingTile)
        {
            p_standingTile = standingTile;
        }

        public OverlayTile GetStandingTile()
        {
            return p_standingTile;
        }

        public Vector2Int GetPosition2D()
        {
            Vector3 position = transform.position;
            Vector2Int positionInt = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            return positionInt;
        }
    }
}