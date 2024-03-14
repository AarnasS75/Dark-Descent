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
        private CharacterAttackEvents _attackEvents;

        private OverlayTile _targetTile;

        public void Initialize(ICharacter character, CharacterAttackEvents attackEvents)
        {
            _character = character;
            _attackEvents = attackEvents;
        }

        public void Attack(OverlayTile selectedTile)
        {
            if (selectedTile.IsAvailable && selectedTile.GetPosition2D() != _character.GetStandingTile().GetPosition2D())
            {
                return;
            }

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

            yield return new WaitForSeconds(0.5f);

            var target = _targetTile.GetStandingCharacter();

            target.TakeDamage(_character.GetAttackDamage());

            _attackEvents.CallAttackEvent(_targetTile, _character, target);
        }
    }
}