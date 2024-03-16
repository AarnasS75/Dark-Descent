using Characters.CharacterControls.HealthEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsControllerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText; 
    [SerializeField] private Image _healthBar; 
    [SerializeField] private Image _xpBar; 
    [SerializeField] private Image _staminaBar;

    private void Start()
    {
        _levelText.text = PlayerPrefs.GetInt(Settings.PLAYER_LEVEL, 1).ToString();
        _xpBar.fillAmount = (float)PlayerPrefs.GetInt(Settings.PLAYER_EXP, 0) / PlayerPrefs.GetInt(Settings.PLAYER_EXP_TO_LEVEL_UP, 100);
        _healthBar.fillAmount = (float)PlayerPrefs.GetInt(Settings.PLAYER_HEALTH, 100) / PlayerPrefs.GetInt(Settings.PLAYER_MAX_HEALTH, 100);
        _staminaBar.fillAmount = (float)PlayerPrefs.GetInt(Settings.PLAYER_STAMINA, 100) / PlayerPrefs.GetInt(Settings.PLAYER_MAX_STAMINA, 100);
    }

    public void Initialize(IPlayer player)
    {
        player.HealthEvents.OnHealthChanged += HealthChangedEvent_OnHealthChanged;
        player.LevelSystem.OnLevelUp += LevelSystem_OnLevelUp;
        player.LevelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        PlayerStaminaTracker.Instance.OnStaminaChanged += StaminaEvents_OnStaminaChanged;
    }

    private void StaminaEvents_OnStaminaChanged(float staminaPercentage)
    {
        _staminaBar.fillAmount = staminaPercentage;
    }

    private void LevelSystem_OnExperienceChanged(float expPercentage)
    {
        _xpBar.fillAmount = expPercentage;
    }

    private void LevelSystem_OnLevelUp(int level)
    {
        _levelText.text = level.ToString();
    }

    private void HealthChangedEvent_OnHealthChanged(CharacterHealthEvents events, HealthChangedEventArgs args)
    {
        _healthBar.fillAmount = args.DamagePercentage;
    }
}
