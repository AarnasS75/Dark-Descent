using Constants;
using Tiles;

namespace Characters.CharacterControls.Player
{
    public class Player : CharacterBase, IPlayer
    {
        public void SelectAction(CombatActionType actionType)
        {
            p_selectedAction = actionType;

            switch (p_selectedAction)
            {
                case CombatActionType.Move:
                    p_movementController.ShowTilesInRange();
                    break;

                case CombatActionType.Attack:
                    p_attackController.MarkTilesInAttackRange();
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