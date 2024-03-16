using Constants;
using Tiles;

public class ActionScenario
{
    public float Value;
    public OverlayTile MoveToTile;
    public OverlayTile AttackTile;

    public ActionScenario(float scenarioValue, OverlayTile attackTile, OverlayTile moveToTile)
    {
        Value = scenarioValue;
        MoveToTile = moveToTile;
        AttackTile = attackTile;
    }

    public ActionScenario()
    {
        Value = -100000;
        MoveToTile = null;
        AttackTile = null;
    }
}