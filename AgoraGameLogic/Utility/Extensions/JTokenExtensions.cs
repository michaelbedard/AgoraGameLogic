using AgoraGameLogic.Entities;
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

    /// <summary>
    /// Attempts to retrieve the value associated with the specified key from the JToken.
    /// Return converted value to the specified type
    /// </summary>
    public static Result<T> GetItem<T>(this JToken token, string key)
    {
        try
        {
            var valueToken = token[key];

            if (valueToken == null)
            {
                return Result<T>.Failure($"Missing required field: {key}");
            }

            // Attempt to convert the token to the specified type.
            var value = valueToken.ToObject<T>();

            return Result<T>.Success(value);
        }
        catch (InvalidCastException)
        {
            return Result<T>.Failure($"The field '{key}' does not match the expected type {typeof(T).Name}.");
        }
        catch (Exception ex)
        {
            return Result<T>.Failure($"Failed to retrieve the field '{key}' as type {typeof(T).Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// Attempts to retrieve the value associated with the specified key from the JToken. Return the JToken object
    /// </summary>
    public static Result<JToken> GetItem(this JToken token, string key)
    {
        try
        {
            return Result<JToken>.Success(token[key]);
        }
        catch (Exception e)
        {
            return Result<JToken>.Failure($"JToken does not have key {key}: {e.Message}");
        }
    }
    
    
    /// <summary>
    /// Attempts to retrieve the value associated with the specified key from the JToken.
    /// If fails, return a default object
    /// </summary>
    public static T GetItemOrDefault<T>(this JToken token, string key, T defaultValue)
    {
        var itemResult = token.GetItem<T>(key);
        if (!itemResult.IsSuccess)
        {
            return defaultValue;
        }

        return itemResult.Value;
    }
    
    /// <summary>
    /// Attempts to retrieve the value associated with the specified key from the JToken.
    /// If fails, throw an exception
    /// </summary>
    public static T GetItemOrThrow<T>(this JToken token, string key)
    {
        var itemResult = token.GetItem<T>(key);
        if (!itemResult.IsSuccess)
        {
            throw new Exception(itemResult.Error);
        }

        return itemResult.Value;
    }
    
    /// <summary>
    /// Attempts to retrieve the value JToken with the specified key from the original JToken.
    /// If fails, throw an exception
    /// </summary>
    public static JToken GetItemOrThrow(this JToken token, string key)
    {
        var itemResult = token.GetItem(key);
        if (!itemResult.IsSuccess)
        {
            throw new Exception(itemResult.Error);
        }

        return itemResult.Value;
    }
    
    /// <summary>
    /// Attempts to convert the given JToken to the specified type. 
    /// </summary>
    public static Result<T> TryGetValue<T>(this JToken valueToken)
    {
        try
        {
            var value = valueToken.ToObject<T>();
            return Result<T>.Success(value);
        }
        catch (InvalidCastException)
        {
            return Result<T>.Failure($"The token does not match the expected type {typeof(T).Name}.");
        }
        catch (Exception ex)
        {
            return Result<T>.Failure($"Failed to convert the token to type {typeof(T).Name}: {ex.Message}");
        }
    }
}
