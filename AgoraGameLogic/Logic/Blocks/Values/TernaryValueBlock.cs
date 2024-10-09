// using AgoraGameLogic.Domain.Entities.BuildDefinition;
// using AgoraGameLogic.Domain.Entities.Models;
// using AgoraGameLogic.Domain.Entities.Utility;
//
// namespace AgoraGameLogic.Logic.Blocks.Values;
//
// public class TernaryValueBlock : ValueBlock
// {
//     private ConditionBlock _condition;
//     private Value<object> _trueValue;
//     private Value<object> _falseValue;
//     
//     public TernaryValueBlock(BlockDefinition definition, BlockCreationUtils controls) : base(definition, controls)
//     {
//         _condition = BlockFactory.Create<ConditionBlock>(definition.Inputs[0], controls);
//         _trueValue = Value<object>.Parse(definition.Inputs[1], controls);
//         _falseValue = Value<object>.Parse(definition.Inputs[2], controls);
//     }
//     
//     public override T GetValue<T>(Context context)
//     {
//         if (_condition.GetValue(context))
//         {
//             return (T)_trueValue.GetValue(context);
//         }
//         else
//         {
//             return (T)_falseValue.GetValue(context);
//         }
//     }
// }