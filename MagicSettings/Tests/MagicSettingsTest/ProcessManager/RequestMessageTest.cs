using ProcessManager.PipeMessage;

namespace ProcessManager;

public class RequestMessageTest
{
    [Fact]
    public void SerializeTest()
    {
        // Arrange

        // Act
        var message = new RequestMessage("Cmd");
        string messageString = string.Empty;
        var exception = Record.Exception(() => messageString = message.Serialize());

        // Assert
        Assert.Null(exception);
        Assert.NotEmpty(messageString);
    }
}
