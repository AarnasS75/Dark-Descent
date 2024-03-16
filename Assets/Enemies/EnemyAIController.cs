using Constants;
using Helpers.PathFinding;
using Helpers.RangeFinding;
using System.Collections.Generic;
using System.Linq;
using Tiles;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Characters.EnemyControls
{
    public class EnemyAIController : MonoBehaviour
    {
        private IEnemy _enemy;
        private IPlayer _player;

        private PathFinder _pathFinder;
        private ActionScenario _previousScenario;

        public void Initialize(IEnemy enemy, PathFinder pathFinder)
        {
            _enemy = enemy;
            _pathFinder = pathFinder;
        }

        public bool ActionScenarioOutcome(IPlayer player)
        {
            if(_enemy.GetRemainingActionsCount() <= 0)
            {
                _enemy.TakeAction(CombatActionType.EndTurn, null);
                return false;
            }

            _player = player;
            var bestScenario = new ActionScenario();

            var tileInMovementRange = _pathFinder.GetAvailableTilesWithinMoveRange(_enemy.GetStandingTile(), _enemy.GetMoveRange());
            tileInMovementRange.Add(_enemy.GetStandingTile());

            foreach (var tempTile in tileInMovementRange)
            {
                ActionScenario possibleScenario = new();


                // Find the tile to go to, to be in range for attack
                var closestDistance = _pathFinder.GetManhattenDistance(tempTile, _player.GetStandingTile());

                if (_enemy.GetAttackRange() >= closestDistance)
                {
                    var scenarioValue = _enemy.GetAttackDamage() + closestDistance - _player.GetHealth();

                    if(_player.GetHealth() - _enemy.GetAttackDamage() <= 0)
                    {
                        scenarioValue = 10000;
                    }

                    possibleScenario = new ActionScenario(scenarioValue, _player.GetStandingTile(), tempTile);
                }




                // Get better value scenario
                if (possibleScenario.Value > bestScenario.Value)
                {
                    bestScenario = possibleScenario;
                }




                // If two scenarios have same value, get the one that requires the shortest path
                if (possibleScenario.MoveToTile != null && possibleScenario.Value == bestScenario.Value)
                {
                    var tempSenarioPathCount = _pathFinder.FindPath(_enemy.GetStandingTile(), possibleScenario.MoveToTile, tileInMovementRange).Count;
                    var senarioPathCount = _pathFinder.FindPath(_enemy.GetStandingTile(), bestScenario.MoveToTile, tileInMovementRange).Count;

                    if (tempSenarioPathCount < senarioPathCount)
                    {
                        bestScenario = possibleScenario;
                    }
                }





                // If there are no tiles to go to, to be in range for attack, then just find the closest tile to the player and is in walking range
                if (possibleScenario.MoveToTile == null && bestScenario.AttackTile == null)
                {
                    var senarioFullPath = _pathFinder.FindPath(_enemy.GetStandingTile(), _player.GetStandingTile(), new List<OverlayTile>());

                    if (senarioFullPath.Count > _enemy.GetAttackRange() + _enemy.GetMoveRange())
                    {
                        var senarioPath = senarioFullPath.GetRange(0, _enemy.GetMoveRange());
                        var senarioValue = senarioPath.Count - _player.GetHealth();

                        if (senarioValue < bestScenario.Value || !bestScenario.MoveToTile)
                        {
                            bestScenario = new ActionScenario(senarioValue, null, senarioPath.Last());
                        }
                    }
                }



               






            }

            if (bestScenario.AttackTile != null && _enemy.GetStandingTile().GetPosition2D() == bestScenario.MoveToTile.GetPosition2D())
            {
                _enemy.TakeAction(CombatActionType.Attack, bestScenario.AttackTile);
            }
            else if (bestScenario.MoveToTile != null)
            {
                _enemy.TakeAction(CombatActionType.Move, bestScenario.MoveToTile);
            }


            return true;
        }
    }
}