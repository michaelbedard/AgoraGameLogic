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

    [Test, Category(nameof(IContext.AddOrUpdate))]
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

    [Test, Category(nameof(IContext.AddOrUpdate))]
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
    
    [Test, Category(nameof(IContext.AddOrUpdate))]
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

    [Test, Category(nameof(IContext.AddOrUpdate))]
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

    [Test, Category(nameof(IContext.Get))]
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
    
    [Test, Category(nameof(IContext.Get))]
    public void Get_ShouldThrowException_WhenKeyExistsButTypeIsIncorrect()
    {
        // Arrange
        var key = "existingKey";
        var storedValue = new GameModule("AnId", "AName", GameModuleType.Player, new string[] {});
        _context.AddOrUpdate(key, ref storedValue);

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _context.Get<int>(key));
    }
    
    [Test, Category(nameof(IContext.Get))]
    public void Get_ShouldThrowException_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "nonExistentKey";

        // Act & Assert
        Assert.Throws<Exception>(() => _context.Get<int>(key));
    }
    
    [Test, Category(nameof(IContext.Get))]
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

    [Test, Category(nameof(IContext.Get))]
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

    [Test, Category(nameof(IContext.Get))]
    public void Get_ShouldThrowException_WhenTypeDoesNotMatchWithPrimitiveValue()
    {
        // Arrange
        var key = "intKey";
        var storedValue = 42;
        _context.AddOrUpdate(key, ref storedValue);

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _context.Get<string>(key));
    }
    
    [Test, Category(nameof(IContext.Get))]
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

    [Test, Category(nameof(IContext.GetOrDefault))]
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

    [Test, Category(nameof(IContext.GetOrDefault))]
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

    [Test, Category(nameof(IContext.GetOrDefault))]
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

    [Test, Category(nameof(IContext.GetOrDefault))]
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

    #region Copy

    [Test, Category(nameof(IContext.Copy))]
    public void Copy_ShouldContainSameKeysAndValues()
    {
        // Arrange
        var intValue = 42;
        var stringValue = "Hello";
        _context.AddOrUpdate("intKey", ref intValue);
        _context.AddOrUpdate("stringKey", ref stringValue);

        // Act
        var copiedContext = (Context)_context.Copy();

        // Assert
        Assert.That(copiedContext.Get<int>("intKey"), Is.EqualTo(42));
        Assert.That(copiedContext.Get<string>("stringKey"), Is.EqualTo("Hello"));
    }

    [Test, Category(nameof(IContext.Copy))]
    public void Copy_WhenOriginalEntriesAreModified_shouldReflectOnOriginal()
    {
        // Arrange
        var intValue = 10;
        _context.AddOrUpdate("key", ref intValue);
        var copiedContext = (Context)_context.Copy();

        // Act
        var newValue = 20;
        copiedContext.AddOrUpdate("key", ref newValue);

        // Assert
        Assert.That(_context.Get<int>("key"), Is.EqualTo(20));  // Original context should have updated value
        Assert.That(copiedContext.Get<int>("key"), Is.EqualTo(20));
    }
    
    [Test, Category(nameof(IContext.Copy))]
    public void Copy_WhenNewEntriesAreAdded_shouldNotReflectOnOriginal()
    {
        // Arrange
        var intValue = 10;
        _context.AddOrUpdate("key", ref intValue);
        var copiedContext = (Context)_context.Copy();

        // Act
        var newValue = 20;
        copiedContext.AddOrUpdate("anotherKey", ref newValue);

        // Assert
        Assert.That(_context.Get<int>("key"), Is.EqualTo(10));
        Assert.Throws<Exception>(() => _context.Get<int>("anotherKey")); // new keys should not be added to original context
        Assert.That(copiedContext.Get<int>("anotherKey"), Is.EqualTo(20));
    }
    

    [Test, Category(nameof(IContext.Copy))]
    public void Copy_ShouldRetainReferenceForComplexObjects()
    {
        // Arrange
        var complexObject = new GameModule("AnId", "AName", GameModuleType.Player, Array.Empty<string>());
        _context.AddOrUpdate("complexKey", ref complexObject);
        var copiedContext = (Context)_context.Copy();

        // Act
        var copiedComplexObject = copiedContext.Get<GameModule>("complexKey");
        copiedComplexObject.Name = "AnotherName";  // Modify the copied complex object

        // Assert
        Assert.That(_context.Get<GameModule>("complexKey").Name, Is.EqualTo("AnotherName"));  // Should reflect in original context
    }

    #endregion
}