using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks.Options;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Blocks;

public abstract class EventBlockBase : BlockBase
{
     protected EventBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
     {
     }

     public abstract Task<Result> TriggerAsync(GameModule? gameModule, IContext context, Command command, Scope? scope);
}

public abstract class EventBlockBase<TCommand> : EventBlockBase
     where TCommand : Command
{
     protected StatementBlockBase[] Blocks;
     
     protected EventBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
     {
     }
     
     protected abstract Task<Result> TriggerAsync(IContext context, TCommand command, Scope? scope);

     // wrapper function
     public override async Task<Result> TriggerAsync(GameModule? gameModule, IContext context, Command command, Scope? scope)
     {
          try
          {
               if (command is TCommand specificCommand)
               {
                    if (HasOption<OnlyTriggerIfTargetedBlock>())
                    {
                         if (gameModule == null || specificCommand.GetArgs().Contains(gameModule))
                         {
                              return Result.Success();
                         }
                    }

                    if (gameModule != null)
                    {
                         context.AddOrUpdate("this", ref gameModule);
                    }

                    return await TriggerAsync(context, specificCommand, scope);
               }

               return Result.Failure($"Called {nameof(TriggerAsync)} with invalid command type.  Expected {typeof(TCommand)} but got {command.Type}", new ErrorBuilder()
               {
                    ClassName = nameof(EventBlockBase),
                    MethodName = nameof(TriggerAsync),
                    Context = context,
                    GameModule = gameModule,
                    Scope = scope,
               });
          }
          catch (Exception e)
          {
               return Result.Failure($"Unexpected Errors: {e.Message}", new ErrorBuilder()
               {
                    ClassName = nameof(EventBlockBase),
                    MethodName = nameof(TriggerAsync),
                    Context = context,
                    GameModule = gameModule,
                    Scope = scope,
               });
          }
     }
}