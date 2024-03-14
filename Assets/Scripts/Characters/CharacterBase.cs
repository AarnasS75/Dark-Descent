using Characters.CharacterControls.Attack;
using Characters.CharacterControls.AttackEvents;
using Characters.CharacterControls.HealthEvents;
using Characters.CharacterControls.Movement;
using Characters.CharacterControls.MovementEvents;
using Characters.HealthControls;
using Constants;
using Helpers.RangeFinding;
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
    public class CharacterBase : MonoBehaviour, ICharacter
    {
        [SerializeField] protected CharacterStatsBase p_stats;

        public CharacterHealthEvents HealthEvents { get; private set; }
        public CharacterMovementEvents MovementEvents { get; private set; }
        public CharacterAttackEvents AttackEvents { get; private set; }

        private Health _health;
        private CharacterMovementController _movementController;
        private CharacterAttackController _attackController;

        private OverlayTile _standingTile;
        private CombatActionType _selectedAction;

        private int _availableActionsCount;

        protected virtual void Awake()
        {
            _health = GetComponent<Health>();
            _movementController = GetComponent<CharacterMovementController>();
            _attackController = GetComponent<CharacterAttackController>();

            HealthEvents = new CharacterHealthEvents();
            AttackEvents = new CharacterAttackEvents();
            MovementEvents = new CharacterMovementEvents();
        }

        protected virtual void Start()
        {
            _availableActionsCount = p_stats.ActionsPerTurnCount;
            _health.Initialize(this, p_stats.Health, HealthEvents);
            _movementController.Initialize(this, MovementEvents);
            _attackController.Initialize(this, AttackEvents);
        }

        #region Getters Actions Stuff

        public void SelectAction(CombatActionType selectedAction)
        {
            _selectedAction = selectedAction;

            switch (_selectedAction)
            {
                case CombatActionType.None:
                    RangeFinder.HideTiles();
                    break;

                case CombatActionType.Move:
                    RangeFinder.ShowTilesInRange(_standingTile, p_stats.MovementStats.MoveDistance);
                    break;

                case CombatActionType.Attack:
                    RangeFinder.HideTiles();
                    RangeFinder.MarkEnemiesInRangeTiles(_standingTile, p_stats.AttackStats.AttackRange);
                    break;
            }
        }

        public void TakeAction(OverlayTile actionTile)
        {
            switch (_selectedAction)
            {
                case CombatActionType.Move:
                    _movementController.Move(actionTile);
                    UseActionPoint();
                    break;

                case CombatActionType.Attack:
                    _attackController.Attack(actionTile);
                    UseActionPoint();
                    break;

                case CombatActionType.None:
                    ResetSelectedAction();
                    break;
            }
        }

        public int GetAvailableActionPoints()
        {
            return _availableActionsCount;
        }

        protected void UseActionPoint()
        {
            _availableActionsCount--;
        }

        protected void ResetSelectedAction()
        {
            _selectedAction = CombatActionType.None;
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
            return p_stats.AttackStats.AttackDamage;
        }

        public int GetAttackRange()
        {
            return p_stats.AttackStats.AttackRange;
        }

        #endregion

        #region Getters Movement Stuff
        public float GetMoveSpeed()
        {
            return p_stats.MovementStats.MoveSpeed;
        }

        public int GetMoveRange()
        {
            return p_stats.MovementStats.MoveDistance;
        }
        #endregion

        #region Getters Character Position Stuff

        public void SetStandingTile(OverlayTile standingTile)
        {
            _standingTile = standingTile;
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