using UnityEngine;

[System.Serializable]
public class CharacterMovementStats
{
    [SerializeField][Min(0)] private float _moveSpeed = 2.5f;
    [SerializeField][Min(0)] private int _moveDistance = 3;

    public float MoveSpeed => _moveSpeed;
    public int MoveDistance => _moveDistance;
}

public abstract class CharacterStatsBase : ScriptableObject
{
    [Header("BASE STATS")]
    [SerializeField][Min(0)] private int _health = 100;
    [SerializeField][Min(1)] private int _actionsPerTurnCount = 3;

    [Header("MOVEMENT STATS")]
    [SerializeField] private CharacterMovementStats _movementStats;

    [Header("STARTING WEAPON")]
    [SerializeField] private SwordStats _weapon;

    public int Health => _health;
    public int ActionsPerTurnCount => _actionsPerTurnCount;
    public CharacterMovementStats MovementStats => _movementStats;
}
