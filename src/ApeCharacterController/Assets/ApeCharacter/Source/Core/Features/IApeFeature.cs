namespace ApeCharacter
{
    public interface IApeFeature : IApeSystem
    {
        void Add<T>() where T : class, IApeSystem;
    }
}