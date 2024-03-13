using Characters.Actions;
using Characters.CharacterControls.Attack;
using Characters.CharacterControls.Movement;
using Constants;
using System.Linq;
using Tiles;
using UnityEngine;

namespace Characters.CharacterControls.Player
{
    public class Player : CharacterBase, IPlayer
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void TakeAction(OverlayTile actionTile)
        {
            if (!CanTakeAction(actionTile))
            {
                return;
            }

            switch (p_selectedAction.ActionType)
            {
                case CombatActionType.Move:
                    p_movementController.Move(actionTile);
                    break;

                case CombatActionType.Attack:

                    break;
            }
        }

        public CombatAction SelectAction(CombatActionType actionType)
        {
            p_selectedAction = p_stats.CombatActions.First(x => x.ActionType.Equals(actionType));

            switch (p_selectedAction.ActionType)
            {
                case CombatActionType.Move:
                    p_movementController.ShowTilesInRange();
                    break;

                case CombatActionType.Attack:
                    p_movementController.ShowTilesInRange();
                    p_attackController.MarkTilesInAttackRange();
                    break;
            }

            return p_selectedAction;
        }
    }
}