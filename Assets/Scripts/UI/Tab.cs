using Characters.CharacterControls.Player;
using UnityEngine;

public abstract class Tab : MonoBehaviour
{
    public abstract void Initialize(IPlayer player);

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}