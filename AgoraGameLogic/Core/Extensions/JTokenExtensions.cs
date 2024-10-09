using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Extensions;

public static class JTokenExtensions
{
    public static JArray AsValidArray(this JToken token)
    {
        if (token == null || token.Type != JTokenType.Array)
        {
            throw new ArgumentException("Expected a non-null JArray with values.");
        }
        return (JArray)token;
    }

    public static T GetValueOrThrow<T>(this JToken token, string key)
    {
        var valueToken = token[key];
    
        if (valueToken == null)
        {
            throw new ArgumentException($"Missing required field: {key}");
        }

        // Check if the type of the value matches the expected type
        try
        {
            return valueToken.ToObject<T>();
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException($"The field '{key}' does not match the expected type {typeof(T).Name}.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to retrieve the field '{key}' as type {typeof(T).Name}: {ex.Message}");
        }
    }


    public static JToken GetValueOrThrow(this JToken token, string key)
    {
        return token[key] ?? throw new ArgumentException($"Missing required value for: {key}");
    }
    
    
    public static T GetValueOrThrow<T>(this JToken valueToken)
    {
        if (valueToken == null || valueToken.Type == JTokenType.Null)
        {
            throw new ArgumentException($"Missing token");
        }

        // Check if the type of the value matches the expected type
        try
        {
            return valueToken.ToObject<T>();
        }
        catch (InvalidCastException)
        {
                throw new InvalidCastException($"The valueToken does not match the expected type {typeof(T).Name}.");
        }
        catch (Exception ex)
        {
                throw new Exception($"Failed to retrieve the valueToken as type {typeof(T).Name}: {ex.Message}");
        }
    }
    
    public static T GetValueOrDefault<T>(this JToken valueToken, string key, T defaultValue)
    {
        var token = valueToken[key];
    
        if (token == null || token.Type == JTokenType.Null)
        {
            return defaultValue;
        }
        
        try
        {
            return token.ToObject<T>();
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException($"The valueToken does not match the expected type {typeof(T).Name}.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to retrieve the valueToken as type {typeof(T).Name}: {ex.Message}");
        }
    }
    
}
