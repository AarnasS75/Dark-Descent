using Characters.CharacterControls.AttackEvents;
using Helpers.RangeFinding;
using System.Collections;
using Tiles;
using UnityEngine;

namespace Characters.CharacterControls.Attack
{
    public class CharacterAttackController : MonoBehaviour
    {
        private ICharacter _character;
        private CharacterAttackStats _attackStats;
        private CharacterAttackEvents _attackEvents;

        private OverlayTile _targetTile;

        public void Initialize(ICharacter character, CharacterAttackStats attackStats, CharacterAttackEvents attackEvents)
        {
            _character = character;
            _attackStats = attackStats;
            _attackEvents = attackEvents;
        }

        public void Attack(OverlayTile selectedTile)
        {
            _targetTile = selectedTile;
            StartAttackRoutine();
        }

        private void StartAttackRoutine()
        {
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            RangeFinder.HideTiles();
            _targetTile.Mark();

            var target = _targetTile.GetStandingCharacter();

            target.TakeDamage(_character.Stats.AttackStats.AttackDamage);

            yield return new WaitForSeconds(1f);

            _character.UseActionPoint();
            _attackEvents.CallAttackEvent(_targetTile, _character);
        }

        public void MarkTilesInAttackRange()
        {
            RangeFinder.HideTiles();
            RangeFinder.MarkEnemiesInRangeTiles(_character.GetStandingTile(), _attackStats.AttackRange);
        }
    }
}