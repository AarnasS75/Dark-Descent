namespace Constants
{
    public enum GameState
    {
        GameStarted,
        PlayingLevel,
        GameEnded,
        GamePaused,
    }

    public enum CombatActionType
    {
        None,
        Move,
        Attack
    }

    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn,
        EnvironmentTurn
    }
}