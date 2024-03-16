using Constants;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionsControllerUI : MonoBehaviour
{
    [SerializeField] private Button _moveActionBtn;
    [SerializeField] private Button _attackActionBtn;
    [SerializeField] private Button _endTurnActionBtn;

    private IPlayer _player;

    public void Initialize(IPlayer player)
    {
        _player = player;

        _moveActionBtn.onClick.AddListener(() =>
        {
            _player.TakeAction(CombatActionType.Move, null);
        });

        _attackActionBtn.onClick.AddListener(() =>
        {
            _player.TakeAction(CombatActionType.Attack, null);
        });

        _endTurnActionBtn.onClick.AddListener(() =>
        {
            _player.TakeAction(CombatActionType.EndTurn, null);
        });
    }
}
