using Characters.Actions;
using Constants;
using Tiles;

public interface IPlayer : ICharacter
{
    CombatAction SelectAction(CombatActionType actionType);
    void TakeAction(OverlayTile actionTile);
}
