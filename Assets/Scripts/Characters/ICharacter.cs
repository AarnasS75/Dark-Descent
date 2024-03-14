using Characters.CharacterControls.AttackEvents;
using Characters.CharacterControls.HealthEvents;
using Characters.CharacterControls.MovementEvents;
using Tiles;

public interface ICharacter
{
    CharacterStatsBase Stats { get; }
    CharacterHealthEvents HealthEvents { get; }
    CharacterMovementEvents MovementEvents { get; }
    CharacterAttackEvents AttackEvents { get; }

    void UseActionPoint();
    void ResetSelectedAction();
    int GetRemainingActionsCount();
    int GetActionsPerTurnCount();

    int GetHealth();
    void TakeDamage(int damageAmmount);

    public OverlayTile GetStandingTile();
}
