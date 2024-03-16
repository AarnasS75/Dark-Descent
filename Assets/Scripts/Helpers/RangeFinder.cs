using System;
using System.Collections.Generic;
using System.Linq;
using Tiles;
using UnityEngine;

namespace Helpers.RangeFinding
{
    public class RangeFinder
    {
        private Dictionary<Vector2Int, OverlayTile> _map;

        public RangeFinder(Dictionary<Vector2Int, OverlayTile> map)
        {
            _map = map;
        }

        public List<OverlayTile> GetTilesInRange(OverlayTile standingTile, int range)
        {
            var inRangeTiles = new HashSet<OverlayTile>();
            var tilesForPreviousStep = new HashSet<OverlayTile>();
            int stepCount = 0;

            inRangeTiles.Add(standingTile);
            tilesForPreviousStep.Add(standingTile);

            while (stepCount < range)
            {
                var surroundingTiles = new HashSet<OverlayTile>();

                foreach (var item in tilesForPreviousStep)
                {
                    surroundingTiles.UnionWith(GetSurroundingTiles(item.GetPosition2D()));
                }

                inRangeTiles.UnionWith(surroundingTiles);
                tilesForPreviousStep = new HashSet<OverlayTile>(surroundingTiles);
                stepCount++;
            }

            return inRangeTiles.ToList();
        }

        private List<OverlayTile> GetSurroundingTiles(Vector2Int originTile)
        {
            var surroundingTiles = new List<OverlayTile>();

            if (_map.TryGetValue(originTile, out OverlayTile originOverlayTile))
            {
                float originZ = originOverlayTile.transform.position.z;

                AddIfInRange(originTile.x + 1, originTile.y, originZ, surroundingTiles);
                AddIfInRange(originTile.x - 1, originTile.y, originZ, surroundingTiles);
                AddIfInRange(originTile.x, originTile.y + 1, originZ, surroundingTiles);
                AddIfInRange(originTile.x, originTile.y - 1, originZ, surroundingTiles);
            }

            return surroundingTiles;
        }

        private void AddIfInRange(int x, int y, float originZ, List<OverlayTile> surroundingTiles)
        {
            var TileToCheck = new Vector2Int(x, y);
            if (_map.TryGetValue(TileToCheck, out OverlayTile tile))
            {
                if (Mathf.Abs(tile.transform.position.z - originZ) <= 1)
                {
                    surroundingTiles.Add(tile);
                }
            }
        }


        public void ShowTilesInRange(OverlayTile standingTile, int range)
        {
            HideTiles();
            var rangeFinderTiles = GetTilesInRange(standingTile, range).Where(x => x.IsAvailable);
            foreach (var item in rangeFinderTiles)
            {
                item.Show();
            }
        }

        public void HideTiles()
        {
            foreach (var item in _map)
            {
                item.Value.Hide();
            }
        }

        public void ShowTilesInAttackRange(OverlayTile standingTile, int attackRange)
        {
            HideTiles();
            var rangeFinderTiles = GetTilesInRange(standingTile, attackRange).Where(x => !x.IsAvailable && x.GetPosition2D() != standingTile.GetPosition2D());

            foreach (var item in rangeFinderTiles)
            {
                item.Mark();
            }
        }
    }
}
