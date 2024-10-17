using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks.Values;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Test;

[TestFixture]
public class BlockFactoryTests
{
    private GameData _gameData;
    
    [SetUp]
    public void SetUp()
    {
        _gameData = new GameData();
    }
    
    [Test, Category(nameof(BlockFactory.Create))]
    public void Create_WithJTokenAndWithSpecifiedType_WithValidInput_ReturnsSuccess()
    {
        // Arrange
        var token = JObject.Parse(@"
        {
            ""Type"": ""ContextValueBlock"",
            ""Inputs"": [""anInput""]
        }");

        // Act
        var createResult = BlockFactory.Create<ContextValueBlock>(token, _gameData);
    
        // Assert
        Assert.That(createResult.IsSuccess, Is.EqualTo(true));
    }
}