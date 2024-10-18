using System;
using System.Collections.Generic;
using System.Linq;
using AgoraGameLogic.Interfaces.Actors;
using Newtonsoft.Json;

namespace AgoraGameLogic.Actors;

public class Context : IContext
{
    private Dictionary<string, object> _context = new Dictionary<string, object>();
    
    public void AddOrUpdate<T>(string key, T value)
    {
        AddOrUpdate(key, ref value);
    }

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
                return (T)typedRef.Value;
            }
            
            if (typeof(T) == typeof(object) && value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(Ref<>))
            {
                var genericValue = value.GetType().GetProperty("Value").GetValue(value);
                return (T)(genericValue);
            }
            
            throw new Exception($"Type mismatch for key '{key}'. Expected type '{typeof(T)}', but found type '{value.GetType()}' wrapped in Ref<T>.");
        }

        throw new Exception($"Tried to get binding for key '{key}' in context, but it doesn't exist.");
    }

    public T GetOrDefault<T>(string key, T defaultValue)
    {
        if (_context.TryGetValue(key, out var value) && value is Ref<T> typedRef)
        {
            return typedRef.Value;
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

    public IContext Copy()
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
            if (entry.Value is Ref<GameModule> moduleRef)
            {
                result[entry.Key] = moduleRef.Value;
            }
            else if (entry.Value is Ref<List<GameModule>> moduleListRef)
            {
                var moduleIds = moduleListRef.Value.Select(m => m.Id).ToList();
                result[entry.Key] = moduleIds; 
            }
            else
            {
                // Otherwise, return the value as it is
                result[entry.Key] = ((Ref<Value<object>>)entry.Value).Value.GetValue(this).Value;
            }
        }
        // Create custom settings to include fields
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
            {
                DefaultMembersSearchFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.FlattenHierarchy
            }
        };

        // Serialize the dictionary to a JSON-formatted string with custom settings
        return JsonConvert.SerializeObject(result, settings);
    }
}