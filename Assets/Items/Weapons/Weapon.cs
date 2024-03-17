using UnityEngine;

public abstract class Weapon : ItemBase<WeaponStatsBase>
{
    private int _level = 1;

    public int GetAttackDamage()
    {
        return _stats.Damage;
    }

    public int GetAttackRange()
    {
        return _stats.Range;
    }

    public string GetName()
    {
        return _stats.Name;
    }

    public Sprite GetSprite()
    {
        return _stats.Sprite;
    }

    public int GetLevel()
    {
        return _level;
    }
}
