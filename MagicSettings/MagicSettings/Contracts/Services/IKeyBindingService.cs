using System.Threading.Tasks;

namespace MagicSettings.Contracts.Services;

internal interface IKeyBindingService
{
    public Task<bool> SetEnabledKeyBindingAsync(bool isEnabled);
}
