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
        public bool IsAttacking { get; private set; }

        private RangeFinder _rangeFinder;

        private void Awake()
        {
            IsAttacking = false;
        }

        public void Initialize(ICharacter character, CharacterAttackEvents attackEvents, RangeFinder rangeFinder)
        {
            _character = character;
            _attackEvents = attackEvents;

            _rangeFinder = rangeFinder;
        }

        public void Attack(OverlayTile selectedTile)
        {
            if (!CanAttack(selectedTile))
            {
                return;
            }

            if (_character is IPlayer player && !PlayerStaminaTracker.Instance.UseStamina(10))
            {
                return;
            }

            _targetTile = selectedTile;
            IsAttacking = true;
            _rangeFinder.HideTiles();
            _targetTile.Mark();

            StartAttackRoutine();
        }

        private void StartAttackRoutine()
        {
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            yield return new WaitForSeconds(0.5f);

            IsAttacking = false;

            var target = _targetTile.GetStandingCharacter();

            if (target != null)
            {
                _character.UseActionPoint();
                target.TakeDamage(_character.GetWeapon().GetAttackDamage());
                _attackEvents.CallAttackEvent(_targetTile, _character, target);
            }
        }

        private bool CanAttack(OverlayTile selectedTile)
        {
            if (selectedTile.IsAvailable || 
                IsAttacking || 
                selectedTile.GetPosition2D() == _character.GetStandingTile().GetPosition2D() ||
                _character.GetRemainingActionsCount() <= 0)
            {
                return false;
            }
            return true;
        }
    }
}