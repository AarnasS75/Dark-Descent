using Characters.CharacterControls.AttackEvents;
using Characters.CharacterControls.HealthEvents;
using Characters.CharacterControls.MovementEvents;
using Characters.CharacterControls.Player;
using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private IPlayer _player;
    private List<IEnemy> _enemies;
    private int _currentEnemyIndex;

    public event Action<ICharacter> OnCharacterTurn;

    public void Initialize(IPlayer player)
    {
        _player = player;
        _player.AttackEvents.OnAttacked += Character_OnAttacked;
        _player.MovementEvents.OnCharacterStopped += Character_OnStopped;
        _player.HealthEvents.OnDied += Character_OnDied;
    }

    public void Launch(List<IEnemy> enemies)
    {
        _enemies = enemies;
        _currentEnemyIndex = 0;

        foreach (var enemy in enemies)
        {
            enemy.MovementEvents.OnCharacterStopped += Character_OnStopped;
            enemy.AttackEvents.OnAttacked += Character_OnAttacked;
            enemy.HealthEvents.OnDied += Character_OnDied;
        }

        UpdateTurnState(TurnState.PlayerTurn);
    }

    public void EndPlayerTurn()
    {
        UpdateTurnState(TurnState.EnemyTurn);
    }

    private void UpdateTurnState(TurnState turnState)
    {
        switch (turnState)
        {
            case TurnState.PlayerTurn:
                _player.RefreshActions();
                OnCharacterTurn?.Invoke(_player);
                break;

            case TurnState.EnemyTurn:

                if (_enemies.Count > 0 && _enemies[_currentEnemyIndex].CreateActionScenario(_player))
                {
                    print(_enemies[_currentEnemyIndex]);
                    OnCharacterTurn?.Invoke(_enemies[_currentEnemyIndex]);
                }
                else
                {
                    goto case TurnState.PlayerTurn;
                }

                break;

            case TurnState.EnvironmentTurn:
                // TODO: If level has some kind of traps, they can be activated during this turn
                break;
        }
    }

    // This is a bit sussy, maybe make a generic action for Move and Attack actions
    private void Character_OnStopped(CharacterMovementEvents events, CharacterStoppedEventArgs characterStoppedEventArgs)
    {
        if(characterStoppedEventArgs.Character is IEnemy enemy)
        {
            StartCoroutine(GetNextEnemyTurn(enemy));
        }

        if (_enemies.Count == 0)
        {
            UpdateTurnState(TurnState.PlayerTurn);
        }
    }

    // This is a bit sussy, maybe make a generic action for Move and Attack actions
    private void Character_OnAttacked(CharacterAttackEvents events, CharacterAttackedEventArgs args)
    {
        if (args.Attacker is IEnemy enemy)
        {
            StartCoroutine(GetNextEnemyTurn(enemy));
        }
    }

    private void Character_OnDied(CharacterHealthEvents events, CharacterDiedEventArgs args)
    {
        if(args.Character is IEnemy enemy)
        {
            _enemies.Remove(enemy);
        }

        if (_enemies.Count == 0)
        {
            UpdateTurnState(TurnState.PlayerTurn);
        }
    }

    private IEnumerator GetNextEnemyTurn(IEnemy enemy)
    {
        yield return new WaitForSeconds(1.3f);

        if (!enemy.CreateActionScenario(_player))
        {
            _currentEnemyIndex++;
            if (_currentEnemyIndex >= _enemies.Count)
            {
                _currentEnemyIndex = 0;
                UpdateTurnState(TurnState.PlayerTurn);
            }
            else
            {
                UpdateTurnState(TurnState.EnemyTurn);
            }
        }
    }
}
