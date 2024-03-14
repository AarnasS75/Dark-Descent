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

        public static List<OverlayTile> GetTilesInRange(OverlayTile standingTile, int range)
        {
            var inRangeTiles = new List<OverlayTile>();
            int stepCount = 0;

            inRangeTiles.Add(standingTile);

            //Should contain the surroundingTiles of the previous step. 
            var tilesForPreviousStep = new List<OverlayTile>();
            tilesForPreviousStep.Add(standingTile);
            while (stepCount < range)
            {
                var surroundingTiles = new List<OverlayTile>();

                foreach (var item in tilesForPreviousStep)
                {
                    surroundingTiles.AddRange(GetSurroundingTiles(item.GetPosition2D()));
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

        public static void ShowTilesInRange(OverlayTile standingTile, int range)
        {
            var rangeFinderTiles = GetTilesInRange(standingTile, range);
            foreach (var item in rangeFinderTiles)
            {
                item.Show();
            }
        }

        public static void HideTiles()
        {
            foreach (var item in _map)
            {
                item.Value.Hide();
            }
        }

        public static void MarkEnemiesInRangeTiles(OverlayTile standingTile, int attackRange)
        {
            var rangeFinderTiles = GetTilesInRange(standingTile, attackRange).Where(x => !x.IsAvailable);
            foreach (var item in rangeFinderTiles)
            {
                item.Mark();
            }
        }
    }
}
