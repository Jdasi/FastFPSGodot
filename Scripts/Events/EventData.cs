namespace EventData;

public interface IEventData { }

public struct PlayerHealthChanged : IEventData
{
    public float PrevHealth;
    public float NewHealth;
}
