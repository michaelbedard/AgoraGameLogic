// using AgoraGameLogic.Domain.Entities.BuildDefinition;
// using AgoraGameLogic.Domain.Entities.Models;
// using AgoraGameLogic.Domain.Entities.Utility;
// using AgoraGameLogic.Domain.Extensions;
// using Newtonsoft.Json.Linq;
//
// namespace AgoraGameLogic.Logic.Blocks.Controls;
//
// public class RepeatBlock : BaseStatementBlock
// {
//     private Value<int> _iterations;
//     private BaseStatementBlock[] _loopBranch;
//     
//     public RepeatBlock(BlockDefinition definition, BlockCreationUtils controls) : base (definition, controls)
//     {
//         _iterations = Value<int>.Parse(definition.Inputs[0], controls);
//         _loopBranch = BlockFactory.CreateArray<BaseStatementBlock>(definition.Inputs[1].AsValidArray(), controls);
//     }
//
//     public override async Task ExecuteAsync()
//     {
//         for (var i = 0; i < _iterations.GetValue(BlockContext); i++)
//         {
//             await BlockControls.StartNewThreadAsync(_loopBranch, BlockContext, BlockThread, BlockScope);
//         }
//     }
// }