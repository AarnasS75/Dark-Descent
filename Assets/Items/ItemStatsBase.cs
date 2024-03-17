using System.Collections;
using UnityEngine;

public class ItemStatsBase : ScriptableObject
{
    [Header("GENERAL")]
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;

    public string Name => _name;
    public Sprite Sprite => _sprite;
}