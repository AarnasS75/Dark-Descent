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
    void TakeAction(CombatActionType actionType, OverlayTile actionTile);
    void UseActionPoint();
    int GetRemainingActionsCount();
    int GetActionsPerTurnCount();
    CombatActionType GetSelectedAction();

    // Weapon
    Weapon GetWeapon();

    // Movement
    float GetMoveSpeed();
    int GetMoveRange();

    // Health
    int GetHealth();
    int GetMaxHealth();
    void TakeDamage(int damageAmmount);

    // Position
    public void Place(OverlayTile placeTile);
    public OverlayTile GetStandingTile();
}
