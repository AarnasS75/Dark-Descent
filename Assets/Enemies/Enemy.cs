using Characters.CharacterControls;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Characters.EnemyControls
{
    [RequireComponent(typeof(EnemyAIController))]
    public class Enemy : CharacterBase<EnemyStats>, IEnemy
    {
        private EnemyAIController _aIController;

        protected override void Awake()
        {
            base.Awake();
            _aIController = GetComponent<EnemyAIController>();
        }

        public override void Initialize(Dictionary<Vector2Int, OverlayTile> roomMap)
        {
            base.Initialize(roomMap);
            _aIController.Initialize(this, _pathFinder);
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