using AgoraGameLogic.Domain.Entities.BuildDefinition;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Test;

[TestFixture]
public class BlockBuildDataTests
{
    [SetUp]
    public void SetUp()
    {
    }
    
    [Test, Category(nameof(BlockBuildData.Parse))]
    public void Parse_ValidToken_ReturnsSuccess()
    {
        // Arrange
        var token = JObject.Parse(@"
        {
            ""Type"": ""SampleType"",
            ""Inputs"": [1, 2, 3],
            ""Options"": [
                { ""Type"": ""OptionType1"", ""Inputs"": [], ""Options"": [] },
                { ""Type"": ""OptionType2"", ""Inputs"": [], ""Options"": [] }
            ]
        }");

        // Act
        var result = BlockBuildData.Parse(token);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value.Type, Is.EqualTo("SampleType"));
        Assert.That(result.Value.Inputs.Count, Is.EqualTo(3));
        Assert.That(result.Value.Options.Count(), Is.EqualTo(2));
    }
    
    [Test, Category(nameof(BlockBuildData.Parse))]
    public void Parse_MissingType_ReturnsFailure()
    {
        // Arrange
        var token = JObject.Parse(@"
            {
                ""Inputs"": [1, 2, 3],
                ""Options"": []
            }");

        // Act
        var result = BlockBuildData.Parse(token);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Error, Does.Contain("Type"));
    }
    
    [Test, Category(nameof(BlockBuildData.Parse))]
    public void Parse_InvalidInputs_ReturnsFailure()
    {
        // Arrange
        var token = JObject.Parse(@"
            {
                ""Type"": ""SampleType"",
                ""Inputs"": ""InvalidInput"",
                ""Options"": []
            }");

        // Act
        var result = BlockBuildData.Parse(token);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.Error, Does.Contain("Inputs"));
    }
}