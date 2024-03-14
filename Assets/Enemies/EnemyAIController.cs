using Constants;
using Helpers.PathFinding;
using Helpers.RangeFinding;
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

        public void Initialize(IEnemy enemy)
        {
            _enemy = enemy;
        }

        public bool ActionScenarioOutcome(IPlayer player)
        {
            if(_enemy.GetRemainingActionsCount() <= 0)
            {
                _enemy.SelectAction(CombatActionType.None);
                _enemy.TakeAction(null);
                return false;
            }

            _player = player;
            _standingOnTile = _enemy.GetStandingTile();

            var senario = new ActionScenario();

            var tileInMovementRange = RangeFinder.GetTilesInRange(_standingOnTile, _enemy.GetMoveRange());

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

                    if (senarioFullPath.Count > _enemy.GetAttackRange() + _enemy.GetMoveRange())
                    {
                        var senarioPath = senarioFullPath.GetRange(0, _enemy.GetMoveRange());
                        var senarioValue = senarioPath.Count - _player.GetHealth();

                        if (senarioValue < senario.ScenarioValue || !senario.MoveToTile)
                        {
                            senario = new ActionScenario(senarioValue, null, senarioPath.Last());
                        }
                    }
                }
            }

            // If doesn't need to get closer and tile to attack is in range
            if (senario.MoveToTile.GetPosition2D() == _enemy.GetStandingTile().GetPosition2D() && senario.AttackTile != null)
            {
                _enemy.SelectAction(CombatActionType.Attack);
                _enemy.TakeAction(senario.AttackTile);
            }
            // If needs to get closer and after moving, tile to attack would still not be in range
            else
            {
                _enemy.SelectAction(CombatActionType.Move);
                _enemy.TakeAction(senario.MoveToTile);
            }

            return true;
        }
        
        private ActionScenario CreateTileSenarioValue(OverlayTile tempTile)
        {
            // NOTE: Can be extended if added character personality types.
            // For example aggresive ones prioritize attacking player, support ones focus on protecting alies, etc.

            var attackScnario = StartegicPlayerAttack(tempTile);

            return attackScnario;
        }

        private ActionScenario StartegicPlayerAttack(OverlayTile tempTile)
        {
            if (_player != null)
            {
                var closestDistance = PathFinder.GetManhattenDistance(tempTile, _player.GetStandingTile());

                if (_enemy.GetAttackRange() >= closestDistance)
                {
                    var scenarioValue = _enemy.GetAttackDamage() + closestDistance - _player.GetHealth();

                    if(_player.GetHealth() - _enemy.GetAttackDamage() <= 0)
                    {
                        scenarioValue = 10000;
                    }

                    return new ActionScenario(scenarioValue, _player.GetStandingTile(), tempTile);
                }
            }

            return new ActionScenario();
        }
    }
}