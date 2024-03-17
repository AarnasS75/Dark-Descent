namespace Characters.CharacterControls.Player
{
    public class Player : CharacterBase<PlayerStats>, IPlayer
    {
        public LevelSystem LevelSystem { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LevelSystem = new LevelSystem(this);
        }

        public void RefreshActions()
        {
            ResetActionCount();
        }
    }
}