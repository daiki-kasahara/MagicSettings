using KeyBindingListener.Actions;
using KeyBindingListener.Contracts;
using MagicSettings.Domains;

namespace KeyBindingListener.Factories;

/// <summary>
/// アクションインスタンスを生成するファクトリ
/// </summary>
public class ActionFactory : IActionFactory
{
    public IAction Create(KeyboardActionType actionType, string fileName)
    {
        return actionType switch
        {
            KeyboardActionType.StartProgram => new LaunchProcessAction(fileName, false),
            KeyboardActionType.OpenUrl => new LaunchProcessAction(fileName, true),
            KeyboardActionType.ScreenCapture => new LaunchProcessAction("ms-screenclip:", true),
            KeyboardActionType.SnippingTool => new LaunchProcessAction("ms-screensketch:", true),
            KeyboardActionType.MicrosoftStore => new LaunchProcessAction("ms-windows-store:", true),
            KeyboardActionType.Settings => new LaunchProcessAction("ms-settings:", true),
            KeyboardActionType.SettingsBluetoothDevice => new LaunchProcessAction("ms-settings:bluetooth", true),
            KeyboardActionType.SettingsCamera => new LaunchProcessAction("ms-settings:camera", true),
            KeyboardActionType.SettingsMouse => new LaunchProcessAction("ms-settings:mousetouchpad", true),
            KeyboardActionType.SettingsKeyboard => new LaunchProcessAction("ms-settings:easeofaccess-keyboard", true),
            KeyboardActionType.SettingsNetwork => new LaunchProcessAction("ms-settings:network-status", true),
            KeyboardActionType.SettingsWifi => new LaunchProcessAction("ms-settings:network-wifi", true),
            KeyboardActionType.SettingsSystemInfo => new LaunchProcessAction("ms-settings:about", true),
            KeyboardActionType.SettingsSystemAppVolume => new LaunchProcessAction("ms-settings:apps-volume", true),
            KeyboardActionType.SettingsSystemBatterySaver => new LaunchProcessAction("ms-settings:batterysaver", true),
            KeyboardActionType.SettingsSystemBatterySaverSettings => new LaunchProcessAction("ms-settings:batterysaver-settings", true),
            KeyboardActionType.SettingsSystemBatteryUse => new LaunchProcessAction("ms-settings:batterysaver-usagedetails", true),
            KeyboardActionType.SettingsSystemDisplay => new LaunchProcessAction("ms-settings:display", true),
            KeyboardActionType.SettingsSystemFocusAssist => new LaunchProcessAction("ms-settings:quiethours", true),
            KeyboardActionType.SettingsSystemNightMode => new LaunchProcessAction("ms-settings:nightlight", true),
            KeyboardActionType.SettingsSystemNotification => new LaunchProcessAction("ms-settings:notifications", true),
            KeyboardActionType.SettingsSystemSound => new LaunchProcessAction("ms-settings:sound", true),
            KeyboardActionType.SettingsSystemSoundDevice => new LaunchProcessAction("ms-settings:sound-devices", true),
            KeyboardActionType.SettingsSystemStorage => new LaunchProcessAction("ms-settings:storagesense", true),
            _ => throw new NotImplementedException(),
        };
    }
}
