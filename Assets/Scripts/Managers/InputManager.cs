using Tiles;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using Constants;

namespace Managers.InpuHandling
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private float _clickBuffer = 0.3f; //Delay in seconds to prevent spam clicking
        private float _clickDelayTime;

        [SerializeField] private GameObject _cursorPrefab;
        private GameObject _cursor;

        private IPlayer _player;

        private void Start()
        {
            _cursor = Instantiate(_cursorPrefab);
        }

        private void LateUpdate()
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                _cursor.SetActive(true);
                _cursor.transform.position = hit.Value.collider.transform.position;

                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && _clickDelayTime > _clickBuffer)
                {
                    if (hit.Value.collider.TryGetComponent<OverlayTile>(out var tile))
                    {
                        switch (_player.GetSelectedAction())
                        {
                            case CombatActionType.EndTurn:
                                _player.TakeAction(CombatActionType.EndTurn, tile);
                                break;
                            case CombatActionType.Move:
                                _player.TakeAction(CombatActionType.Move, tile);
                                break;
                            case CombatActionType.Attack:
                                _player.TakeAction(CombatActionType.Attack, tile);
                                break;
                        }
                    }
                    _clickDelayTime = 0;
                }
                else
                {
                    _clickDelayTime += Time.deltaTime;
                }
            }
            else
            {
                _cursor.SetActive(false);
                _clickDelayTime = 0;
            }
        }

        public void Initialize(IPlayer player)
        {
            _player = player;
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
