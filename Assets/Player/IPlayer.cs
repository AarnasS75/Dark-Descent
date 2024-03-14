using Characters.Actions;
using Constants;
using Tiles;

public interface IPlayer : ICharacter
{
    void SelectAction(CombatActionType actionType);
    void TakeAction(OverlayTile actionTile);
}
