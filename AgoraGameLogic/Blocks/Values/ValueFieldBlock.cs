// using AgoraGameLogic.Domain.Entities.BuildDefinition;
// using AgoraGameLogic.Domain.Entities.Models;
// using AgoraGameLogic.Domain.Entities.Utility;
//
// namespace AgoraGameLogic.Logic.Blocks.Values;
//
// public class ValueFieldBlock : ValueBlock
// {
//     private Value<GameModule> _gameModule;
//     private Value<string> _field;
//     
//     public ValueFieldBlock(BlockDefinition definition, BlockCreationUtils controls) : base(definition, controls)
//     {
//         _gameModule = Value<GameModule>.Parse(definition.Inputs[0], controls);
//         _field = Value<string>.Parse(definition.Inputs[1], controls);
//     }
//     
//     public override T GetValue<T>(Context context)
//     {
//         var gameModule = _gameModule.GetValue(context);
//         var field = _field.GetValue(context);
//         
//         return gameModule.Fields.Get<T>(field);
//     }
// }