using Characters.CharacterControls;
using UnityEngine;

namespace Characters.EnemyControls
{
    [RequireComponent(typeof(EnemyAIController))]
    public class Enemy : CharacterBase, IEnemy
    {
        private EnemyAIController _aIController;

        protected override void Awake()
        {
            base.Awake();
            _aIController = GetComponent<EnemyAIController>();
        }

        protected override void Start()
        {
            base.Start();
            _aIController.Initialize(this);
        }

        public void TakeAction(IPlayer player)
        {
            var actionScenario = _aIController.GetScenario(player);

            // If doesn't need to get closer and tile to attack is in range
            if (actionScenario.MoveToTile.GetPosition2D() == p_standingTile.GetPosition2D() && actionScenario.AttackTile != null)
            {
                p_attackController.Attack(actionScenario.AttackTile);
            }
            // If needs to get closer and after moving, tile to attack would still not be in range
            else
            {
                p_movementController.Move(actionScenario.MoveToTile);
            }
        }
    }
}