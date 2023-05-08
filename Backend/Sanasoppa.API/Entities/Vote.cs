// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Entities;
public class Vote
{
    public int Id { get; set; }
    public int RoundId { get; set; }
    public int PlayerId { get; set; }
    public int ExplanationId { get; set; }

    public Round Round { get; set; } = default!;
    public Player Player { get; set; } = default!;
    public Explanation Explanation { get; set; } = default!;
}
