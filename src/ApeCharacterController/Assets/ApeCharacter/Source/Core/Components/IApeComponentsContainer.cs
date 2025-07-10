namespace ApeCharacter
{
    public interface IApeComponentsContainer
    {
        void AddComponent<T>(T component) where T : IApeComponent;
        void RemoveComponent<T>() where T : IApeComponent;
        bool TryGetComponent<T>(out T component) where T : IApeComponent;
        bool HasComponent<T>() where T : IApeComponent;
        T GetComponent<T>() where T : IApeComponent;
    }
}