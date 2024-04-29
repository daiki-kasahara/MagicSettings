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
        service.OnKeyUp(null, new KeyboardHookEventArgs() { Key = VKeys.LeftWindows });
        service.OnKeyUp(null, new KeyboardHookEventArgs() { Key = VKeys.LeftMenu });
    }

    // 代表のキーでいくつかチェックする
    [Theory]
    [InlineData(VKeys.A, VKeys.LeftWindows, VKeys.LeftMenu)]
    [InlineData(VKeys.B, VKeys.RightWindows, VKeys.LeftMenu)]
    [InlineData(VKeys.Y, VKeys.RightWindows, VKeys.RightMenu)]
    [InlineData(VKeys.Z, VKeys.LeftWindows, VKeys.RightMenu)]
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
    [InlineData(VKeys.LeftWindows)]
    [InlineData(VKeys.RightWindows)]
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
    [InlineData(VKeys.LeftMenu)]
    [InlineData(VKeys.RightMenu)]
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
    [InlineData(VKeys.LeftMenu)]
    [InlineData(VKeys.RightMenu)]
    public void OnKeyUpTest_WithAlt(VKeys key)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardActionService>();
        serviceStub.Setup(x => x.ActionAsync(It.IsAny<VKeys>()));

        // Act
        var service = new KeyHookService(serviceStub.Object);
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = VKeys.RightWindows });
        service.OnKeyUp(null, new KeyboardHookEventArgs() { Key = key });
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = VKeys.A });

        // Assert
        // KeyUpしたときは処理が実行されないこと
        serviceStub.Verify(x => x.ActionAsync(It.IsAny<VKeys>()), Times.Never);
    }

    [Theory]
    [InlineData(VKeys.LeftWindows)]
    [InlineData(VKeys.RightWindows)]
    public void OnKeyUpTest_WithWin(VKeys key)
    {
        // Arrange
        var serviceStub = new Mock<IKeyboardActionService>();
        serviceStub.Setup(x => x.ActionAsync(It.IsAny<VKeys>()));

        // Act
        var service = new KeyHookService(serviceStub.Object);
        service.OnKeyDown(null, new KeyboardHookEventArgs() { Key = VKeys.RightMenu });
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
