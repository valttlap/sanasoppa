// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces;

public interface IVoteRepository
{
    Task<IEnumerable<Vote>> GetRoundVotesAsync(int roundId);
    Task<IEnumerable<Vote>> GetRoundVotesAsync(Round round);

    void AddVote(Vote vote);
    void UpdateVote(Vote vote);
}
