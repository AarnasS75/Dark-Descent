using System.Collections.Generic;
using System.Linq;
using Tiles;
using UnityEngine;

namespace Helpers.PathFinding
{
    public class PathFinder
    {
        private Dictionary<Vector2Int, OverlayTile> _map;
        private Dictionary<Vector2Int, OverlayTile> _searchableTiles;

        public PathFinder(Dictionary<Vector2Int, OverlayTile> newMap)
        {
            _map = newMap;
        }

        public void ShowAvailableTilesWithinMoveRange(OverlayTile centerTile, int moveRange)
        {
            var inRangeTiles = _map.Values
                .Where(tile => tile.IsAvailable && GetManhattenDistance(centerTile, tile) <= moveRange)
                .ToList();

            foreach (var tile in inRangeTiles)
            {
                var path = FindPath(centerTile, tile, inRangeTiles);

                if (path.Count > 0 && path.Count - 1 <= moveRange)
                {
                    tile.Show();
                }
            }
        }

        public List<OverlayTile> GetAvailableTilesWithinMoveRange(OverlayTile centerTile, int moveRange)
        {
            var availableTiles = new List<OverlayTile>();

            // Find all tiles within the move range
            var inRangeTiles = _map.Values
                .Where(tile => tile.IsAvailable && GetManhattenDistance(centerTile, tile) <= moveRange)
                .ToList();

            // Iterate through each tile in range and find paths to it
            foreach (var tile in inRangeTiles)
            {
                // Find a path from the centerTile to the current tile within the move range
                var path = FindPath(centerTile, tile, inRangeTiles);

                // If a valid path exists within the move range, add the tile to the list
                if (path.Count > 0 && path.Count - 1 <= moveRange)
                {
                    availableTiles.Add(tile);
                }
            }

            return availableTiles;
        }

        public void HideAvailableTilesWithinMoveRange(OverlayTile centerTile, int moveRange)
        {
            // Get available tiles within move range
            var availableTiles = GetAvailableTilesWithinMoveRange(centerTile, moveRange);

            // Hide each available tile
            foreach (var tile in availableTiles)
            {
                tile.Hide();
            }
        }

        public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> inRangeTiles)
        {
            _searchableTiles = new Dictionary<Vector2Int, OverlayTile>();

            var openList = new List<OverlayTile>();
            var closedList = new HashSet<OverlayTile>();

            if (inRangeTiles.Count > 0)
            {
                foreach (var item in inRangeTiles)
                {
                    if (item.IsAvailable) // Check if the tile is available
                        _searchableTiles.Add(item.GetPosition2D(), _map[item.GetPosition2D()]);
                }
            }
            else
            {
                foreach (var tile in _map.Values)
                {
                    if (tile.IsAvailable)
                        _searchableTiles.Add(tile.GetPosition2D(), tile);
                }

                if (!_searchableTiles.ContainsKey(start.GetPosition2D()))
                    _searchableTiles.Add(start.GetPosition2D(), start);

                if (!_searchableTiles.ContainsKey(end.GetPosition2D()))
                    _searchableTiles.Add(end.GetPosition2D(), end);
            }

            openList.Add(start);

            while (openList.Count > 0)
            {
                var currentOverlayTile = openList.OrderBy(x => x.F).First();

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

        private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
        {
            var finishedList = new List<OverlayTile>();
            var currentTile = end;

            while (currentTile != start)
            {
                finishedList.Add(currentTile);
                currentTile = currentTile.Previous;
            }

            finishedList.Reverse();

            return finishedList;
        }

        public int GetManhattenDistance(OverlayTile start, OverlayTile tile)
        {
            return Mathf.Abs(start.GetGridLocation().x - tile.GetGridLocation().x) + Mathf.Abs(start.GetGridLocation().y - tile.GetGridLocation().y);
        }

        private List<OverlayTile> GetNeightbourOverlayTiles(OverlayTile currentOverlayTile)
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