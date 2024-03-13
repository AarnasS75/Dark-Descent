using Rooms;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rooms DB", menuName = "Databases/Rooms")]
public class RoomsDatabase : ScriptableObject
{
    [SerializeField] private List<Room> _rooms;

    public Room GetRandom()
    {
        return _rooms[Random.Range(0, _rooms.Count)];
    }
}
