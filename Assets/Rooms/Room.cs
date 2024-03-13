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
        public RoomController Controller { get; private set; }

        private void Awake()
        {
            Controller = GetComponent<RoomController>();
        }

        public void Initialize(IPlayer player, Dictionary<Vector2Int, OverlayTile> roomMap, List<Enemy> enemies)
        {
            Controller.Initialize(player, roomMap, enemies);
        }

        public Tilemap GetRoomGroundMap()
        {
            return Controller.GetRoomGroundMap();
        }

        public Vector2Int? GetExitPosition()
        {
            return Controller.GetExitPosition();
        }

        public Vector2Int? GetEntrancePosition()
        {
            return Controller.GetEntrancePosition();
        }

        public List<IEnemy> GetEnemies()
        {
            return Controller.GetEnemies();
        }
    }
}