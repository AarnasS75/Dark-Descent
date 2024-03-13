using System;

namespace Characters.CharacterControls.HealthEvents
{
    public class CharacterHealthEvents
    {
        public event Action<CharacterHealthEvents, HealthChangedEventArgs> OnHealthChanged;

        public void CallHealthChangedEvent(ICharacter character, float damagePercentage, int damageDealt)
        {
            OnHealthChanged?.Invoke(this, new HealthChangedEventArgs()
            {
                Character = character,
                DamagePercentage = damagePercentage,
                DamageDealt = damageDealt
            });
        }

        public class HealthChangedEventArgs : EventArgs
        {
            public ICharacter Character;
            public float DamagePercentage;
            public int DamageDealt;
        }
    }
}