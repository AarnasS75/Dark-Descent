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

        protected Health p_health;
        protected CharacterMovementController p_movementController;
        protected CharacterAttackController p_attackController;

        public OverlayTile p_standingTile;
        public CombatActionType p_selectedAction;

        public int p_availableActionsCount;

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
            p_availableActionsCount = p_stats.ActionsPerTurnCount;
            p_health.Initialize(this, p_stats.Health, HealthEvents);
            p_movementController.Initialize(this, MovementEvents);
            p_attackController.Initialize(this, AttackEvents);
        }

        #region Getters Actions Stuff

        public void SelectAction(CombatActionType selectedAction)
        {
            p_selectedAction = selectedAction;

            switch (p_selectedAction)
            {
                case CombatActionType.None:
                    RangeFinder.HideTiles();
                    break;

                case CombatActionType.Move:
                    RangeFinder.ShowTilesInRange(p_standingTile, p_stats.MovementStats.MoveDistance);
                    break;

                case CombatActionType.Attack:
                    RangeFinder.HideTiles();
                    RangeFinder.MarkEnemiesInRangeTiles(p_standingTile, p_stats.AttackStats.AttackRange);
                    break;
            }
        }

        public void TakeAction(OverlayTile actionTile)
        {
            switch (p_selectedAction)
            {
                case CombatActionType.Move:
                    p_movementController.Move(actionTile);
                    UseActionPoint();
                    break;

                case CombatActionType.Attack:
                    p_attackController.Attack(actionTile);
                    UseActionPoint();
                    break;

                case CombatActionType.None:
                    ResetSelectedAction();
                    break;
            }
        }

        public int GetAvailableActionPoints()
        {
            return p_availableActionsCount;
        }

        protected void UseActionPoint()
        {
            p_availableActionsCount--;
        }

        protected void ResetSelectedAction()
        {
            p_selectedAction = CombatActionType.None;
            p_availableActionsCount = p_stats.ActionsPerTurnCount;
        }

        public int GetRemainingActionsCount()
        {
            return p_availableActionsCount;
        }

        public int GetActionsPerTurnCount()
        {
            return p_stats.ActionsPerTurnCount;
        }

        #endregion

        #region Getters Health Stuff

        public int GetHealth()
        {
            return p_health.GetCurrentHealth();
        }

        public int GetMaxHealth()
        {
            return p_health.GetMaxHealth();
        }

        public void TakeDamage(int ammount)
        {
            p_health.TakeDamage(ammount);
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
        #endregion

    }
}