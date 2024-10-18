using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Blocks.Turns;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Actors;

namespace AgoraGameLogic.Utility.Commands;

public abstract class ActionCommand : Command
{
    public abstract Task<Result> PerformAsync(IContext context, bool shouldRegisterAction);
}

public abstract class ActionCommand<TCommand, TBlock, TEvent> : ActionCommand
    where TCommand : ActionCommand<TCommand, TBlock, TEvent>
    where TBlock : ActionBlockBase<TCommand, TBlock, TEvent>
    where TEvent : EventBlockBase
{
    public TBlock ActionBlock;

    public ActionCommand(TBlock actionBlockStatementBlock, Scope? scope)
    {
        ActionBlock = actionBlockStatementBlock;
        Type = typeof(TBlock);
        Scope = scope;
    }
    
    public abstract Result Perform(TCommand command, IContext context);
    public abstract Result Revert(TCommand command, IContext context);
    public abstract CommandDto InitializeDto();

    #region Wrappers

    /// <summary>
    /// PerformAsync
    /// </summary>
    public override async Task<Result> PerformAsync(IContext context, bool shouldRegisterAction)
    {
        // perform
        var performResult = Perform((TCommand)this, context);
        if (!performResult.IsSuccess)
        {
            return Result.Failure($"Error while performing {nameof(TCommand)}: {performResult.Error}");
        }
        
        // events
        var triggerEventResult = await ActionBlock.TriggerEventsAsync<TEvent>(context, (TCommand)this);
        if (!triggerEventResult.IsSuccess)
        {
            return Result.Failure($"Error while triggering events for {nameof(TCommand)}: {triggerEventResult.Error}");
        }

        // check if shouldRegisterAction
        if (shouldRegisterAction && Scope != null)
        {
            Scope.TurnBlock.RegisterActionCount(Target);
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
        var temp = InitializeDto();
        temp.Id = Id;
        temp.Key = typeof(TCommand).Name;
        temp.Options = Options;
        temp.TargetId = Target.Id;

        return temp;
    }

    #endregion
}