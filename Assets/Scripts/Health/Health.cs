using Characters.CharacterControls.HealthEvents;
using System;
using UnityEngine;

namespace Characters.HealthControls
{
    public class Health : MonoBehaviour
    {
        private int _maxHealth;
        public int _currentHealth;

        private CharacterHealthEvents _healthEvents;
        private ICharacter _character;

        public void Initialize(ICharacter character, int health, CharacterHealthEvents healthEvents)
        {
            _character = character;
            _maxHealth = character is IPlayer ? PlayerPrefs.GetInt(Settings.PLAYER_MAX_HEALTH, health) : health;
            _currentHealth = character is IPlayer ? PlayerPrefs.GetInt(Settings.PLAYER_HEALTH, health) : health;
            _healthEvents = healthEvents;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if(_character is IPlayer player)
            {
                PlayerPrefs.SetInt(Settings.PLAYER_HEALTH, _currentHealth);
            }

            _healthEvents.CallHealthChangedEvent(_character, (float)_currentHealth / (float)_maxHealth, damage);

            if(_currentHealth <= 0)
            {
                _healthEvents.CallCharacterDiedEvent(_character);
                Destroy(gameObject);
            }
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }

        public int GetMaxHealth()
        {
            return _maxHealth;
        }
    }
}