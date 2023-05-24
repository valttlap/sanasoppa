// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces;

public interface IRoundRepository
{
    /// <summary>
    /// Adds an explanation to the round
    /// </summary>
    /// <param name="roundId">The id of the round that the explanation is for.</param>
    /// <param name="explanation">The explanation object that you want to add to the round.</param>
    /// <returns></returns>
    Task AddExplanationAsync(int roundId, Explanation explanation);
    /// <summary>
    /// > Adds an explanation to the round
    /// </summary>
    /// <param name="round">The round that the explanation is for.</param>
    /// <param name="explanation">The explanation object that you want to add to the round.</param>
    /// <returns></returns>
    void AddExplanation(Round round, Explanation explanation);
    Task<Round?> GetRoundWithExplanationsAsync(int roundId);
    /// <summary>
    /// > Adds a round to the game
    /// </summary>
    /// <param name="round">The round object that you want to add to the database.</param>
    /// <returns></returns>
    void AddRound(Round round);
    void Update(Round round);
}
