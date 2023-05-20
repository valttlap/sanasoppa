// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Services;

public class Auth0Service : IAuth0Service
{
    private readonly IManagementApiClient _managementApiClient;
    public Auth0Service(IManagementApiClient managementApiClient)
    {
        _managementApiClient = managementApiClient;
    }

    public async Task<IPagedList<User>> GetPagedUsersAsync(int pageNumber, int resultsPerPage)
    {
        var paginationInfo = new PaginationInfo(pageNumber, resultsPerPage, true);
        var request = new GetUsersRequest
        {
        };

        return await _managementApiClient.Users.GetAllAsync(request, paginationInfo).ConfigureAwait(false);
    }

    public async Task<User> GetUserAsync(string userId)
    {
        return await _managementApiClient.Users.GetAsync(userId).ConfigureAwait(false);
    }

    public async Task<IPagedList<Role>> GetUserRolesAsync(string userId)
    {
        return await _managementApiClient.Users.GetRolesAsync(userId).ConfigureAwait(false);
    }

    public async Task<IPagedList<User>> GetUsersAsync()
    {
        var request = new GetUsersRequest
        {
            Fields = "",
            IncludeFields = true
        };

        return await _managementApiClient.Users.GetAllAsync(request).ConfigureAwait(false);
    }

    public async Task<IPagedList<Role>> GetRolesAsync()
    {
        var req = new GetRolesRequest
        {
            NameFilter = ""
        };
        return await _managementApiClient.Roles.GetAllAsync(req).ConfigureAwait(false);
    }

    public async Task AssignRolesToUserAsync(string userId, AssignRolesRequest roles)
    {
        await _managementApiClient.Users.AssignRolesAsync(userId, roles).ConfigureAwait(false);
    }

    public async Task RemoveRolesFromUserAsync(string userId, AssignRolesRequest roles)
    {
        await _managementApiClient.Users.RemoveRolesAsync(userId, roles).ConfigureAwait(false);
    }

    public async Task<User> UpdateUserNameAsync(string userId, string name)
    {
        var req = new UserUpdateRequest
        {
            NickName = name
        };
        return await _managementApiClient.Users.UpdateAsync(userId, req).ConfigureAwait(false);
    }

    public async Task<User> UpdateUserAsync(string userId, UserUpdateRequest userUpdateRequest)
    {
        return await _managementApiClient.Users.UpdateAsync(userId, userUpdateRequest).ConfigureAwait(false);
    }

    public async Task<User> ConfirmUserEmailAsync(string userId)
    {
        var req = new UserUpdateRequest
        {
            EmailVerified = true
        };
        return await _managementApiClient.Users.UpdateAsync(userId, req).ConfigureAwait(false);

    }

    public async Task DeletUserAsync(string userId)
    {
        await _managementApiClient.Users.DeleteAsync(userId).ConfigureAwait(false);
    }

    public async Task<Job> ResendUserVerificationEmailAsync(string userId)
    {
        var req = new VerifyEmailJobRequest
        {
            UserId = userId
        };
        return await _managementApiClient.Jobs.SendVerificationEmailAsync(req).ConfigureAwait(false);
    }
}
