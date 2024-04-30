using KeyBindingListener.Contracts;
using KeyBindingListener.Events;
using MagicSettings.Domains;
using Moq;

namespace KeyBindingListener.Services;

public class KeyHookServiceTest
{
    public KeyHookServiceTest()
    {
        // Setup
        var serviceStub = new Mock<IKeyboardActionService>();
        var service = new KeyHookService(serviceStub.Object);

        // キー押しフラグをリセットする
        service.OnKeyUp(null, new KeyboardHookEventArgs() { Key = VKeys.LWin });
        service.OnKeyUp(null, new KeyboardHookEventArgs() { Key = VKeys.LMenu });
    }

    // 代表のキーでいくつかチェックする
    [Theory]
    [InlineData(VKeys.A, VKeys.LWin, VKeys.LMenu)]
    [InlineData(VKeys.B, VKeys.RWin, VKeys.LMenu)]
    [InlineData(VKeys.W, VKeys.LWin, VKeys.Menu)]
    [InlineData(VKeys.X, VKeys.RWin, VKeys.Menu)]
    [InlineData(VKeys.Y, VKeys.RWin, VKeys.RMenu)]
    [InlineData(VKeys.Z, VKeys.LWin, VKeys.RMenu)]
    public void OnKeyDownTest(VKeys key, VKeys win, VKeys alt)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardActionService>();
        serviceStub.Setup(x => x.ActionAsync(key));

        // Act
        var service = new KeyHookService(serviceStub.Object);
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = win });
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = alt });
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = key });

        // Assert
        serviceStub.Verify(x => x.ActionAsync(key), Times.Once);
    }

    // 代表のキーでいくつかチェックする
    [Theory]
    [InlineData(VKeys.A)]
    [InlineData(VKeys.B)]
    [InlineData(VKeys.Z)]
    public void OnKeyDownTest_NotMatch(VKeys key)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardActionService>();
        serviceStub.Setup(x => x.ActionAsync(It.IsAny<VKeys>()));

        // Act
        var service = new KeyHookService(serviceStub.Object);
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = key });

        // Assert
        // 単押しの場合は処理が実行されないこと
        serviceStub.Verify(x => x.ActionAsync(It.IsAny<VKeys>()), Times.Never);
    }

    [Theory]
    [InlineData(VKeys.LWin)]
    [InlineData(VKeys.RWin)]
    public void OnKeyDownTest_OnlyWinKey(VKeys key)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardActionService>();
        serviceStub.Setup(x => x.ActionAsync(It.IsAny<VKeys>()));

        // Act
        var service = new KeyHookService(serviceStub.Object);
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = key });

        // Assert
        // 単押しの場合は処理が実行されないこと
        serviceStub.Verify(x => x.ActionAsync(It.IsAny<VKeys>()), Times.Never);
    }

    [Theory]
    [InlineData(VKeys.Menu)]
    [InlineData(VKeys.LMenu)]
    [InlineData(VKeys.RMenu)]
    public void OnKeyDownTest_OnlyAltKey(VKeys key)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardActionService>();
        serviceStub.Setup(x => x.ActionAsync(It.IsAny<VKeys>()));

        // Act
        var service = new KeyHookService(serviceStub.Object);
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = key });

        // Assert
        // 単押しの場合は処理が実行されないこと
        serviceStub.Verify(x => x.ActionAsync(It.IsAny<VKeys>()), Times.Never);
    }

    [Theory]
    [InlineData(VKeys.Menu)]
    [InlineData(VKeys.LMenu)]
    [InlineData(VKeys.RMenu)]
    public void OnKeyUpTest_WithAlt(VKeys key)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardActionService>();
        serviceStub.Setup(x => x.ActionAsync(It.IsAny<VKeys>()));

        // Act
        var service = new KeyHookService(serviceStub.Object);
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = VKeys.RWin });
        service.OnKeyUp(null, new KeyboardHookEventArgs() { Key = key });
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = VKeys.A });

        // Assert
        // KeyUpしたときは処理が実行されないこと
        serviceStub.Verify(x => x.ActionAsync(It.IsAny<VKeys>()), Times.Never);
    }

    [Theory]
    [InlineData(VKeys.LWin)]
    [InlineData(VKeys.RWin)]
    public void OnKeyUpTest_WithWin(VKeys key)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardActionService>();
        serviceStub.Setup(x => x.ActionAsync(It.IsAny<VKeys>()));

        // Act
        var service = new KeyHookService(serviceStub.Object);
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = VKeys.RMenu });
        service.OnKeyUp(null, new KeyboardHookEventArgs() { Key = key });
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = VKeys.A });

        // Assert
        // KeyUpしたときは処理が実行されないこと
        serviceStub.Verify(x => x.ActionAsync(It.IsAny<VKeys>()), Times.Never);
    }

    [Theory]
    [InlineData(VKeys.A)]
    [InlineData(VKeys.Z)]
    public void OnKeyUpTest_OtherKey(VKeys key)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardActionService>();
        serviceStub.Setup(x => x.ActionAsync(It.IsAny<VKeys>()));

        // Act
        var service = new KeyHookService(serviceStub.Object);
        var exception = Record.Exception(() => service.OnKeyUp(null, new KeyboardHookEventArgs() { Key = key }));

        // Assert
        Assert.Null(exception);
    }
}
