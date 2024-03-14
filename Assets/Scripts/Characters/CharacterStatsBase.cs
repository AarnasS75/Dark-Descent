using Characters.Actions;
using Constants;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CharacterMovementStats
{
    [SerializeField][Min(0)] private float _moveSpeed = 2.5f;
    [SerializeField][Min(0)] private int _moveDistance = 3;

    public float MoveSpeed => _moveSpeed;
    public int MoveDistance => _moveDistance;
}

[System.Serializable]
public class CharacterAttackStats
{
    [SerializeField][Min(0)] private int _attackDamage = 3;
    [SerializeField][Min(0)] private int _attackRange = 3;

    public int AttackDamage => _attackDamage;
    public int AttackRange => _attackRange;
}

public abstract class CharacterStatsBase : ScriptableObject
{
    [Header("BASE STATS")]
    [SerializeField][Min(0)] private int _health = 100;
    [SerializeField][Min(1)] private int _actionsPerTurnCount = 3;

    [Header("MOVEMENT STATS")]
    [SerializeField] private CharacterMovementStats _movementStats;

    [Header("ATTACK STATS")]
    [SerializeField] private CharacterAttackStats _attackStats;

    public int Health => _health;
    public int ActionsPerTurnCount => _actionsPerTurnCount;
    public CharacterAttackStats AttackStats => _attackStats;
    public CharacterMovementStats MovementStats => _movementStats;
}
