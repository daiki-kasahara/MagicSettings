using KeyBindingListener.Contracts;
using MagicSettings.Domains;

namespace KeyBindingListener.Factories;

public class ActionFactoryTest
{
    [Theory]
    [InlineData(KeyboardActionType.StartProgram)]
    [InlineData(KeyboardActionType.OpenUrl)]
    [InlineData(KeyboardActionType.ScreenCapture)]
    [InlineData(KeyboardActionType.SnippingTool)]
    [InlineData(KeyboardActionType.MicrosoftStore)]
    [InlineData(KeyboardActionType.Settings)]
    [InlineData(KeyboardActionType.SettingsBluetoothDevice)]
    [InlineData(KeyboardActionType.SettingsCamera)]
    [InlineData(KeyboardActionType.SettingsMouse)]
    [InlineData(KeyboardActionType.SettingsKeyboard)]
    [InlineData(KeyboardActionType.SettingsNetwork)]
    [InlineData(KeyboardActionType.SettingsWifi)]
    [InlineData(KeyboardActionType.SettingsSystemInfo)]
    [InlineData(KeyboardActionType.SettingsSystemAppVolume)]
    [InlineData(KeyboardActionType.SettingsSystemBatterySaver)]
    [InlineData(KeyboardActionType.SettingsSystemBatterySaverSettings)]
    [InlineData(KeyboardActionType.SettingsSystemBatteryUse)]
    [InlineData(KeyboardActionType.SettingsSystemDisplay)]
    [InlineData(KeyboardActionType.SettingsSystemFocusAssist)]
    [InlineData(KeyboardActionType.SettingsSystemNightMode)]
    [InlineData(KeyboardActionType.SettingsSystemNotification)]
    [InlineData(KeyboardActionType.SettingsSystemSound)]
    [InlineData(KeyboardActionType.SettingsSystemSoundDevice)]
    [InlineData(KeyboardActionType.SettingsSystemStorage)]
    public void Create(KeyboardActionType actionType)
    {
        // Arrange

        // Action
        var factory = new ActionFactory();
        IAction action = factory.Create(actionType, string.Empty);
        var exception = Record.Exception(() =>
        {
            action = factory.Create(actionType, string.Empty);
        });

        // Assert
        Assert.Null(exception);
        Assert.NotNull(action);
    }
}
