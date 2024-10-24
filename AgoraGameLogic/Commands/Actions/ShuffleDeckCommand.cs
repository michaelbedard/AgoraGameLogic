using System;
using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Actions.ShuffleDeck;

public class ShuffleDeckCommand : ActionCommand<ShuffleDeckCommand, ShuffleDeckBlock, OnShuffleDeckBlock>
{
    public GameModule Deck;
    
    public ShuffleDeckCommand(ShuffleDeckBlock actionBlock, TurnScope? scope) : base(actionBlock, scope)
    {
    }

    public override Result Perform()
    {
        try
        {
            // logic
            Deck.Fields.Get<List<GameModule>>("Cards").Shuffle();

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public override Result Revert()
    {
        throw new NotImplementedException();
    }

    public override CommandDto GetDtoCore()
    {
        throw new NotImplementedException();
    }

    public override List<GameModule> GetArgs()
    {
        return new List<GameModule>() { Target, Deck };
    }
}