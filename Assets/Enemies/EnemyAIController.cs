using Characters.CharacterControls.Attack;
using Characters.CharacterControls.Movement;
using Helpers.PathFinding;
using Helpers.RangeFinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tiles;
using UnityEngine;

namespace Characters.EnemyControls
{
    public class EnemyAIController : MonoBehaviour
    {
        private IEnemy _enemy;
        private IPlayer _player;

        private OverlayTile _standingOnTile;

        private CharacterAttackController _attackController;
        private CharacterMovementController _movementController;

        public void Initialize(IEnemy enemy, CharacterMovementController movementController, CharacterAttackController attackController)
        {
            _enemy = enemy;
            _attackController = attackController;
            _movementController = movementController;
        }

        public void StartActionRoutine(IPlayer player)
        {
            _player = player;
            _standingOnTile = _enemy.GetStandingTile();

            ScenarioRecoursively();
        }

        private void ScenarioRecoursively()
        {
            var senario = new ActionScenario();

            var tileInMovementRange = RangeFinder.GetTilesInRange(_standingOnTile.GetGridLocation2D(), _enemy.Stats.MovementStats.MoveDistance);

            foreach (var tile in tileInMovementRange)
            {
                var tempSenario = CreateTileSenarioValue(tile);

                if (tempSenario != null && tempSenario.ScenarioValue > senario.ScenarioValue)
                {
                    senario = tempSenario;
                }

                if (tempSenario.MoveToTile != null && tempSenario.ScenarioValue == senario.ScenarioValue)
                {
                    var tempSenarioPathCount = PathFinder.FindPath(_standingOnTile, tempSenario.MoveToTile, tileInMovementRange).Count;
                    var senarioPathCount = PathFinder.FindPath(_standingOnTile, senario.MoveToTile, tileInMovementRange).Count;

                    if (tempSenarioPathCount < senarioPathCount)
                    {
                        senario = tempSenario;
                    }
                }

                if (tempSenario.MoveToTile == null && senario.AttackTile == null)
                {
                    var senarioFullPath = PathFinder.FindPath(_standingOnTile, _player.GetStandingTile(), new List<OverlayTile>());

                    if (senarioFullPath.Count > _enemy.Stats.AttackStats.AttackRange + _enemy.Stats.MovementStats.MoveDistance)
                    {
                        var senarioPath = senarioFullPath.GetRange(0, _enemy.Stats.MovementStats.MoveDistance);
                        var senarioValue = senarioPath.Count - _player.GetHealth();

                        if (senarioValue < senario.ScenarioValue || !senario.MoveToTile)
                        {
                            senario = new ActionScenario(senarioValue, null, senarioPath.Last());
                        }
                    }
                }
            }

            senario.MoveToTile.Show();
            _standingOnTile.ResetTile();

            // If doesn't need to get closer and tile to attack is in range
            if (senario.MoveToTile.GetGridLocation2D() == _standingOnTile.GetGridLocation2D() && senario.AttackTile != null)
            {
                _attackController.Attack(senario.AttackTile);
            }
            // If needs to get closer and after moving, tile to attack would still not be in range
            else
            {
                _movementController.Move(senario.MoveToTile);
            }
        }
        
        private ActionScenario CreateTileSenarioValue(OverlayTile tempTile)
        {
            var attackScnario = StartegicPlayerAttack(tempTile);

            return attackScnario;
        }

        private ActionScenario StartegicPlayerAttack(OverlayTile tempTile)
        {
            if (_player != null)
            {
                var closestDistance = PathFinder.GetManhattenDistance(tempTile, _player.GetStandingTile());

                if (_enemy.Stats.AttackStats.AttackRange >= closestDistance)
                {
                    var scenarioValue = _enemy.Stats.AttackStats.AttackDamage + closestDistance - _player.GetHealth();

                    if(_player.GetHealth() - _enemy.Stats.AttackStats.AttackDamage <= 0)
                    {
                        scenarioValue = 10000;
                    }

                    return new ActionScenario(scenarioValue, _player.GetStandingTile(), tempTile);
                }
            }

            return new ActionScenario();
        }

        private bool HasActionAvailable()
        {
            return _enemy.Stats.CombatActions.Any(x => _enemy.GetRemainingActionsCount() - 1 >= 0);
        }
    }
}