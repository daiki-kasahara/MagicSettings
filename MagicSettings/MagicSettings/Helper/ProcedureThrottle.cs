using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace MagicSettings.Helper;

/// <summary>
/// 処理の開始タイミングを指定した時間遅延させるヘルパー
/// </summary>
internal class ProcedureThrottle
{
    private DispatcherTimer? _timer;

    /// <summary>
    /// 遅延したい処理をポストする
    /// </summary>
    /// <param name="action">非同期処理</param>
    /// <param name="dueTime">遅延時間</param>
    public void PostAsyncAction(Func<Task> action, TimeSpan dueTime)
    {
        // 実行中ののタスクをキャンセル
        _timer?.Stop();

        _timer = new DispatcherTimer
        {
            Interval = dueTime
        };

        _timer.Tick += async (sender, e) =>
        {
            // タイマーを停止してから処理を実行
            _timer.Stop();
            await action();
        };

        _timer.Start();
    }
}
