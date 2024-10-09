using Newtonsoft.Json;

namespace AgoraGameLogic.Domain.Entities.Models;

public class Context
{
    private Dictionary<string, object> _context = new Dictionary<string, object>();

    public void AddOrUpdate<T>(string key, ref T value)
    {
        if (_context.ContainsKey(key))
        {
            if (_context[key] is Ref<T> existingRef)
            {
                existingRef.Value = value;
            }
            else
            {
                _context[key] = new Ref<T>(value);
            }
        }
        else
        {
            _context[key] = new Ref<T>(value);
        }
    }

    public T Get<T>(string key)
    {
        if (_context.TryGetValue(key, out var value))
        {
            if (value is Ref<T> typedRef)
            {
                return typedRef.Value;
            }
            
            throw new Exception($"Type mismatch for key '{key}'. Expected type '{typeof(T)}', but found type '{value.GetType()}' wrapped in Ref<T>.");
        }

        throw new Exception($"Tried to get binding for key '{key}' in context, but it doesn't exist.");
    }

    public T GetOrDefault<T>(string key, T defaultValue)
    {
        if (_context.TryGetValue(key, out var value))
        {
            if (value is Ref<T> typedRef)
            {
                return typedRef.Value;
            }

            throw new Exception($"Type mismatch for key '{key}'. Expected type '{typeof(T)}', but found type '{value.GetType()}' wrapped in Ref<T>.");
        }

        return defaultValue;
    }

    public bool ContainsKey(string key)
    {
        return _context.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return _context.Remove(key);
    }

    public void Clear()
    {
        _context.Clear();
    }

    public Context Copy()
    {
        var copy = new Context();
        
        // Shallow copy of the dictionary
        foreach (var kvp in _context)
        {
            copy._context.Add(kvp.Key, kvp.Value);
        }

        return copy;
    }
    
    public override string ToString()
    {
        var result = new Dictionary<string, object>();

        foreach (var entry in _context)
        {
            if (entry.Value is GameModule module)
            {
                result[entry.Key] = module.Name + "*";
            }
            else if (entry.Value is List<GameModule> moduleList)
            {
                // If the value is a list of ContextGameModules, return a list of their Names
                result[entry.Key] = moduleList.ConvertAll(m => m.Name + "*");
            }
            else
            {
                // Otherwise, return the value as it is
                result[entry.Key] = entry.Value.ToString();
            }
        }

        // Serialize the dictionary to a JSON-formatted string
        return JsonConvert.SerializeObject(result, Formatting.Indented);
    }
}