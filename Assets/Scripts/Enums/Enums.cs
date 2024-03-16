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
        EndTurn,
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