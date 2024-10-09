// using AgoraGameLogic.Domain.Entities.BuildDefinition;
// using AgoraGameLogic.Domain.Entities.Models;
// using AgoraGameLogic.Domain.Entities.Utility;
// using AgoraGameLogic.Domain.Enums;
// using AgoraGameLogic.Domain.Extensions;
// using Newtonsoft.Json.Linq;
//
// namespace AgoraGameLogic.Logic.Blocks.Turns;
//
// public class AllTogetherBlock : TurnBlock
// {
//     private Value<int> _numberOfAllowedActions;
//     private BaseStatementBlock[] _startBranch;
//     private BaseStatementBlock[] _updateBranch;
//     private BaseStatementBlock[] _endBranch;
//     
//     private int _numberOfActionsMade;
//     
//     public AllTogetherBlock(BlockDefinition definition, BlockCreationUtils controls) : base(definition, controls)
//     {
//         _numberOfAllowedActions = Value<int>.Parse(definition.Inputs[0], controls);
//         _startBranch = BlockFactory.CreateArray<BaseStatementBlock>(definition.Inputs[1].AsValidArray(), controls);
//         _updateBranch = BlockFactory.CreateArray<BaseStatementBlock>(definition.Inputs[2].AsValidArray(), controls);
//         _endBranch = BlockFactory.CreateArray<BaseStatementBlock>(definition.Inputs[3].AsValidArray(), controls);
//         
//         _numberOfActionsMade = 0;
//     }
//
//     public override async Task ExecuteAsync()
//     {
//         var tasks = new List<Task>();
//
//         // For each player, add their TurnLogic task to the list
//         foreach (var player in BlockContext.Get<GameModule[]>("Players"))
//         {
//             tasks.Add(TurnLogic(player));  // TurnLogic should return a Task
//         }
//
//         // Wait for all TurnLogic tasks to complete
//         await Task.WhenAll(tasks);
//         
//         // do something when all done
//         Console.WriteLine("All done");
//     }
//
//     public void RegisterActionCount(string playerId)
//     {
//         _numberOfActionsMade++;
//         
//         Console.WriteLine("made an action.  " +
//                           (_numberOfAllowedActions.GetValue(BlockContext) - _numberOfActionsMade) + "to make");
//         
//         ValidateCompletionSource();
//     }
//     
//     public override void OnShallowCopy()
//     {
//         base.OnShallowCopy();
//         _numberOfActionsMade = 0;
//     }
//
//     private async Task TurnLogic(GameModule player)
//     {
//         BlockContext.AddOrUpdate("Player", player);
//             
//         // reset
//         _numberOfActionsMade = 0;
//             
//         // start of turn, called once
//         var startScope = new Scope(this, ScopeType.Start, player.Name);
//         await BlockControls.StartNewThreadAsync(_startBranch, BlockContext, BlockThread, startScope);
//             
//         Console.WriteLine(_numberOfActionsMade < _numberOfAllowedActions.GetValue(BlockContext));
//         Console.WriteLine(player.Name);
//
//         while (_numberOfActionsMade < _numberOfAllowedActions.GetValue(BlockContext))
//         {
//             BlockControls.FilterCommands(this, ScopeType.Update, player.Name);
//                 
//             // update, called once every action
//             var updateScope = new Scope(this, ScopeType.Update, player.Name);
//             await BlockControls.StartNewThreadAsync(_updateBranch, BlockContext, BlockThread, updateScope);
//                 
//             // wait for an action to be made
//             await CompletionSource.Task;
//             ResetCompletionSource();
//         }
//             
//         BlockControls.FilterCommands(this, ScopeType.Start, player.Name);
//         BlockControls.FilterCommands(this, ScopeType.Update, player.Name);
//             
//         // start of turn, called once
//         var endScope = new Scope(this, ScopeType.End, player.Name);
//         await BlockControls.StartNewThreadAsync(_endBranch, BlockContext, BlockThread, endScope);
//             
//         BlockControls.FilterCommands(this, ScopeType.End, player.Name);
//     }
// }