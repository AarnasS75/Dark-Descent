using System;
using Tiles;

namespace Characters.CharacterControls.AttackEvents
{
    public class CharacterAttackEvents
    {
        public event Action<CharacterAttackEvents, CharacterAttackedEventArgs> OnAttacked;

        public void CallAttackEvent(OverlayTile enemyTile, ICharacter attacker, ICharacter target)
        {
            OnAttacked?.Invoke(this, new CharacterAttackedEventArgs
            {
                EnemyTile = enemyTile,
                Attacker = attacker,
                Target = target
            });
        }
    }

    public class CharacterAttackedEventArgs : EventArgs
    {
        public OverlayTile EnemyTile;
        public ICharacter Attacker;
        public ICharacter Target;
    }
}