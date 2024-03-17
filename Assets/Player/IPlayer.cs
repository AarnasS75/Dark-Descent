
using UnityEngine;

public interface IPlayer : ICharacter
{
    LevelSystem LevelSystem { get; }

    void RefreshActions();
}
