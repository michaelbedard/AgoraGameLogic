using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks.Options;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Commands;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public abstract class EventBlock : Block
{
     protected EventBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData) { }

     public abstract Task<Result> TriggerAsync(IContext context, TurnScope scope, Command command, GameModule? gameModule);
}

public abstract class EventBlock<TCommand> : EventBlock
     where TCommand : Command
{
     protected StatementBlock[] Blocks;
     
     protected EventBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
     {
          BlockType = BlockType.FunctionBlock;
     }
     
     protected abstract Task<Result> TriggerAsyncCore(TCommand command);

     // wrapper function
     public override async Task<Result> TriggerAsync(IContext context, TurnScope scope, Command command, GameModule? gameModule)
     {
          try
          {
               SetUpContext(context);
               SetUpScope(scope);
               
               if (command is TCommand specificCommand)
               {
                    // OnlyTriggerIfTargetedBlock
                    if (HasOption<OnlyTriggerIfTargetedBlock>())
                    {
                         if (gameModule == null || !specificCommand.GetArgs().Contains(gameModule))
                         {
                              return Result.Success();
                         }
                    }

                    // add 'this' to context if event is module event
                    if (gameModule != null)
                    {
                         Context.AddOrUpdate("this", ref gameModule);
                    }

                    return await TriggerAsyncCore(specificCommand);
               }

               return Result.Failure($"Called {nameof(TriggerAsync)} with invalid command type.  Expected {typeof(TCommand)} but got {command.Type}", new ErrorBuilder()
               {
                    ClassName = nameof(EventBlock),
                    MethodName = nameof(TriggerAsync),
                    GameModule = gameModule,
                    Scope = Scope,
               });
          }
          catch (Exception e)
          {
               return Result.Failure($"Unexpected Errors: {e.Message}", new ErrorBuilder()
               {
                    ClassName = nameof(EventBlock),
                    MethodName = nameof(TriggerAsync),
                    GameModule = gameModule,
                    Scope = Scope,
               });
          }
     }
}