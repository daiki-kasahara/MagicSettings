using System.Diagnostics;
using Moq;
using ProcessManager;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace ProcessManagerTest;

public class ServerPipeTest
{
    [Fact]
    public void OpenPipe()
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
    public void OpenPipe_TaskIsNotNull()
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
    public void ClosePipe()
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
    public void ClosePipe_Failed()
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
    public void ClosePipe_TaskIsNull()
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
