using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
