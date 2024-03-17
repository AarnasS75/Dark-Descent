using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items DB", menuName = "Databases/Items")]
public class ItemsDatabase : ScriptableObject
{
    [Header("WEAPONS")]
    [SerializeField] private List<Weapon> _weapons;

    public Weapon GetRandomWeapon()
    {
        return _weapons[Random.Range(0, _weapons.Count)];
    }
}
