using Characters.CharacterControls.HealthEvents;
using UnityEngine;

namespace Characters.HealthControls
{
    public class Health : MonoBehaviour
    {
        private int _maxHealth;
        private int _currentHealth;

        private CharacterHealthEvents _healthEvents;
        private ICharacter _character;

        public void Initialize(ICharacter character, int health, CharacterHealthEvents healthEvents)
        {
            _character = character;
            _maxHealth = health;
            _currentHealth = health;
            _healthEvents = healthEvents;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            _healthEvents.CallHealthChangedEvent(_character, (float)_currentHealth / (float)_maxHealth, damage);
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }
    }
}