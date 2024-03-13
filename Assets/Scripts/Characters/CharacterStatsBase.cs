using Characters.Actions;
using Constants;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CharacterMovementStats
{
    [Min(1)] public float MoveSpeed = 3;
    [Min(1)] public int MoveDistance = 3;
}

[System.Serializable]
public class CharacterAttackStats
{
    [Min(1)] public int AttackRange = 3;
    [Min(1)] public int MoveDistance = 3;
    [Min(0)] public int AttackDamage = 3;
}

public abstract class CharacterStatsBase : ScriptableObject
{
    [Header("BASE STATS")]
    [SerializeField][Min(0)] private int _health = 100;
    [SerializeField][Min(1)] private int _actionsPerTurnCount = 3;
    [SerializeField] private List<CombatAction> _characterActions;

    [Header("MOVEMENT STATS")]
    [SerializeField] private CharacterMovementStats _movementStats;

    [Header("ATTACK STATS")]
    [SerializeField] private CharacterAttackStats _attackStats;

    public int Health => _health;
    public int ActionsPerTurnCount => _actionsPerTurnCount;
    public CharacterAttackStats AttackStats => _attackStats;
    public CharacterMovementStats MovementStats => _movementStats;
    public List<CombatAction> CombatActions => _characterActions;

    public CombatAction? GetAction(CombatActionType actionType)
    {
        return CombatActions.FirstOrDefault(x => x.ActionType == actionType);
    }
}
