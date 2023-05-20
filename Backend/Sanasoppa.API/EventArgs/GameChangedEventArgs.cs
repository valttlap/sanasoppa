// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Sanasoppa.API.Entities;

namespace Sanasoppa.API.EventArgs;
public class GameListChangedEventArgs : System.EventArgs
{
    public IEnumerable<Game> Games { get; }

    public GameListChangedEventArgs(IEnumerable<Game> games)
    {
        Games = games;
    }
}
