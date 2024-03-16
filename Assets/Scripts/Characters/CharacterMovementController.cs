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
        public bool IsMoving { get; private set; }

        private PathFinder _pathFinder;

        private void Awake()
        {
            IsMoving = false;
        }

        public void Initialize(ICharacter character, CharacterMovementEvents movementEvents, PathFinder pathFinder)
        {
            _character = character;
            _movementEvents = movementEvents;

            _pathFinder = pathFinder;
        }

        public void Move(OverlayTile moveToTile)
        {
            var tilesInRange = GetAvailableTilesInRange();

            if (_character.GetRemainingActionsCount() <= 0 || !tilesInRange.Contains(moveToTile) || IsMoving)
            {
                print($"Cant go to: {moveToTile} | Action count: {_character.GetRemainingActionsCount()} | Tile in range: {tilesInRange.Contains(moveToTile)} | IsMoving: {IsMoving}");
                return;
            }

            if (_character is IPlayer player && !PlayerStaminaTracker.Instance.UseStamina(10))
            {
                return;
            }

            IsMoving = true;
            HideTilesInRange();
            _character.GetStandingTile().Hide();
            moveToTile.ShowMoveTo();

            _path = GetPath(moveToTile, tilesInRange);

            StartCoroutine(MoveRoutine());
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

            _character.UseActionPoint();
            if (_character.GetRemainingActionsCount() <= 0)
            {
                HideTilesInRange();
            }
            else
            {
                ShowTilesInRange();
            }

            _movementEvents.CallStoppedEvent(_character.GetStandingTile(), _character);
            IsMoving = false;
        }

        private List<OverlayTile> GetPath(OverlayTile moveToTile, List<OverlayTile> tilesInRange)
        {
            return _pathFinder.FindPath(_character.GetStandingTile(), moveToTile, tilesInRange);
        }

        private List<OverlayTile> GetAvailableTilesInRange()
        {
            return _pathFinder.GetAvailableTilesWithinMoveRange(_character.GetStandingTile(), _character.GetMoveRange());
        }

        private void HideTilesInRange()
        {
            _pathFinder.HideAvailableTilesWithinMoveRange(_character.GetStandingTile(), _character.GetMoveRange());
        }

        private void ShowTilesInRange()
        {
            _pathFinder.ShowAvailableTilesWithinMoveRange(_character.GetStandingTile(), _character.GetMoveRange());
        }
    }
}