using System;
using System.Collections;
using UnityEngine;

public class PlayerStaminaTracker : MonoBehaviour
{
    public static PlayerStaminaTracker Instance { get; private set; }

    [SerializeField] private int _maxStamina = 50;                  // Max available turns
    [SerializeField] private float _staminaRecoveryRate = 5f;       // How quickly the stamina gets recovered (in minutes)
    [SerializeField] private int _staminaRecoveryAmmount = 5;       // How much stamina gets recovered
    private WaitForSeconds _delay;
    public int _currentStamina;

    public int MaxStamina => _maxStamina;

    public event Action<float> OnStaminaChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        _currentStamina = PlayerPrefs.GetInt(Settings.PLAYER_STAMINA, _maxStamina);
        _delay = new WaitForSeconds(_staminaRecoveryRate * 60);
;
        StartCoroutine(RunRoutine());
    }

    public bool UseStamina(int staminaCost = 1)
    {
        if (_currentStamina - staminaCost < 0)
        {
            return false;
        }

        _currentStamina -= staminaCost;
        _currentStamina = Mathf.Max(0, _currentStamina);

        OnStaminaChanged?.Invoke((float)_currentStamina / _maxStamina);
        return true;
    }

    private IEnumerator RunRoutine()
    {
        while (true)
        {
            UpdateStamina();
            yield return _delay;
        }
    }

    private void UpdateStamina()
    {
        _currentStamina = Mathf.Clamp(_currentStamina + _staminaRecoveryAmmount, 0, _maxStamina);
        PlayerPrefs.SetInt(Settings.PLAYER_STAMINA, _currentStamina);

        OnStaminaChanged?.Invoke((float)_currentStamina / _maxStamina);
    }
}
