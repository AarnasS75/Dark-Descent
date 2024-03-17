using UnityEngine;

public class WeaponStatsBase : ItemStatsBase
{
    [Header("WEAPON CONFIGURATION")]
    [SerializeField] private int _damage;
    [SerializeField] private int _range;

    public int Damage => _damage;
    public int Range => _range;
}
