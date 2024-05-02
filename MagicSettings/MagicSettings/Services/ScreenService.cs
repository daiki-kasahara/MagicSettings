using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;
using MagicSettings.Models.SettingsFile;
using MagicSettings.Repositories.Contracts;
using ProcessManager;
using ProcessManager.Contracts;
using ProcessManager.PipeMessage;

namespace MagicSettings.Services;

/// <summary>
/// スクリーンの設定をするサービス
/// </summary>
/// <param name="repository"></param>
/// <param name="controller"></param>
internal class ScreenService(IScreenRepository repository, IProcessController controller) : IScreenService
{
    private readonly IScreenRepository _screenRepository = repository;
    private readonly IProcessController _controller = controller;

    /// <summary>
    /// スクリーンの設定情報を取得する
    /// </summary>
    /// <returns></returns>
    public async Task<ScreenSettings> GetScreenSettingsAsync() => await _screenRepository.GetAsync();

    /// <summary>
    /// 軽減率の設定をする
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public async Task<bool> SetBlueLightBlockingAsync(BlueLightBlocking value)
    {
        await _screenRepository.SaveAsync(value);
        return await _controller.SendMessageAsync(MyProcesses.ScreenController, new RequestMessage("Update"));
    }

    /// <summary>
    /// ブルーライトカットの有効無効を設定する
    /// </summary>
    /// <param name="isEnabled"></param>
    /// <returns></returns>
    public async Task<bool> SetEnabledBlueLightBlockingAsync(bool isEnabled)
    {
        await _screenRepository.SaveAsync(isEnabled);
        return isEnabled ? await _controller.LaunchAsync(MyProcesses.ScreenController) :
            await _controller.TerminateAsync(MyProcesses.ScreenController);
    }

    /// <summary>
    /// 設定を更新する
    /// </summary>
    /// <returns></returns>
    public async Task UpdateSettingAsync() => await _screenRepository.SaveAsync(_controller.IsExistsProcess(MyProcesses.ScreenController));
}
