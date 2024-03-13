using Characters.CharacterControls.AttackEvents;
using Characters.CharacterControls.MovementEvents;
using Constants;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private IPlayer _player;
    private List<IEnemy> _enemies;
    private int _currentEnemyIndex;

    public event Action<TurnState> OnEndTurn;

    private void OnDisable()
    {
        _player.AttackEvents.OnAttacked -= Character_OnAttacked;
        _player.MovementEvents.OnCharacterStopped -= Character_OnStopped;

        foreach (var enemy in _enemies)
        {
            enemy.MovementEvents.OnCharacterStopped -= Character_OnStopped;
            enemy.AttackEvents.OnAttacked -= Character_OnAttacked;
        }
    }

    public void Initialize(IPlayer player)
    {
        _player = player;
        _player.AttackEvents.OnAttacked += Character_OnAttacked;
        _player.MovementEvents.OnCharacterStopped += Character_OnStopped;
    }

    public void Launch(List<IEnemy> enemies)
    {
        _enemies = enemies;
        _currentEnemyIndex = 0;

        foreach (var enemy in enemies)
        {
            enemy.MovementEvents.OnCharacterStopped += Character_OnStopped;
            enemy.AttackEvents.OnAttacked += Character_OnAttacked;
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
                PlayerTurn();
                break;

            case TurnState.EnemyTurn:
                EnemyTurn();
                break;

            case TurnState.EnvironmentTurn:
                // TODO:
                break;
        }

        OnEndTurn?.Invoke(turnState);
    }

    private void Character_OnStopped(CharacterMovementEvents events, CharacterStoppedEventArgs args)
    {
        if(args.Character is IEnemy enemy)
        {
            if(enemy.GetRemainingActionsCount() > 0)
            {
                enemy.TakeAction(_player);
            }
            else
            {
                GetNextEnemyTurn();
            }
        }
    }

    private void Character_OnAttacked(CharacterAttackEvents events, CharacterAttackedEventArgs args)
    {
        if (args.Character is IEnemy enemy)
        {
            GetNextEnemyTurn();
        }
    }

    private void GetNextEnemyTurn()
    {
        _currentEnemyIndex++;
        if (_currentEnemyIndex >= _enemies.Count)
        {
            // All enemies have finished their turns, switch back to player's turn
            _currentEnemyIndex = 0;
            UpdateTurnState(TurnState.PlayerTurn);
        }
        else
        {
            // Start the turn for the next enemy
            _enemies[_currentEnemyIndex].ResetAction();
            _enemies[_currentEnemyIndex].TakeAction(_player);
        }
    }

    private void EnemyTurn()
    {
        // Start the turn for the first enemy
        _enemies[_currentEnemyIndex].ResetAction();
        _enemies[_currentEnemyIndex].TakeAction(_player);
    }

    private void PlayerTurn()
    {
        _player.ResetAction();
    }
}
