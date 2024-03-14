using Characters.CharacterControls.Player;
using Characters.EnemyControls;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Rooms
{
    [RequireComponent(typeof(RoomController))]
    [DisallowMultipleComponent]
    public class Room : MonoBehaviour
    {
        private RoomController _controller;

        private void Awake()
        {
            _controller = GetComponent<RoomController>();
        }

        public void Initialize(IPlayer player, Dictionary<Vector2Int, OverlayTile> roomMap, List<Enemy> enemies)
        {
            _controller.Initialize(player, roomMap, enemies);
        }

        public Tilemap GetRoomGroundMap()
        {
            return _controller.GetRoomGroundMap();
        }

        public Vector2Int? GetExitPosition()
        {
            return _controller.GetExitPosition();
        }

        public Vector2Int? GetEntrancePosition()
        {
            return _controller.GetEntrancePosition();
        }

        public List<IEnemy> GetEnemies()
        {
            return _controller.GetEnemies();
        }

        public int GetEnemiesToSpawnCount()
        {
            return _controller.GetEnemiesToSpawnCont();
        }
    }
}