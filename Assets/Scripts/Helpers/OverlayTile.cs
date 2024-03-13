using UnityEngine;

namespace Tiles
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class OverlayTile : MonoBehaviour
    {
        [HideInInspector] public int G;
        [HideInInspector] public int H;
        public int F { get { return G + H; } }

        [HideInInspector] public OverlayTile Previous;

        public bool IsAvailable => _standingCharacter == null;

        private ICharacter _standingCharacter;
        private Vector3Int _gridLocation;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Occupy(ICharacter character)
        {
            _standingCharacter = character;
        }

        public void ResetTile()
        {
            _standingCharacter = null;
        }

        public ICharacter GetStandingCharacter()
        {
            return _standingCharacter;
        }

        public Vector2Int GetGridLocation2D()
        {
            return (Vector2Int)_gridLocation;
        }

        public Vector3Int GetGridLocation()
        {
            return _gridLocation;
        }

        public void SetGridLocation(Vector3Int gridLocation)
        {
            _gridLocation = gridLocation;
        }

        public void Hide()
        {
            _spriteRenderer.color = new Color(1, 1, 1, 0);
        }

        public void Show()
        {
            _spriteRenderer.color = new Color(1, 1, 1, 1);
        }

        public void Mark()
        {
            _spriteRenderer.color = Color.red;
        }
    }
}
