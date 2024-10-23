using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Actors;

namespace AgoraGameLogic.Utility.Commands;

public abstract class ActionCommand : Command
{
    public abstract Task<Result> PerformAsync(bool shouldRegisterAction);
}

public abstract class ActionCommand<TCommand, TBlock, TEvent> : ActionCommand
    where TCommand : ActionCommand<TCommand, TBlock, TEvent>
    where TBlock : ActionBlock<TCommand, TBlock, TEvent>
    where TEvent : EventBlock
{
    public TBlock ActionBlock;

    public ActionCommand(TBlock actionBlockStatementBlock, TurnScope? scope)
    {
        ActionBlock = actionBlockStatementBlock;
        Type = typeof(TBlock);
        Scope = scope;
    }
    
    public abstract Result Perform();
    public abstract Result Revert();
    public abstract CommandDto GetDtoCore();

    #region Wrappers

    /// <summary>
    /// PerformAsync
    /// </summary>
    public override async Task<Result> PerformAsync(bool shouldRegisterAction)
    {
        if (shouldRegisterAction && Scope != null)
        {
            // register action
            Scope.TurnBlock.RegisterActionCount(Target);
        }
        
        // perform
        var performResult = Perform();
        if (!performResult.IsSuccess)
        {
            return Result.Failure($"Error while performing {typeof(TCommand).Name}: {performResult.Error}");
        }
        
        // events
        var triggerEventResult = await ActionBlock.TriggerEventsAsync<TEvent>((TCommand)this);
        if (!triggerEventResult.IsSuccess)
        {
            return Result.Failure($"Error while triggering events for {typeof(TCommand).Name}: {triggerEventResult.Error}");
        }
        
        if (Scope != null)
        {
            // resume every player current turn.  This effectively trigger update for all
            Scope.TurnBlock.ResumeAllPlayerCurrentTurn();
        }
    
        return Result.Success();
    }

    /// <summary>
    /// Equals
    /// </summary>
    public override bool Equals(Command command)
    {
        // check type
        if (command.Type != typeof(TBlock))
        {
            return false;
        }
        
        // check args
        var commandArgs = GetArgs();
        var commandToCompareToArgs = GetArgs();

        if (commandArgs.Count != commandToCompareToArgs.Count)
        {
            return false;
        }

        for (var i = 0; i < commandArgs.Count; i++)
        {
            var commandArg = commandArgs[i];
            var commandToCompareToArg = commandToCompareToArgs[i];

            if (commandArg == null || commandToCompareToArg == null)
            {
                continue;
            }

            if (commandArg.Id != commandToCompareToArg.Id)
            {
                return false;
            }
        }

        return true;
    }
    
    /// <summary>
    /// CommandDtoBase
    /// </summary>
    public override CommandDto GetDto()
    {
        var temp = GetDtoCore();
        temp.Id = Id;
        temp.Key = typeof(TCommand).Name;
        temp.Options = Options;
        temp.TargetId = Target.Id;

        return temp;
    }

    #endregion
}