// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
namespace Sanasoppa.API.Interfaces
{
    public interface IAuth0Service
    {
        public Task<IPagedList<User>> GetUsersAsync();
        public Task<IPagedList<User>> GetPagedUsersAsync(int pageNumber, int resultsPerPage);
        public Task<User> GetUserAsync(string userId);
        public Task<IPagedList<Role>> GetUserRolesAsync(string userId);
        public Task<IPagedList<Role>> GetRolesAsync();
        public Task AssignRolesToUserAsync(string userId, AssignRolesRequest roles);
        public Task RemoveRolesFromUserAsync(string userId, AssignRolesRequest roles);
        public Task<User> UpdateUserNameAsync(string userId, string name);
        public Task<User> UpdateUserAsync(string userId, UserUpdateRequest userUpdateRequest);
        public Task<User> ConfirmUserEmailAsync(string userId);
        public Task DeletUserAsync(string userId);
        public Task<Job> ResendUserVerificationEmailAsync(string userId);
    }
}
