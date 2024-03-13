using System;
using Tiles;

namespace Characters.CharacterControls.AttackEvents
{
    public class CharacterAttackEvents
    {
        public event Action<CharacterAttackEvents, CharacterAttackedEventArgs> OnAttacked;

        public void CallAttackEvent(OverlayTile enemyTile, ICharacter character)
        {
            OnAttacked?.Invoke(this, new CharacterAttackedEventArgs
            {
                EnemyTile = enemyTile,
                Character = character
            });
        }
    }

    public class CharacterAttackedEventArgs : EventArgs
    {
        public OverlayTile EnemyTile;
        public ICharacter Character;
    }
}