using System;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Actors;

public class Value<T>
{
    private readonly ValueBlock? _blockInput;
    private readonly T? _userInput;
    
    private readonly bool _isUserInput;

    private Value(ValueBlock blockAsInput)
    {
        _isUserInput = false;
        _blockInput = blockAsInput;
        _userInput = default(T);
    }
    
    private Value(T userInput)
    {
        _isUserInput = true;
        _blockInput = null;
        _userInput = userInput;
    }

    public Result<T> GetValue(IContext context)
    {
        try
        {
            if (_isUserInput)
            {
                // User input value
                return _userInput != null ? Result<T>.Success(_userInput) : Result<T>.Failure("User input was null");
            }
            else
            {
                // Block input value
                if (_blockInput == null)
                {
                    return Result<T>.Failure("Block input was null");
                }
                
                var blockResult = _blockInput.GetValue<T>(context);
                return blockResult.IsSuccess ? blockResult : Result<T>.Failure(blockResult.Error);
            }
        }
        catch (Exception ex)
        {
            return Result<T>.Failure($"Cannot get value: {ex.Message}");
        }
    }

    public T GetValueOrThrow(IContext context)
    {
        var valueResult = GetValue(context);
        if (!valueResult.IsSuccess)
        {
            throw new Exception(valueResult.Error);
        }

        return valueResult.Value;
    }

    public static Value<T> ParseOrThrow(JToken valueToken, GameData gameData)
    {
        
        // Case 1: If the token is a block (it's an object with a "Type" field)
        if (valueToken.Type == JTokenType.Object && valueToken["Type"] != null)
        {
            var valueBlockResult = BlockFactory.Create<ValueBlock>(valueToken, gameData);
            if (!valueBlockResult.IsSuccess)
            {
                throw new Exception($"Cannot parse block {valueToken["Type"]}: {valueBlockResult.Error}");
            }
            
            return new Value<T>(valueBlockResult.Value);

        }
        
        // Case 2: If the token is a raw value, parse it into the type M
        else
        {
            var userInput = valueToken.ToObject<T>();
            
            if (userInput != null)
            {
                return new Value<T>(userInput);
            }
        
            throw new Exception("user input of wrong type or null");
        }
    }

    public static Value<T> FromOrThrow(T value)
    {
        return new Value<T>(value);
    }
}