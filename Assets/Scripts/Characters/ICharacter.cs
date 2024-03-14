using Characters.CharacterControls.AttackEvents;
using Characters.CharacterControls.HealthEvents;
using Characters.CharacterControls.MovementEvents;
using Constants;
using Tiles;

public interface ICharacter
{
    CharacterHealthEvents HealthEvents { get; }
    CharacterMovementEvents MovementEvents { get; }
    CharacterAttackEvents AttackEvents { get; }

    // Action
    void SelectAction(CombatActionType actionType);
    void TakeAction(OverlayTile actionTile);
    int GetRemainingActionsCount();
    int GetActionsPerTurnCount();

    // Attack
    int GetAttackDamage();
    int GetAttackRange();

    // Movement
    float GetMoveSpeed();
    int GetMoveRange();

    // Health
    int GetHealth();
    int GetMaxHealth();
    void TakeDamage(int damageAmmount);

    // Position
    public OverlayTile GetStandingTile();
}
