using Characters.EnemyControls;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyDatabase _enemyDb;

    private IPlayer _player;

    public void Initialize(IPlayer player)
    {
        _player = player;
    }

    public Enemy SpawnRandom(Dictionary<Vector2Int, OverlayTile> roomMap)
    {
        var enemyToSpawn = Instantiate(_enemyDb.GetRandom(), transform);
        enemyToSpawn.Initialize(roomMap);

        return enemyToSpawn;
    }
}
