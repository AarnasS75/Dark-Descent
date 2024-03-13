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
            _aIController.Initialize(this, p_movementController, p_attackController);
        }

        public void TakeAction(IPlayer player)
        {
            _aIController.StartActionRoutine(player);
        }
    }
}