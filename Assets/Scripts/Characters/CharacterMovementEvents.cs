using System;
using Tiles;

namespace Characters.CharacterControls.MovementEvents
{
    public class CharacterMovementEvents
    {
        public event Action<CharacterMovementEvents, CharacterStoppedEventArgs> OnCharacterStopped;

        public void CallStoppedEvent(OverlayTile standingTile, ICharacter character)
        {
            OnCharacterStopped?.Invoke(this, new CharacterStoppedEventArgs
            {
                StandingTile = standingTile,
                Character = character
            });
        }
    }

    public class CharacterStoppedEventArgs : EventArgs
    {
        public OverlayTile StandingTile;
        public ICharacter Character;
    }
}