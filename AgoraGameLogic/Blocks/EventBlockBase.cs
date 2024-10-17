using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks._options;

namespace AgoraGameLogic.Logic.Blocks.Values;

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

               return Result.Failure($"Invalid type of command");
          }
          catch (Exception e)
          {
               return Result.Failure(e.Message);
          }
     }
}