using Characters.CharacterControls.Player;
using Constants;
using Managers.InpuHandling;
using Managers.Rooms;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("MANAGERS")]
    [SerializeField] private RoomsManager _roomsManager;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CanvasManager _canvasManager;
    [SerializeField] private TurnManager _turnManager;

    [Header("PLAYER REFERENCE")]
    [SerializeField] private GameObject _playerPrefab;
    private Player _player;

    private void Awake()
    {
        UpdateGameState(GameState.GameStarted);
    }

    private void OnEnable()
    {
        _turnManager.OnEndTurn += _turnSystem_OnTurnStarted;
    }

    private void OnDisable()
    {
        _turnManager.OnEndTurn -= _turnSystem_OnTurnStarted;
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
                break;

            case GameState.GamePaused:
                break;
        }
    }

    private void _turnSystem_OnTurnStarted(TurnState turnState)
    {
        switch (turnState)
        {
            case TurnState.PlayerTurn:
                _canvasManager.GetTab<GameplayTab>().ShowPlayerActions();
                break;

            case TurnState.EnemyTurn:
                _canvasManager.GetTab<GameplayTab>().HidePlayerActions();
                break;

            case TurnState.EnvironmentTurn:
                //_canvasManager.GetTab<GameplayTab>().HidePlayerActions();
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

    private void InitializeManagers()
    {
        _roomsManager.Initialize(_player);
        _canvasManager.Initialize(_player);
        _inputManager.Initialize(_player);
        _turnManager.Initialize(_player);

        UpdateGameState(GameState.PlayingLevel);
    }
}
