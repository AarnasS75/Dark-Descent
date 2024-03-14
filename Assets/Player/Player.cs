using Constants;
using Helpers.RangeFinding;
using Tiles;

namespace Characters.CharacterControls.Player
{
    public class Player : CharacterBase, IPlayer
    {
        public LevelSystem LevelSystem { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LevelSystem = new LevelSystem(this);
        }

        public void SelectAction(CombatActionType actionType)
        {
            p_selectedAction = actionType;

            switch (p_selectedAction)
            {
                case CombatActionType.None:
                    ResetSelectedAction();
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
            if (!CanTakeAction(actionTile))
            {
                return;
            }

            switch (p_selectedAction)
            {
                case CombatActionType.Move:
                    p_movementController.Move(actionTile);
                    break;

                case CombatActionType.Attack:
                    p_attackController.Attack(actionTile);
                    break;
            }

            UseActionPoint();
        }

        private bool CanTakeAction(OverlayTile selectedTile)
        {
            if (selectedTile == null || p_selectedAction == CombatActionType.None)
            {
                return false;
            }

            if (_availableActionsCount - 1 < 0)
            {
                return false;
            }

            return true;
        }
    }
}