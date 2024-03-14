using Characters.CharacterControls.AttackEvents;
using System;
using UnityEngine;

public class LevelSystem
{
    private int _level;
    private int _exp;
    private int _expLeftToLevelUp;

    public event Action<float> OnExperienceChanged;
    public event Action<int> OnLevelUp;

    public LevelSystem(IPlayer player)
    {
        player.AttackEvents.OnAttacked += AttackEvents_OnAttacked;

        _level = PlayerPrefs.GetInt(Settings.PLAYER_LEVEL, 1);
        _exp = PlayerPrefs.GetInt(Settings.PLAYER_EXP, 0);
        _expLeftToLevelUp = PlayerPrefs.GetInt(Settings.PLAYER_EXP_TO_LEVEL_UP, 100);
    }

    private void AddExperience(int expIncrease)
    {
        _exp += expIncrease;

        if (_exp >= _expLeftToLevelUp)
        {
            _level++;
            _exp -= _expLeftToLevelUp;
            _expLeftToLevelUp += 100;   // Move to scaling configuration

            PlayerPrefs.SetInt(Settings.PLAYER_LEVEL, _level);
            PlayerPrefs.SetInt(Settings.PLAYER_EXP_TO_LEVEL_UP, _expLeftToLevelUp);

            OnLevelUp?.Invoke(_level);
        }

        PlayerPrefs.SetInt(Settings.PLAYER_EXP, _exp);

        OnExperienceChanged?.Invoke((float)_exp / (float)_expLeftToLevelUp);
    }

    public int GetLevel()
    {
        return _level;
    }

    public int GetExp()
    {
        return _exp;
    }

    public int GetExpToNextLevel()
    {
        return _expLeftToLevelUp;
    }

    public float GetExpNormalized()
    {
        return (float)_exp / _expLeftToLevelUp;
    }

    private void AttackEvents_OnAttacked(CharacterAttackEvents arg1, CharacterAttackedEventArgs attackEventArgs)
    {
        if(attackEventArgs.Target is IEnemy enemy)
        {
            if(enemy.GetHealth() <= 0)
            {
                AddExperience(enemy.GetExp());
            }
        }
    }

}
