using Characters.CharacterControls.AttackEvents;
using Characters.CharacterControls.HealthEvents;
using Characters.CharacterControls.MovementEvents;
using Tiles;

public interface ICharacter
{
    CharacterHealthEvents HealthEvents { get; }
    CharacterMovementEvents MovementEvents { get; }
    CharacterAttackEvents AttackEvents { get; }

    int GetRemainingActionsCount();
    int GetActionsPerTurnCount();

    int GetAttackDamage();
    int GetAttackRange();

    float GetMoveSpeed();
    int GetMoveRange();

    int GetHealth();
    int GetMaxHealth();
    void TakeDamage(int damageAmmount);

    public OverlayTile GetStandingTile();
}
