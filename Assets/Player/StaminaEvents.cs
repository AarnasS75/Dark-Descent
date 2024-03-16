using System;

public class StaminaEvents
{
    public event Action<float> OnStaminaChanged;

    public void CallStaminaChangedEvent(float staminaPercentage)
    {
        OnStaminaChanged?.Invoke(staminaPercentage);
    }
}