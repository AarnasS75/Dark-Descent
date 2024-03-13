using Characters.EnemyControls;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies DB", menuName = "Databases/Enemies")]
public class EnemyDatabase : ScriptableObject
{
    [SerializeField] private List<Enemy> _enemies;

    public Enemy GetRandom()
    {
        return _enemies[Random.Range(0, _enemies.Count)];
    }
}
