using Characters.CharacterControls.MovementEvents;
using Characters.EnemyControls;
using Rooms;
using System;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Managers.Rooms
{
    public class RoomsManager : MonoBehaviour
    {
        [Header("ROOMS DATABASE")]
        [SerializeField] private RoomsDatabase _roomsDb;

        [Header("ENEMY SPAWNER")]
        [SerializeField] private EnemySpawner _enemySpawner;

        [Header("MISC")]
        [SerializeField] private OverlayTile _overlayTilePrefab;
        [SerializeField] private GameObject _overlayContainerObject;

        public Dictionary<Vector2Int, OverlayTile> _roomMap;

        private Room _currentRoom;
        private IPlayer _player;

        public event Action<Room> OnRoomExit;

        public void Initialize(IPlayer player)
        {
            _roomMap = new();
            _player = player;
            _player.MovementEvents.OnCharacterStopped += MovementEvents_OnCharacterStopped;

            _enemySpawner.Initialize(player);
        }

        public Dictionary<Vector2Int, OverlayTile> GetMap()
        {
            return _roomMap;
        }

        public List<IEnemy> GetSpawnedEnemies()
        {
            return _currentRoom.GetEnemies();
        }

        public void LoadRoom()
        {
            DeleteCurrentRoom();
            _currentRoom = Instantiate(_roomsDb.GetRandom());

            var groundMap = _currentRoom.GetRoomGroundMap();
            var groundBounds = groundMap.cellBounds;
            var index = 0;

            for (int z = groundBounds.max.z; z >= groundBounds.min.z; z--)
            {
                for (int y = groundBounds.min.y; y < groundBounds.max.y; y++)
                {
                    for (int x = groundBounds.min.x; x < groundBounds.max.x; x++)
                    {
                        if (groundMap.HasTile(new Vector3Int(x, y, z)))
                        {
                            if (!_roomMap.ContainsKey(new Vector2Int(x, y)))
                            {
                                var overlayTile = Instantiate(_overlayTilePrefab, _overlayContainerObject.transform);
                                overlayTile.name = $"Tile_{index++}";
                                var cellWorldPosition = groundMap.GetCellCenterWorld(new Vector3Int(x, y, z));

                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                overlayTile.SetGridLocation(new Vector3Int(x, y, z));

                                _roomMap.Add(new Vector2Int(x, y), overlayTile);
                            }
                        }
                    }
                }
            }

            InitializeRoom();
        }

        private void MovementEvents_OnCharacterStopped(CharacterMovementEvents arg1, CharacterStoppedEventArgs arg2)
        {
            var exitTilePosition = _currentRoom.GetExitPosition();

            if (arg2.StandingTile.GetPosition2D() == exitTilePosition.Value)
            {
                print("Load next room");
                OnRoomExit?.Invoke(_currentRoom);
            }
        }

        private void DeleteCurrentRoom()
        {
            if (_currentRoom != null)
            {
                HideAnyShownTiles();
                _roomMap.Clear();
                _player.RefreshActions();
                Destroy(_currentRoom.gameObject);
            }
        }

        private void InitializeRoom()
        {
            var enemiesToSpawn = new List<Enemy>();
            var enemiesToSpawnCount = _currentRoom.GetEnemiesToSpawnCount();

            while (enemiesToSpawnCount > 0)
            {
                enemiesToSpawn.Add(_enemySpawner.SpawnRandom(_roomMap));
                enemiesToSpawnCount--;
            }

            _currentRoom.Initialize(_player, _roomMap, enemiesToSpawn);
        }

        private void HideAnyShownTiles()
        {
            foreach (var tile in _roomMap.Values)
            {
                tile.Hide();
            }
        }
    }
}
