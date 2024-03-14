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

        public bool CreateActionScenario(IPlayer player)
        {
            return _aIController.ActionScenarioOutcome(player);
        }

        public int GetExp()
        {
            return 100;
        }

    }
}