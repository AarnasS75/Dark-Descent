using Characters.EnemyControls;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    [SerializeField] private Tilemap _groundMap;
    [SerializeField] private Tilemap _entranceMap;
    [SerializeField] private Tilemap _exitMap;

    private List<IEnemy> _spawnedEnemies;

    private void Start()
    {
        // Hide the entrance and exit tiles
        // _entranceMap.color = new Color(1f, 1f, 1f, 0f);
        // _exitMap.color = new Color(1f, 1f, 1f, 0f);
    }

    public void Initialize(IPlayer player, Dictionary<Vector2Int, OverlayTile> roomMap, List<Enemy> enemies)
    {
        _spawnedEnemies = new();
        PositionPlayerAtEntrance(player, roomMap);
        PositionEnemies(enemies, roomMap);
    }

    public List<IEnemy> GetEnemies()
    {
        return _spawnedEnemies;
    }

    public Tilemap GetRoomGroundMap()
    {
        return _groundMap;
    }

    public Vector2Int? GetExitPosition()
    {
        foreach (var cell in _exitMap.cellBounds.allPositionsWithin)
        {
            var position = new Vector3Int(cell.x, cell.y, cell.z);
            if (_exitMap.HasTile(position))
            {
                return new Vector2Int(position.x, position.y);
            }
        }

        Debug.LogWarning("The Exit tile is not found on the exit map.");
        return null;
    }

    public Vector2Int? GetEntrancePosition()
    {
        foreach (var cell in _entranceMap.cellBounds.allPositionsWithin)
        {
            var position = new Vector3Int(cell.x, cell.y, cell.z);
            if (_entranceMap.HasTile(position))
            {
                return new Vector2Int(position.x, position.y);
            }
        }

        Debug.LogWarning("The Entrance tile is not found on the entrance map.");
        return null;
    }

    private void PositionEnemies(List<Enemy> enemies, Dictionary<Vector2Int, OverlayTile> roomMap)
    {
        var availablePositions = GetAvailablePositions(roomMap);

        foreach (var enemy in enemies)
        {
            int attempts = 0;
            while (attempts < availablePositions.Count)
            {
                int index = Random.Range(0, availablePositions.Count);
                var availablePosition = availablePositions[index];

                if (availablePosition != null && roomMap.TryGetValue(availablePosition, out var tile))
                {
                    var enemyToSpawn = Instantiate(enemy);
                    tile.PlaceCharacter(enemyToSpawn);

                    _spawnedEnemies.Add(enemyToSpawn);
                    break;
                }
                attempts++;
            }
        }
    }

    private List<Vector2Int> GetAvailablePositions(Dictionary<Vector2Int, OverlayTile> roomMap)
    {
        var availablePositions = new List<Vector2Int>();

        foreach (var entry in roomMap)
        {
            if (entry.Value.IsAvailable)
            {
                availablePositions.Add(entry.Key);
            }
        }

        return availablePositions;
    }

    private void PositionPlayerAtEntrance(IPlayer player, Dictionary<Vector2Int, OverlayTile> roomMap)
    {
        var entranceTilePosition = GetEntrancePosition();

        if (entranceTilePosition != null && roomMap.TryGetValue(entranceTilePosition.Value, out var tile))
        {
            tile.PlaceCharacter(player);
        }
    }
}
