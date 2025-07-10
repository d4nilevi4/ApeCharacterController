namespace ApeCharacter
{
    public interface IApeCharacter
    {
        IApeSystemsContainer Systems { get; }
        IApeComponentsContainer Components { get; }
    }
}