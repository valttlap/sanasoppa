using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// It takes an AppUser object and returns a token for user.
        /// </summary>
        /// <param name="AppUser">The user object that we want to create a token for.</param>
        Task<string> CreateToken(AppUser user);
    }
}
