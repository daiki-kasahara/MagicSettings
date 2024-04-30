using ProcessManager.PipeMessage;

namespace ProcessManager;

public class ResponseMessageTest
{
    [Fact]
    public void SerializeTest()
    {
        // Arrange

        // Act
        var message = new ResponseMessage();
        string messageString = string.Empty;
        var exception = Record.Exception(() => messageString = message.Serialize());

        // Assert
        Assert.Null(exception);
        Assert.NotEmpty(messageString);
    }

    [Fact]
    public void DeserializeTest()
    {
        // Arrange
        var jsonString = """
            {
            "ReturnCode": 0,
            "ReturnParameters": "parameters"
            }
            """;

        // Act
        var message = ResponseMessage.Deserialize(jsonString);

        // Assert
        Assert.NotNull(message);
    }

    [Fact]
    public void DeserializeTest_StringEmpty()
    {
        // Arrange
        var jsonString = "";

        // Act
        var message = ResponseMessage.Deserialize(jsonString);

        // Assert
        Assert.Null(message);
    }

    [Fact]
    public void DeserializeTest_Failed()
    {
        // Arrange
        var jsonString = "InvalidJson";

        // Act
        var message = ResponseMessage.Deserialize(jsonString);

        // Assert
        Assert.Null(message);
    }

    [Fact]
    public void Result()
    {
        // Arrange

        // Act
        var message = new ResponseMessage();
        message.ReturnCode = 0;

        // Assert
        Assert.True(message.Result);
    }
}
