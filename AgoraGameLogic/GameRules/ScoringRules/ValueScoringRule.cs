using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.GameRules.ScoringRules;

public class ValueScoringRule : ScoringRule
{
    private Value<int> _value;
    
    public ValueScoringRule(ScoringRuleBuildData buildData, GameData gameData) : base(gameData)
    {
        _value =Value<int>.ParseOrThrow(buildData.Inputs[0], gameData);
    }

    public override Result<int> ResolveScore(IContext context, GameModule player)
    {
        return _value.GetValue(context);
    }
}