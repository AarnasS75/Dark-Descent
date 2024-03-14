using Characters.CharacterControls.HealthEvents;
using Characters.CharacterControls.Player;
using Constants;
using Managers.InpuHandling;
using Managers.Rooms;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("MANAGERS")]
    [SerializeField] private RoomsManager _roomsManager;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CanvasManager _canvasManager;
    [SerializeField] private TurnManager _turnManager;

    [Header("PLAYER")]
    [SerializeField] private GameObject _playerPrefab;
    private Player _player;

    private void Awake()
    {
        UpdateGameState(GameState.GameStarted);
    }

    private void OnEnable()
    {
        _turnManager.OnCharacterTurn += _turnSystem_OnCharacterTurn;
        _player.HealthEvents.OnDied += PlayerHealthEvents_OnDied;
    }

    private void OnDisable()
    {
        _turnManager.OnCharacterTurn -= _turnSystem_OnCharacterTurn;
        _player.HealthEvents.OnDied -= PlayerHealthEvents_OnDied;
    }

    private void _turnSystem_OnCharacterTurn(ICharacter character)
    {
        if(character is IPlayer player)
        {
            _canvasManager.GetTab<GameplayTab>().ShowPlayerActions();
        }
        else
        {
            _canvasManager.GetTab<GameplayTab>().HidePlayerActions();
        }
    }

    private void UpdateGameState(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.GameStarted:
                InitializePlayer();
                InitializeManagers();
                break;

            case GameState.PlayingLevel:
                _roomsManager.LoadRoom();
                _turnManager.Launch(_roomsManager.GetSpawnedEnemies());
                break;

            case GameState.GameEnded:
                print("gg");
                break;

            case GameState.GamePaused:
                break;
        }
    }

    private void InitializePlayer()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null)
        {
            _player = Instantiate(_playerPrefab).GetComponent<Player>();
        }
    }

    private void PlayerHealthEvents_OnDied(CharacterHealthEvents arg1, CharacterDiedEventArgs arg2)
    {
        UpdateGameState(GameState.GameEnded);
    }

    private void InitializeManagers()
    {
        _roomsManager.Initialize(_player);
        _canvasManager.Initialize(_player);
        _inputManager.Initialize(_player);
        _turnManager.Initialize(_player);

        UpdateGameState(GameState.PlayingLevel);
    }
}
