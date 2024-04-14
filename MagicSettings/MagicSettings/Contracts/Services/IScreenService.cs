using System.Threading.Tasks;
using MagicSettings.Domains;

namespace MagicSettings.Contracts.Services;

internal interface IScreenService
{
    public Task<bool> SetBlueLightBlocking(BlueLightBlocking value);

    public Task<BlueLightBlocking> GetBlueLightBlocking();
}
