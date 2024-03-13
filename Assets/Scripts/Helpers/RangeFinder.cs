using System;
using System.Collections.Generic;
using System.Linq;
using Tiles;
using UnityEngine;

namespace Helpers.RangeFinding
{
    public static class RangeFinder
    {
        private static Dictionary<Vector2Int, OverlayTile> _map;

        public static void UpdateWithNewMap(Dictionary<Vector2Int, OverlayTile> map)
        {
            _map = map;
        }

        public static List<OverlayTile> GetTilesInRange(Vector2Int location, int range)
        {
            var startingTile = _map[location];
            var inRangeTiles = new List<OverlayTile>();
            int stepCount = 0;

            inRangeTiles.Add(startingTile);

            //Should contain the surroundingTiles of the previous step. 
            var tilesForPreviousStep = new List<OverlayTile>();
            tilesForPreviousStep.Add(startingTile);
            while (stepCount < range)
            {
                var surroundingTiles = new List<OverlayTile>();

                foreach (var item in tilesForPreviousStep)
                {
                    surroundingTiles.AddRange(GetSurroundingTiles(item.GetGridLocation2D()));
                }

                inRangeTiles.AddRange(surroundingTiles);
                tilesForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            }

            return inRangeTiles.Distinct().ToList();
        }

        private static List<OverlayTile> GetSurroundingTiles(Vector2Int originTile)
        {
            var surroundingTiles = new List<OverlayTile>();

            Vector2Int TileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
            if (_map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(_map[TileToCheck].transform.position.z - _map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(_map[TileToCheck]);
            }

            TileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
            if (_map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(_map[TileToCheck].transform.position.z - _map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(_map[TileToCheck]);
            }

            TileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
            if (_map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(_map[TileToCheck].transform.position.z - _map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(_map[TileToCheck]);
            }

            TileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
            if (_map.ContainsKey(TileToCheck))
            {
                if (Mathf.Abs(_map[TileToCheck].transform.position.z - _map[originTile].transform.position.z) <= 1)
                    surroundingTiles.Add(_map[TileToCheck]);
            }

            return surroundingTiles;
        }

        public static void ShowTilesInRange(Vector2Int standingTilePosition, int range)
        {
            var rangeFinderTiles = GetTilesInRange(standingTilePosition, range);
            foreach (var item in rangeFinderTiles)
            {
                item.Show();
            }
        }

        public static void HideRangeTiles()
        {
            foreach (var item in _map)
            {
                item.Value.Hide();
            }
        }

        public static void MarkEnemiesInRangeTiles(Vector2Int standingTilePosition, int attackRange)
        {
            var rangeFinderTiles = GetTilesInRange(standingTilePosition, attackRange).Where(x => !x.IsAvailable);
            foreach (var item in rangeFinderTiles)
            {
                item.Mark();
            }
        }
    }
}
