using System.Collections.Generic;
using System.Linq;
using Tiles;
using UnityEngine;

namespace Helpers.PathFinding
{
    public static class PathFinder
    {
        private static Dictionary<Vector2Int, OverlayTile> _map;
        private static Dictionary<Vector2Int, OverlayTile> _searchableTiles;

        public static void UpdateWithNewMap(Dictionary<Vector2Int, OverlayTile> newMap)
        {
            _map = newMap;
        }

        public static List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> inRangeTiles)
        {
            _searchableTiles = new Dictionary<Vector2Int, OverlayTile>();

            List<OverlayTile> openList = new List<OverlayTile>();
            HashSet<OverlayTile> closedList = new HashSet<OverlayTile>();

            if (inRangeTiles.Count > 0)
            {
                foreach (var item in inRangeTiles)
                {
                    _searchableTiles.Add(item.GetGridLocation2D(), _map[item.GetGridLocation2D()]);
                }
            }
            else
            {
                _searchableTiles = _map;
            }

            openList.Add(start);

            while (openList.Count > 0)
            {
                OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();

                openList.Remove(currentOverlayTile);
                closedList.Add(currentOverlayTile);

                if (currentOverlayTile == end)
                {
                    return GetFinishedList(start, end);
                }

                foreach (var tile in GetNeightbourOverlayTiles(currentOverlayTile))
                {
                    if (closedList.Contains(tile) || Mathf.Abs(currentOverlayTile.transform.position.z - tile.transform.position.z) > 1)
                    {
                        continue;
                    }

                    tile.G = GetManhattenDistance(start, tile);
                    tile.H = GetManhattenDistance(end, tile);

                    tile.Previous = currentOverlayTile;


                    if (!openList.Contains(tile))
                    {
                        openList.Add(tile);
                    }
                }
            }

            return new List<OverlayTile>();
        }

        private static List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
        {
            List<OverlayTile> finishedList = new List<OverlayTile>();
            OverlayTile currentTile = end;

            while (currentTile != start)
            {
                finishedList.Add(currentTile);
                currentTile = currentTile.Previous;
            }

            finishedList.Reverse();

            return finishedList;
        }

        public static int GetManhattenDistance(OverlayTile start, OverlayTile tile)
        {
            return Mathf.Abs(start.GetGridLocation().x - tile.GetGridLocation().x) + Mathf.Abs(start.GetGridLocation().y - tile.GetGridLocation().y);
        }

        private static List<OverlayTile> GetNeightbourOverlayTiles(OverlayTile currentOverlayTile)
        {
            var neighbours = new List<OverlayTile>();

            //right
            Vector2Int locationToCheck = new Vector2Int(
                currentOverlayTile.GetGridLocation().x + 1,
                currentOverlayTile.GetGridLocation().y
            );

            if (_searchableTiles.ContainsKey(locationToCheck))
            {
                neighbours.Add(_searchableTiles[locationToCheck]);
            }

            //left
            locationToCheck = new Vector2Int(
                currentOverlayTile.GetGridLocation().x - 1,
                currentOverlayTile.GetGridLocation().y
            );

            if (_searchableTiles.ContainsKey(locationToCheck))
            {
                neighbours.Add(_searchableTiles[locationToCheck]);
            }

            //top
            locationToCheck = new Vector2Int(
                currentOverlayTile.GetGridLocation().x,
                currentOverlayTile.GetGridLocation().y + 1
            );

            if (_searchableTiles.ContainsKey(locationToCheck))
            {
                neighbours.Add(_searchableTiles[locationToCheck]);
            }

            //bottom
            locationToCheck = new Vector2Int(
                currentOverlayTile.GetGridLocation().x,
                currentOverlayTile.GetGridLocation().y - 1
            );

            if (_searchableTiles.ContainsKey(locationToCheck))
            {
                neighbours.Add(_searchableTiles[locationToCheck]);
            }

            return neighbours;
        }

      
    }
}