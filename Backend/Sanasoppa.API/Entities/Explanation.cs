// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Entities;
public class Explanation
{
    public int Id { get; set; }
    public int RoundId { get; set; }
    public int PlayerId { get; set; }
    public string Text { get; set; } = default!;
    public bool IsRight { get; set; }

    public Round Round { get; set; } = default!;
    public Player Player { get; set; } = default!;
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
