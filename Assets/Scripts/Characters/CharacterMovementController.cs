using Characters.CharacterControls.MovementEvents;
using Helpers.PathFinding;
using Helpers.RangeFinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tiles;
using UnityEngine;

namespace Characters.CharacterControls.Movement
{
    public class CharacterMovementController : MonoBehaviour
    {
        private ICharacter _character;

        private List<OverlayTile> _path;

        private CharacterMovementEvents _movementEvents;

        public void Initialize(ICharacter character, CharacterMovementEvents movementEvents)
        {
            _character = character;
            _movementEvents = movementEvents;
        }

        public void Move(OverlayTile clickedTile)
        {
            var tilesInRange = GetAvailableTilesInRange();

            if (tilesInRange.Contains(clickedTile))
            {
                RangeFinder.HideTiles();
                clickedTile.ShowMoveTo();

                _path = PathFinder.FindPath(_character.GetStandingTile(), clickedTile, tilesInRange);
                StartCoroutine(MoveRoutine());
            }
        }

        private IEnumerator MoveRoutine()
        {
            while (_path.Count > 0)
            {
                var targetPosition = _path[0].transform.position;
                var zIndex = targetPosition.z;
                var step = _character.GetMoveSpeed() * Time.deltaTime;

                while (Vector2.Distance(transform.position, targetPosition) > 0.00001f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
                    transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);
                    yield return null;
                }
                transform.position = new Vector3(targetPosition.x, targetPosition.y, zIndex);
                _path[0].PlaceCharacter(_character);
                _path.RemoveAt(0);
            }

            if(_character.GetRemainingActionsCount() <= 0)
            {
                RangeFinder.HideTiles();
            }
            else
            {
                RangeFinder.ShowTilesInRange(_character.GetStandingTile(), _character.GetMoveRange());
            }

            _movementEvents.CallStoppedEvent(_character.GetStandingTile(), _character);
        }

        private List<OverlayTile> GetAvailableTilesInRange()
        {
            return RangeFinder.GetTilesInRange(_character.GetStandingTile(), _character.GetMoveRange()).Where(x => x.IsAvailable).ToList();
        }
    }
}