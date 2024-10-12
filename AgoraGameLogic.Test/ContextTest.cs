using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Test;

[TestFixture]
public class ContextTests
{
    private IContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = new Context();
    }

    #region AddOrUpdate

    [Test, Category(nameof(Context.AddOrUpdate))]
    [TestCase("value")]
    [TestCase(10)]
    [TestCase(true)]
    public void AddOrUpdate_ShouldAddNewValue_WhenKeyDoesNotExist_WithSimpleTypes(object value)
    {
        // Arrange
        var key = "testKey";

        // Act
        _context.AddOrUpdate(key, ref value);
    
        // Assert
        var result = _context.Get<object>(key);
        Assert.That(result, Is.EqualTo(value));
    }

    [Test, Category(nameof(Context.AddOrUpdate))]
    public void AddOrUpdate_WithComplexType_WithCorrespondingTypeT_AddValue()
    {
        // Arrange
        var key = "testKey";
        var value = new GameModule("AnId", "AName", GameModuleType.Player, new string[] {});

        // Act
        _context.AddOrUpdate(key, ref value);
    
        // Assert
        var result = _context.Get<GameModule>(key);
        Assert.That(result.Id, Is.EqualTo("AnId"));
    }
    
    [Test, Category(nameof(Context.AddOrUpdate))]
    public void AddOrUpdate_WithComplexType_WithObjectTypeT_AddValue()
    {
        // Arrange
        var key = "testKey";
        var value = new GameModule("AnId", "AName", GameModuleType.Player, new string[] {});

        // Act
        _context.AddOrUpdate(key, ref value);
    
        // Assert
        var result = (GameModule)_context.Get<object>(key);
        Assert.That(result.Id, Is.EqualTo("AnId"));
    }

    [Test, Category(nameof(Context.AddOrUpdate))]
    public void AddOrUpdate_WhenKeyExists_ShouldUpdateRefValue()
    {
        // Arrange
        var key = "testKey";
        var initialValue = 10;
        var updatedValue = 30;

        // Act
        _context.AddOrUpdate(key, ref initialValue);
        _context.AddOrUpdate(key, ref updatedValue);

        // Assert
        var result = _context.Get<int>(key);
        Assert.That(result, Is.EqualTo(30));  // Ensure the reference's value is updated
    }

    #endregion

    #region Get

    [Test, Category(nameof(Context.Get))]
    public void Get_ShouldReturnStoredValue_WhenKeyExistsAndTypeIsCorrect()
    {
        // Arrange
        var key = "existingKey";
        var storedValue = new GameModule("AnId", "AName", GameModuleType.Player, new string[] {});
        _context.AddOrUpdate(key, ref storedValue);

        // Act
        var result = _context.Get<GameModule>(key);

        // Assert
        Assert.That(result, Is.EqualTo(storedValue));
    }
    
    [Test, Category(nameof(Context.Get))]
    public void Get_ShouldThrowException_WhenKeyExistsButTypeIsIncorrect()
    {
        // Arrange
        var key = "existingKey";
        var storedValue = new GameModule("AnId", "AName", GameModuleType.Player, new string[] {});
        _context.AddOrUpdate(key, ref storedValue);

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _context.Get<int>(key));
    }
    
    [Test, Category(nameof(Context.Get))]
    public void Get_ShouldThrowException_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "nonExistentKey";

        // Act & Assert
        Assert.Throws<Exception>(() => _context.Get<int>(key));
    }
    
    [Test, Category(nameof(Context.Get))]
    public void Get_ShouldHandleComplexType_WithCorrectType()
    {
        // Arrange
        var key = "complexKey";
        var storedValue = new GameModule("StoredId", "StoredName", GameModuleType.Player, new string[] {});
        _context.AddOrUpdate(key, ref storedValue);

        // Act
        var result = _context.Get<GameModule>(key);

        // Assert
        Assert.That(result.Id, Is.EqualTo("StoredId"));
    }

    [Test, Category(nameof(Context.Get))]
    public void Get_ShouldReturnPrimitiveValue_WhenKeyExistsWithCorrectType()
    {
        // Arrange
        var key = "intKey";
        var storedValue = 42;
        _context.AddOrUpdate(key, ref storedValue);

        // Act
        var result = _context.Get<int>(key);

        // Assert
        Assert.That(result, Is.EqualTo(42));
    }

    [Test, Category(nameof(Context.Get))]
    public void Get_ShouldThrowException_WhenTypeDoesNotMatchWithPrimitiveValue()
    {
        // Arrange
        var key = "intKey";
        var storedValue = 42;
        _context.AddOrUpdate(key, ref storedValue);

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _context.Get<string>(key));
    }
    
    [Test, Category(nameof(Context.Get))]
    public void Get_ShouldReturnValue_WhenKeyExistsAndRequestedAsObject()
    {
        // Arrange
        var key = "existingKey";
        var storedValue = new GameModule("AnId", "AName", GameModuleType.Player, new string[] {});
        _context.AddOrUpdate(key, ref storedValue);

        // Act
        var result = _context.Get<object>(key);  // Retrieve as object

        // Assert
        Assert.That(result, Is.InstanceOf<GameModule>());
        Assert.That(((GameModule)result).Id, Is.EqualTo("AnId"));  // Cast to GameModule and check
    }

    #endregion

    #region GetOrDefault

    [Test, Category(nameof(Context.GetOrDefault))]
    public void GetOrDefault_WhenKeyDoesNotExist_ShouldReturnDefaultValue()
    {
        // Arrange
        var key = "nonExistentKey";
        var defaultValue = 42;

        // Act
        var result = _context.GetOrDefault(key, defaultValue);

        // Assert
        Assert.That(result, Is.EqualTo(defaultValue));
    }

    [Test, Category(nameof(Context.GetOrDefault))]
    public void GetOrDefault_ShouldReturnStoredValue_WhenKeyExistsWithCorrectType()
    {
        // Arrange
        var key = "existingKey";
        var storedValue = new GameModule("AnId", "AName", GameModuleType.Player, new string[] {});
        _context.AddOrUpdate(key, ref storedValue);

        // Act
        var result = _context.GetOrDefault(key, default(GameModule));

        // Assert
        Assert.That(result, Is.EqualTo(storedValue));
    }

    [Test, Category(nameof(Context.GetOrDefault))]
    public void GetOrDefault_ShouldReturnDefaultValue_WhenKeyExistsButTypeMismatch()
    {
        // Arrange
        var key = "existingKey";
        var storedValue = 42;  // Storing an int
        _context.AddOrUpdate(key, ref storedValue);

        // Act
        var result = _context.GetOrDefault(key, "default string");

        // Assert
        Assert.That(result, Is.EqualTo("default string"));  // Should return default value, since type mismatch
    }

    [Test, Category(nameof(Context.GetOrDefault))]
    public void GetOrDefault_ShouldHandleComplexType()
    {
        // Arrange
        var key = "complexKey";
        var defaultValue = new GameModule("DefaultId", "DefaultName", GameModuleType.Card, new string[] {});
        var storedValue = new GameModule("StoredId", "StoredName", GameModuleType.Player, new string[] {});
        _context.AddOrUpdate(key, ref storedValue);

        // Act
        var result = _context.GetOrDefault(key, defaultValue);

        // Assert
        Assert.That(result, Is.EqualTo(storedValue));
    }

    #endregion
}