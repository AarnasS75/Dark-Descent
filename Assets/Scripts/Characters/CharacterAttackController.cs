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
        private bool _isAttacking;

        private void Awake()
        {
            _isAttacking = false;
        }

        public void Initialize(ICharacter character, CharacterAttackEvents attackEvents)
        {
            _character = character;
            _attackEvents = attackEvents;
        }

        public void Attack(OverlayTile selectedTile)
        {
            if (selectedTile.IsAvailable && selectedTile.GetPosition2D() != _character.GetStandingTile().GetPosition2D() && _isAttacking)
            {
                return;
            }

            _isAttacking = true;
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

            _isAttacking = false;

            var target = _targetTile.GetStandingCharacter();

            if (target != null)
            {
                target.TakeDamage(_character.GetAttackDamage());
                _attackEvents.CallAttackEvent(_targetTile, _character, target);
            }
        }
    }
}