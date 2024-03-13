using Constants;
using System;
using Tiles;
using UnityEngine;

namespace Characters.Actions
{
    [Serializable]
    public class CombatAction
    {
        [SerializeField] private CombatActionType _characterActionType;

        public CombatActionType ActionType => _characterActionType;
    }
}