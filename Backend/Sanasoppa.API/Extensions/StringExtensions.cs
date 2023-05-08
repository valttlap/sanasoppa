// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

namespace Sanasoppa.API.Extensions;

public static class StringExtensions
{
    public static string Sanitize(this string str)
    {
        var pattern = @"[^\w\s.,:;+\-/&'öÖäÄåÅ]";
        var sanitized = Regex.Replace(str, pattern, "", RegexOptions.NonBacktracking);
        return sanitized;
    }
}
