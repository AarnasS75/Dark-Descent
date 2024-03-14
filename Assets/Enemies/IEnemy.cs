public interface IEnemy : ICharacter
{
    bool CreateActionScenario(IPlayer player);
    int GetExp();
}
