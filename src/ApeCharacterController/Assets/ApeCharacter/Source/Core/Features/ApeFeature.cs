namespace ApeCharacter
{
    public abstract class ApeFeature : IApeFeature
    {
        private readonly IApeSystemFactory _systemFactory;
        private readonly IApeCharacter _owner;

        protected ApeFeature(
            IApeSystemFactory systemFactory,
            IApeCharacter owner
        )
        {
            _systemFactory = systemFactory;
            _owner = owner;
        }
        
        public void Add<T>() where T : class, IApeSystem
        {
            _owner.Systems.AddSystem(_systemFactory.CreateSystem<T>());
        }
    }
}