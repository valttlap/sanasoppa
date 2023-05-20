// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces;

public interface IExplanationRepository
{
    public Task<Explanation?> GetExplanationAsync(int explanationId);
    public Task<Explanation?> GetExplanationAsync(int playerId, int roundId);
    public Task<IEnumerable<Explanation>> GetRoundExplanationsWithVotesAsync(int roundId);
    public Task RemoveExplanationAsync(int playerId, int roundId);
    public void AddExplanation(Explanation explanation);
    public void Update(Explanation explanation);
}
