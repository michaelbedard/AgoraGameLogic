// using AgoraGameLogic.Domain.Entities.BuildDefinition;
// using AgoraGameLogic.Domain.Entities.Models;
// using AgoraGameLogic.Domain.Entities.Utility;
// using AgoraGameLogic.Domain.Extensions;
//
// namespace AgoraGameLogic.Logic.Blocks.Controls;
//
// public class ForeachPlayerBlock : BaseStatementBlock
// {
//     private Value<string> _key;
//     private BaseStatementBlock[] _loopBranch;
//     
//     private object _cacheValue;
//     
//     public ForeachPlayerBlock(BlockDefinition definition, GameData controls) : base (definition, controls)
//     {
//         _key = Value<string>.Parse(definition.Inputs[0], controls);
//         _loopBranch = BlockFactory.CreateArray<BaseStatementBlock>(definition.Inputs[1].AsValidArray(), controls);
//     }
//
//     public override async Task ExecuteAsync()
//     {
//         var key = _key.GetValue(BlockContext);
//         _cacheValue = BlockContext.GetOrDefault<object>(key, null);
//             
//         foreach (var player in BlockControls.Players)
//         {
//             BlockContext.AddOrUpdate(key, player);
//             await BlockControls.StartNewThreadAsync(_loopBranch, BlockContext, BlockThread, BlockScope);
//         }
//         
//         BlockContext.AddOrUpdate(key, _cacheValue);
//     }
// }