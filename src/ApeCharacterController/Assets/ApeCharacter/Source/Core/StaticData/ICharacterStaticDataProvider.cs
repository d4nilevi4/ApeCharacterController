namespace ApeCharacter
{
    public interface ICharacterStaticDataProvider
    {
        public T GetConfig<T>();
    }
}