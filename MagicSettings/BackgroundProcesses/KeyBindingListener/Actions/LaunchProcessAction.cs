using System.Diagnostics;
using System.IO;
using KeyBindingListener.Contracts;

namespace KeyBindingListener.Actions;

/// <summary>
/// プロセスを実行するアクション
/// </summary>
/// <param name="fileName">プロセスのファイルパス</param>
/// <param name="isUri">ファイルパスがUriかどうか</param>
public class LaunchProcessAction(string fileName, bool isUri) : IAction
{
    private readonly string _fileName = fileName;

    private readonly bool _isUri = isUri;

    /// <summary>
    /// アクション
    /// </summary>
    public void Action()
    {
        try
        {
            if (!_isUri && !File.Exists(_fileName))
                return;

            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = _fileName,
                WorkingDirectory = _isUri ? string.Empty : Directory.GetParent(_fileName)?.FullName
            });
        }
        catch (Exception)
        {
            return;
        }
    }
}
