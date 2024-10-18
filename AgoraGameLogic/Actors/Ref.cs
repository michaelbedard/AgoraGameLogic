namespace AgoraGameLogic.Actors;

public class Ref<T>
{
    public T Value { get; set; }

    public Ref(T value)
    {
        Value = value;
    }
}