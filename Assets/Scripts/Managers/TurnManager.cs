using Characters.CharacterControls.AttackEvents;
using Characters.CharacterControls.HealthEvents;
using Characters.CharacterControls.MovementEvents;
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
    private TurnState _turnState;

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

    public void EndTurn()
    {
        UpdateTurnState(TurnState.EnemyTurn);
    }

    private void UpdateTurnState(TurnState turnState)
    {
        _turnState = turnState;

        switch (turnState)
        {
            case TurnState.PlayerTurn:
                PlayerTurn();
                break;

            case TurnState.EnemyTurn:
                EnemyTurn();
                break;

            case TurnState.EnvironmentTurn:
                // TODO: If level has some kind of traps, they can be activated during this turn
                break;
        }
    }

    private void Character_OnStopped(CharacterMovementEvents events, CharacterStoppedEventArgs characterStoppedEventArgs)
    {
        if(characterStoppedEventArgs.Character is IEnemy enemy)
        {
            StartCoroutine(GetNextEnemyTurn(enemy));
        }
    }

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
    }

    private IEnumerator GetNextEnemyTurn(IEnemy enemy)
    {
        yield return new WaitForSeconds(1.3f);

        if (enemy.GetRemainingActionsCount() > 0)
        {
            enemy.TakeAction(_player);
        }
        else
        {
            _currentEnemyIndex++;
            if (_currentEnemyIndex >= _enemies.Count)
            {
                _currentEnemyIndex = 0;
                UpdateTurnState(TurnState.PlayerTurn);
            }
            else
            {
                _enemies[_currentEnemyIndex].TakeAction(_player);
            }
        }
    }

    private void PlayerTurn()
    {
        OnCharacterTurn?.Invoke(_player);
    }

    private void EnemyTurn()
    {
        if(_enemies.Count > 0)
        {
            // Start the turn for the first enemy
            _enemies[_currentEnemyIndex].TakeAction(_player);
            OnCharacterTurn?.Invoke(_enemies[_currentEnemyIndex]);
        }
        else
        {
            UpdateTurnState(TurnState.PlayerTurn);
        }
    }
}
