﻿using System.Diagnostics;
using Moq;
using ProcessManager.Contracts;
using ProcessManager.Internal;
using ProcessManager.PipeMessage;

namespace ProcessManager;

public class ProcessControllerTest : IDisposable
{
    public void Dispose()
    {
        System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName("MagicSettings.KeyBindingListener");

        foreach (System.Diagnostics.Process p in ps)
        {
            try
            {
                p.Kill();
            }
            catch (Exception)
            {
            }
        }
    }

    [Fact]
    public void ProcessController_PublicConstructor()
    {
        // Arrange

        // Act
        var exception = Record.Exception(() => new ProcessController());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void IsExistsProcessTest()
    {
        // Arrange
        using var proc = new Process();
        proc.StartInfo = new ProcessStartInfo()
        {
            FileName = Path.Combine(AppContext.BaseDirectory, $"MagicSettings.KeyBindingListener.exe"),
            UseShellExecute = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            WorkingDirectory = AppContext.BaseDirectory,
        };
        proc.Start();

        // Act
        var controller = new ProcessController(new ClientPipe());

        var actual = controller.IsExistsProcess(MyProcesses.KeyBindingListener);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsExistsProcessTest_NotExists()
    {
        // Arrange

        // Act
        var controller = new ProcessController(new ClientPipe());

        var actual = controller.IsExistsProcess(MyProcesses.KeyBindingListener);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public async Task LaunchAsyncTest()
    {
        // Arrange
        var process = MyProcesses.KeyBindingListener;
        var clientStub = new Mock<IClientPipe>();
        clientStub.Setup(x => x.CheckExistedMessageAsync(process)).ReturnsAsync(true);

        // Act
        var controller = new ProcessController(clientStub.Object);
        var actual = await controller.LaunchAsync(process);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task SendMessageAsyncTest()
    {
        // Arrange
        var process = MyProcesses.KeyBindingListener;
        var clientStub = new Mock<IClientPipe>();
        clientStub.Setup(x => x.SendRequestMessageAsync(process, It.IsAny<RequestMessage>())).ReturnsAsync(true);

        // Act
        var controller = new ProcessController(clientStub.Object);
        var actual = await controller.SendMessageAsync(process, new RequestMessage(""));

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task SendMessageAsyncTest_Failed()
    {
        // Arrange
        var process = MyProcesses.KeyBindingListener;
        var clientStub = new Mock<IClientPipe>();
        clientStub.Setup(x => x.SendRequestMessageAsync(process, It.IsAny<RequestMessage>())).ReturnsAsync(false);

        // Act
        var controller = new ProcessController(clientStub.Object);
        var actual = await controller.SendMessageAsync(process, new RequestMessage(""));

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public async Task TerminateAsyncTest()
    {
        // Arrange
        var process = MyProcesses.KeyBindingListener;
        var clientStub = new Mock<IClientPipe>();
        clientStub.Setup(x => x.SendTerminateMessageAsync(process)).ReturnsAsync(true);

        // Act
        var controller = new ProcessController(clientStub.Object);
        var actual = await controller.TerminateAsync(process);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task TerminateAsyncTest_Failed()
    {
        // Arrange
        var process = MyProcesses.KeyBindingListener;
        var clientStub = new Mock<IClientPipe>();
        clientStub.Setup(x => x.SendTerminateMessageAsync(process)).ReturnsAsync(false);

        // Act
        var controller = new ProcessController(clientStub.Object);
        var actual = await controller.TerminateAsync(process);

        // Assert
        Assert.False(actual);
    }
}
