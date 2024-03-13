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

        private float _moveSpeed;
        private int _moveRage;

        private CharacterMovementEvents _movementEvents;

        public void Initialize(ICharacter character, CharacterMovementStats movementStats, CharacterMovementEvents movementEvents)
        {
            _character = character;
            _movementEvents = movementEvents;
            _moveSpeed = movementStats.MoveSpeed;
            _moveRage = movementStats.MoveDistance;
        }

        public void Move(OverlayTile clickedTile)
        {
            HideRangeTiles();

            var tilesInRange = GetTilesInRange().Where(x => x.IsAvailable).ToList();

            if (tilesInRange.Contains(clickedTile))
            {
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
                var step = _moveSpeed * Time.deltaTime;

                while (Vector2.Distance(transform.position, targetPosition) > 0.00001f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
                    transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);
                    yield return null;
                }
                transform.position = new Vector3(targetPosition.x, targetPosition.y, zIndex);
                _character.SetPosition(_path[0]);
                _path.RemoveAt(0);
            }

            yield return new WaitForSeconds(1.5f);

            _character.UseActionPoint();
            _movementEvents.CallStoppedEvent(_character.GetStandingTile(), _character);
        }

        public List<OverlayTile> GetTilesInRange()
        {
            return RangeFinder.GetTilesInRange(_character.GetStandingTile().GetGridLocation2D(), _moveRage);
        }

        public void ShowTilesInRange()
        {
            RangeFinder.ShowTilesInRange(_character.GetStandingTile().GetGridLocation2D(), _moveRage);
        }

        public void HideRangeTiles()
        {
            RangeFinder.HideRangeTiles();
        }
    }
}