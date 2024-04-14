using System;
using System.Threading.Tasks;
using MagicSettings.Contracts.Services;
using MagicSettings.Domains;

namespace MagicSettings.Services;

internal class ScreenService : IScreenService
{
    public Task<BlueLightBlocking> GetBlueLightBlocking()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetBlueLightBlocking(BlueLightBlocking value)
    {
        throw new NotImplementedException();
    }
}
