using UnityEngine;

public abstract class ItemBase<T> : MonoBehaviour
{
    [SerializeField] protected T _stats;

    public abstract void Initialize(ICharacter character);
    public abstract void Use(ICharacter target);
}