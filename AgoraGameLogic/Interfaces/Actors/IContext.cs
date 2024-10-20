namespace AgoraGameLogic.Interfaces.Actors;

public interface IContext
{
    void AddOrUpdate<T>(string key, T value);
    void AddOrUpdate<T>(string key, ref T value);
    T Get<T>(string key);
    T GetOrDefault<T>(string key, T defaultValue);
    bool ContainsKey(string key);
    bool Remove(string key);
    void Clear();
    IContext Copy();
}