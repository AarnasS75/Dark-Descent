using Characters.CharacterControls.Player;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Tab _startingTab;
    [SerializeField] private Tab[] _tabs;

    private Tab _currentTab;

    public void Initialize(IPlayer player)
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            _tabs[i].Initialize(player);
            _tabs[i].Hide();
        }

        if (_startingTab != null)
        {
            Show(_startingTab);
        }
    }

    public T GetTab<T>() where T : Tab
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            if (_tabs[i] is T tTab)
            {
                return tTab;
            }
        }
        return null;
    }

    private void Show(Tab tab)
    {
        if (_currentTab != null)
        {
            _currentTab.Hide();
        }
        tab.Show();

        _currentTab = tab;
    }
}
