using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic.Blocks._options;

namespace AgoraGameLogic.Logic.Blocks.Values;

public abstract class EventBlockBase : BlockBase
{
     protected StatementBlockBase[] Blocks;
     
     protected EventBlockBase(BlockDefinition definition, GameData gameData) : base(definition, gameData)
     {
     }
     
     protected abstract Task TriggerAsync(IContext context, object[] args, Scope? scope);

     // wrapper function
     public async Task TriggerAsync(GameModule? gameModule, IContext context, object[] args, Scope? scope)
     {
          if (HasOption<OnlyTriggerIfTargetedBlock>())
          {
               if (gameModule == null || args.Contains(gameModule))
               {
                    return;
               }
          }
          
          if (gameModule != null)
          {
               context.AddOrUpdate("this", ref gameModule);
          }
          
          await TriggerAsync(context, args, scope);
     }
}