using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Logic.Blocks;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.Utility;

public class Value<T>
{
    private readonly BaseValueBlock? _blockInput;
    private readonly T? _userInput;
    
    private readonly bool _isUserInput;

    private Value(BaseValueBlock blockAsInput)
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

    public T GetValue(Context context)
    {
        if (_isUserInput)
        {
            if (_userInput != null) return _userInput;
            throw new Exception("user input was null");
        };

        if (_blockInput != null)
        {
            return _blockInput.GetValue<T>(context);
        }

        throw new Exception("block input was null");
    }

    public static Value<T> Parse(JToken valueToken, GameData gameData)
    {
        
        // Case 1: If the token is a block (it's an object with a "Type" field)
        if (valueToken.Type == JTokenType.Object && valueToken["Type"] != null)
        {
            var valueBlock = BlockFactory.Create<BaseValueBlock>(valueToken, gameData);
            return new Value<T>(valueBlock);

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

    public static Value<T> From(T value)
    {
        return new Value<T>(value);
    }
}