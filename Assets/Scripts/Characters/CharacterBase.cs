using Characters.CharacterControls.Attack;
using Characters.CharacterControls.AttackEvents;
using Characters.CharacterControls.HealthEvents;
using Characters.CharacterControls.Movement;
using Characters.CharacterControls.MovementEvents;
using Characters.HealthControls;
using Constants;
using Helpers.PathFinding;
using Helpers.RangeFinding;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Characters.CharacterControls
{
    #region Required Components
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(CharacterMovementController))]
    [RequireComponent(typeof(CharacterAttackController))]
    [DisallowMultipleComponent]
    #endregion
    public class CharacterBase<T> : MonoBehaviour, ICharacter where T : CharacterStatsBase
    {
        [SerializeField] protected T _stats;

        public CharacterHealthEvents HealthEvents { get; private set; }
        public CharacterMovementEvents MovementEvents { get; private set; }
        public CharacterAttackEvents AttackEvents { get; private set; }

        private Health _health;
        private CharacterMovementController _movementController;
        private CharacterAttackController _attackController;

        private OverlayTile _standingTile;
        public CombatActionType _selectedAction;

        protected RangeFinder _rangeFinder;
        protected PathFinder _pathFinder;

        public int _availableActionsCount;

        protected virtual void Awake()
        {
            _health = GetComponent<Health>();
            _movementController = GetComponent<CharacterMovementController>();
            _attackController = GetComponent<CharacterAttackController>();

            HealthEvents = new CharacterHealthEvents();
            AttackEvents = new CharacterAttackEvents();
            MovementEvents = new CharacterMovementEvents();
        }

        public virtual void Initialize(Dictionary<Vector2Int, OverlayTile> roomMap)
        {
            _availableActionsCount = _stats.ActionsPerTurnCount;

            _rangeFinder = new RangeFinder(roomMap);
            _pathFinder = new PathFinder(roomMap);

            _health.Initialize(this, _stats.Health, HealthEvents);
            _movementController.Initialize(this, MovementEvents, _pathFinder);
            _attackController.Initialize(this, AttackEvents, _rangeFinder);
        }

        public void TakeAction(CombatActionType selectedAction, OverlayTile actionTile = null)
        {
            if (_attackController.IsAttacking || _movementController.IsMoving)
            {
                return;
            }

            _selectedAction = selectedAction;

            switch (_selectedAction)
            {
                case CombatActionType.Move:
                    if (actionTile != null)
                    {
                        _movementController.Move(actionTile);
                    }
                    else
                    {
                        _pathFinder.ShowAvailableTilesWithinMoveRange(_standingTile, _stats.MovementStats.MoveDistance);
                    }
                    break;

                case CombatActionType.Attack:
                    if (actionTile != null)
                    {
                        _attackController.Attack(actionTile);
                    }
                    else
                    {
                        _rangeFinder.ShowTilesInAttackRange(_standingTile, _stats.AttackStats.AttackRange);
                    }
                    break;

                case CombatActionType.EndTurn:
                    _rangeFinder.HideTiles();
                    _selectedAction = CombatActionType.EndTurn;
                    ResetActionCount();
                    break;
            }
        }

        #region Getters Actions Stuff
        public CombatActionType GetSelectedAction()
        {
            return _selectedAction;
        }

        public void UseActionPoint()
        {
            _availableActionsCount--;
        }

        public int GetRemainingActionsCount()
        {
            return _availableActionsCount;
        }

        public int GetActionsPerTurnCount()
        {
            return _stats.ActionsPerTurnCount;
        }

        protected void ResetActionCount()
        {
            _availableActionsCount = _stats.ActionsPerTurnCount;
        }
        #endregion

        #region Getters Health Stuff

        public int GetHealth()
        {
            return _health.GetCurrentHealth();
        }

        public int GetMaxHealth()
        {
            return _health.GetMaxHealth();
        }

        public void TakeDamage(int ammount)
        {
            _health.TakeDamage(ammount);
        }

        #endregion

        #region Getters Attack Stuff
        public int GetAttackDamage()
        {
            return _stats.AttackStats.AttackDamage;
        }

        public int GetAttackRange()
        {
            return _stats.AttackStats.AttackRange;
        }

        #endregion

        #region Getters Movement Stuff
        public float GetMoveSpeed()
        {
            return _stats.MovementStats.MoveSpeed;
        }

        public int GetMoveRange()
        {
            return _stats.MovementStats.MoveDistance;
        }
        #endregion

        #region Getters Character Position Stuff

        public void Place(OverlayTile standingTile)
        {
            _standingTile = standingTile;
            transform.position = new Vector3(_standingTile.transform.position.x, _standingTile.transform.position.y + 0.0001f, _standingTile.transform.position.z);
        }

        public OverlayTile GetStandingTile()
        {
            return _standingTile;
        }

        public Vector2Int GetPosition2D()
        {
            Vector3 position = transform.position;
            Vector2Int positionInt = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            return positionInt;
        }
        #endregion

    }
}