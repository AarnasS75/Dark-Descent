using Characters.CharacterControls.Player;
using UnityEngine;

public class GameplayTab : Tab
{
    [SerializeField] private PlayerStatsControllerUI _statsControllerUI;
    [SerializeField] private PlayerActionsControllerUI _actionsControllerUI;

    public override void Initialize(IPlayer player)
    {
        _statsControllerUI.Initialize(player);
        _actionsControllerUI.Initialize(player);
    }

    public PlayerActionsControllerUI GetActionsControllerUI()
    {
        return _actionsControllerUI;
    }

    public void ShowPlayerStats()
    {
        _statsControllerUI.gameObject.SetActive(true);
    }

    public void HidePlayerStats()
    {
        _statsControllerUI.gameObject.SetActive(false);
    }

    public void ShowPlayerActions()
    {
        _actionsControllerUI.gameObject.SetActive(true);
    }

    public void HidePlayerActions()
    {
        _actionsControllerUI.gameObject.SetActive(false);
    }
}
