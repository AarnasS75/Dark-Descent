using Constants;
using Tiles;

public interface IPlayer : ICharacter
{
    LevelSystem LevelSystem { get; }

    void SelectAction(CombatActionType actionType);
    void TakeAction(OverlayTile actionTile);
}
