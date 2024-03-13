using Characters.CharacterControls.HealthEvents;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsControllerUI : MonoBehaviour
{
    [SerializeField] private Image _healthBar; 
    [SerializeField] private Image _xpBar; 
    [SerializeField] private Image _staminaBar;

    public void Initialize(IPlayer player)
    {
        player.HealthEvents.OnHealthChanged += HealthChangedEvent_OnHealthChanged;
    }

    private void HealthChangedEvent_OnHealthChanged(CharacterHealthEvents events, CharacterHealthEvents.HealthChangedEventArgs args)
    {
        _healthBar.fillAmount = args.DamagePercentage;
    }
}
