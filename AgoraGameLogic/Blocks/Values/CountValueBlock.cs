// using AgoraGameLogic.Domain.Entities.BuildDefinition;
// using AgoraGameLogic.Domain.Entities.Models;
// using AgoraGameLogic.Domain.Entities.Utility;
//
// namespace AgoraGameLogic.Logic.Blocks.Values;
//
// public class CountValueBlock : ValueBlock
// {
//     private Value<IEnumerable<object>> _enumerable;
//     
//     public CountValueBlock(BlockDefinition definition, BlockCreationUtils controls) : base(definition, controls)
//     {
//         _enumerable = Value<IEnumerable<object>>.Parse(definition.Inputs[0], controls);
//     }
//
//     public override T GetValue<T>(Context context)
//     {
//         var enumerable = _enumerable.GetValue(context);
//         var count = enumerable.Count();
//         
//         if (typeof(T) == typeof(int) || typeof(T) == typeof(object))
//         {
//             return (T)(object)count;
//         }
//
//         throw new InvalidCastException($"Cannot convert count (int) to type {typeof(T)}.");
//     }
// }