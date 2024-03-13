using Constants;
using Tiles;

public class ActionScenario
{
    public float ScenarioValue;
    public OverlayTile MoveToTile;
    public OverlayTile AttackTile;

    public ActionScenario(float scenarioValue, OverlayTile attackTile, OverlayTile moveToTile)
    {
        ScenarioValue = scenarioValue;
        MoveToTile = moveToTile;
        AttackTile = attackTile;
    }

    public ActionScenario()
    {
        ScenarioValue = -100000;
        MoveToTile = null;
        AttackTile = null;
    }
}