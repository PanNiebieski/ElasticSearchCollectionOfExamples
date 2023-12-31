﻿using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ElasticSearch.DebugInformation;

public static class JsonUtil
{
    public static string Beautify(string json)
    {
        if (!IsJson(json))
            return string.Empty;

        const int indentWidth = 4;
        const string pattern = "(?>([{\\[][}\\]],?)|([{\\[])|([}\\]],?)|([^{}:]+:)([^{}\\[\\],]*(?>([{\\[])|,)?)|([^{}\\[\\],]+,?))";

        var match = Regex.Match(json, pattern);
        var beautified = new StringBuilder();
        var indent = 0;
        while (match.Success)
        {
            if (match.Groups[3].Length > 0)
                indent--;

            beautified.AppendLine(
                new string(' ', indent * indentWidth) +
                (match.Groups[4].Length > 0
                    ? match.Groups[4].Value + " " + match.Groups[5].Value
                    : (match.Groups[7].Length > 0 ? match.Groups[7].Value : match.Value))
            );

            if (match.Groups[2].Length > 0 || match.Groups[6].Length > 0)
                indent++;

            match = match.NextMatch();
        }

        return beautified.ToString();
    }

    public static bool IsJson(this string source)
    {
        if (source == null)
            return false;

        try
        {
            JsonDocument.Parse(source);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}