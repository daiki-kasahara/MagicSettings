using System.Diagnostics;
using Moq;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace ProcessManager;

public class ServerPipeTest
{
    [Fact]
    public void ServerPipe_PublicConstructor()
    {
        // Arrange

        // Act
        var exception = Record.Exception(() => new ServerPipe(MyProcesses.KeyBindingListener));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void OpenPipeTest()
    {
        // Arrange
        var clientStub = new Mock<IClientPipe>();

        // Act
        var pipe = new ServerPipe(MyProcesses.KeyBindingListener, clientStub.Object);
        pipe.OnAction += (requestMessage) => Debug.WriteLine(requestMessage);
        var actual = pipe.OpenPipe();

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void OpenPipeTest_TaskIsNotNull()
    {
        // Arrange
        var clientStub = new Mock<IClientPipe>();

        // Act
        var pipe = new ServerPipe(MyProcesses.KeyBindingListener, clientStub.Object);
        var actual = pipe.OpenPipe();
        var actualTaskIsNotNull = pipe.OpenPipe();

        // Assert
        Assert.True(actual);
        Assert.True(actualTaskIsNotNull);
    }

    [Fact]
    public void ClosePipeTest()
    {
        // Arrange
        var clientStub = new Mock<IClientPipe>();
        clientStub.Setup(x => x.SendRequestMessageAsync(MyProcesses.KeyBindingListener, It.IsAny<RequestMessage>())).ReturnsAsync(true);

        // Act
        var pipe = new ServerPipe(MyProcesses.KeyBindingListener, clientStub.Object);
        pipe.OnAction += (requestMessage) => Debug.WriteLine(requestMessage);
        var _ = pipe.OpenPipe();
        var actual = pipe.ClosePipe();

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void ClosePipeTest_Failed()
    {
        // Arrange
        var clientStub = new Mock<IClientPipe>();
        clientStub.Setup(x => x.SendRequestMessageAsync(MyProcesses.KeyBindingListener, It.IsAny<RequestMessage>())).ReturnsAsync(false);

        // Act
        var pipe = new ServerPipe(MyProcesses.KeyBindingListener, clientStub.Object);
        var _ = pipe.OpenPipe();
        var actual = pipe.ClosePipe();

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void ClosePipeTest_TaskIsNull()
    {
        // Arrange
        var clientStub = new Mock<IClientPipe>();
        clientStub.Setup(x => x.SendRequestMessageAsync(MyProcesses.KeyBindingListener, It.IsAny<RequestMessage>())).ReturnsAsync(true);

        // Act
        var pipe = new ServerPipe(MyProcesses.KeyBindingListener, clientStub.Object);
        var actual = pipe.ClosePipe();

        // Assert
        Assert.True(actual);
    }
}
