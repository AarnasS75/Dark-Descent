using Characters.CharacterControls.Player;
using Characters.EnemyControls;
using Helpers.PathFinding;
using Helpers.RangeFinding;
using Rooms;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

namespace Managers.Rooms
{
    public class RoomsManager : MonoBehaviour
    {
        [Header("ROOMS DATABASE")]
        [SerializeField] private RoomsDatabase _roomsDb;

        [Header("ENEMIES DATABASE")]
        [SerializeField] private EnemyDatabase _enemiesDb;

        [Header("MISC")]
        [SerializeField] private OverlayTile _overlayTilePrefab;
        [SerializeField] private GameObject _overlayContainerObject;

        public Dictionary<Vector2Int, OverlayTile> _roomMap;

        private Room _currentRoom;
        private IPlayer _player;

        public void Initialize(IPlayer player)
        {
            _player = player;
        }

        /*private void Player_OnStopped(OverlayTile stoppedTile)
        {
            var exitTilePosition = _currentRoom.GetExitPosition();

            if (stoppedTile.GetGridLocation2D() == exitTilePosition.Value)
            {
                print("EXIT");
                DeleteCurrentRoom();
                LoadRoom();
            }
        }*/

        public Dictionary<Vector2Int, OverlayTile> GetMap()
        {
            return _roomMap;
        }

        public void HideOverviewTiles()
        {
            foreach (var item in _roomMap)
            {
                item.Value.Hide();
            }
        }

        public List<IEnemy> GetSpawnedEnemies()
        {
            return _currentRoom.GetEnemies();
        }

        public void LoadRoom()
        {
            _roomMap = new();

            _currentRoom = Instantiate(_roomsDb.GetRandom());
            var groundMap = _currentRoom.GetRoomGroundMap();

            var groundBounds = groundMap.cellBounds;

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
                                var cellWorldPosition = groundMap.GetCellCenterWorld(new Vector3Int(x, y, z));

                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                overlayTile.SetGridLocation(new Vector3Int(x, y, z));

                                _roomMap.Add(new Vector2Int(x, y), overlayTile);
                            }
                        }
                    }
                }
            }

            PathFinder.UpdateWithNewMap(_roomMap);
            RangeFinder.UpdateWithNewMap(_roomMap); 
            RangeFinder.HideTiles();

            _currentRoom.Initialize(_player, _roomMap, new List<Enemy> { _enemiesDb.GetRandom() });
        }

        private void DeleteCurrentRoom()
        {
            foreach (var item in _roomMap)
            {
                Destroy(item.Value.gameObject);
            }

            Destroy(_currentRoom.gameObject);
        }
    }
}
