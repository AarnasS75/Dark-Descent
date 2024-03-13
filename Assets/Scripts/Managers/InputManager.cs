using Tiles;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using Characters.CharacterControls.Player;

namespace Managers.InpuHandling
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private GameObject _cursorPrefab;
        private GameObject _cursor;

        private Player _player;

        public void Initialize(Player player)
        {
            _player = player;
        }

        private void Start()
        {
            _cursor = Instantiate(_cursorPrefab);
        }

        void LateUpdate()
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                _cursor.SetActive(true);
                _cursor.transform.position = hit.Value.collider.transform.position;

                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    if( hit.Value.collider.TryGetComponent<OverlayTile>(out var tile))
                    {
                        _player.TakeAction(tile);
                    }
                }
            }
            else
            {
                _cursor.SetActive(false);
            }
        }

        private static RaycastHit2D? GetFocusedOnTile()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }
    }
}
