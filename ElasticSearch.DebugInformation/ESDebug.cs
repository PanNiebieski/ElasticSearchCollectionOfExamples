using System.Text;

namespace ElasticSearch.DebugInformation;

public static class ESDebug
{
    public static DebugInformationElasticSearch Structurize(string debuginformation)
    {
        int requestWordLenght = "# Request:".Length;
        int responseWordLenght = "# Response:".Length;

        var reqIndex = debuginformation.IndexOf("# Request:");
        var resIndex = debuginformation.IndexOf("# Response:");
        var lenghtResponse = resIndex - reqIndex;

        if ((resIndex <= reqIndex) || (reqIndex == -1 && resIndex == -1))
            return new DebugInformationElasticSearch("", "", debuginformation);

        var requestText = string.Empty;

        if (reqIndex != -1 && lenghtResponse > responseWordLenght)
            requestText = debuginformation.Substring
                (reqIndex + requestWordLenght,
                    lenghtResponse - responseWordLenght);

        var responseText = string.Empty;

        if (resIndex != -1)
            responseText = debuginformation.Substring
                (resIndex + responseWordLenght);

        var statusText = string.Empty;

        if (reqIndex != -1)
            statusText = debuginformation.Substring(0, reqIndex);

        return new DebugInformationElasticSearch
            (requestText, responseText, statusText);
    }

    public static void ConsoleWriteAll(string debuginformation)
    {
        Console.WriteLine(Structurize(debuginformation));
    }

    public static void ConsoleWriteFlatAll(string debuginformation)
    {
        Console.WriteLine(debuginformation);
    }

    public static void ConsoleWriteFlatWithoutResponse(string debuginformation)
    {
        var s = Structurize(debuginformation);
        Console.WriteLine(s.ToStringFlatWithoutResponse());
    }

    public static void ConsoleWriteWithoutResponse(string debuginformation)
    {
        var s = Structurize(debuginformation);
        Console.WriteLine(s.ToStringWithoutResponse());
    }
}

public class DebugInformationElasticSearch
{
    public DebugInformationElasticSearch(string request,
        string response,
        string status)
    {
        Request = request.Trim();
        Response = response.Trim();
        FullStatus = status;

        SetCall(status);

        if (!string.IsNullOrWhiteSpace(Request))
        {
            RequestBeautify = JsonUtil.Beautify(Request);
            RequestBeautifyWithCall = $"{Call}\n{RequestBeautify}";
        }

        if (!string.IsNullOrWhiteSpace(Response))
            ResponseBeautify = JsonUtil.Beautify(Response);

        SetTookTimeSpan(status);
        SetNode(status);
    }

    private void SetCall(string status)
    {
        var word = "low level call on ";
        var wordLenght = word.Length;
        var start = status.IndexOf(word);
        var end = status.IndexOf("#");

        if (start > 0 && end > 0 && end > start)
        {
            var callw = FullStatus.Substring(start + wordLenght,
                end - (start + wordLenght));
            Call = callw.Trim().Replace(":", "");
        }
    }

    private void SetNode(string status)
    {
        var word = "Node: ";
        var wordLenght = word.Length;
        var start = status.IndexOf(word);
        var end = status.IndexOf("Took:");

        if (start > 0 && end > 0 && end > start)
        {
            var callw = FullStatus.Substring(start + wordLenght,
                end - (start + wordLenght));
            Node = callw.Trim().Replace(":", " ");
        }
    }

    private void SetTookTimeSpan(string status)
    {
        var wordTook = "Took: ";
        var wordTookLenght = wordTook.Length;
        var startT = status.IndexOf(wordTook);

        if (startT > 0)
        {
            var took = FullStatus.Substring(startT + wordTookLenght)
                .Trim();

            if (TimeSpan.TryParse(took, out var timespan))
                Took = timespan;
        }
    }

    public string Call { get; set; }

    public string Node { get; set; }

    public TimeSpan? Took { get; set; }

    public string Request { get; set; }

    public string RequestBeautify { get; init; }

    public string RequestBeautifyWithCall { get; init; }

    public string Response { get; set; }

    public string ResponseBeautify { get; init; }

    public string FullStatus { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrWhiteSpace(Request))
            return FullStatus;

        if (Node != null)
            sb.AppendLine($"Node : {Node}");

        if (Took != null)
            sb.AppendLine($"Took : {Took}");

        sb.AppendLine("# Request:");
        sb.AppendLine(RequestBeautifyWithCall);
        sb.AppendLine("# Response:");
        sb.AppendLine(ResponseBeautify);

        return sb.ToString();
    }

    public string ToStringFlat()
    {
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrWhiteSpace(Request))
            return FullStatus;

        if (Node != null)
            sb.AppendLine($"Node : {Node}");

        if (Took != null)
            sb.AppendLine($"Took : {Took}");

        sb.AppendLine("# Request:");
        sb.AppendLine(Request);
        sb.AppendLine("# Response:");
        sb.AppendLine(Response);

        return sb.ToString();
    }

    public string ToStringWithoutResponse()
    {
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrWhiteSpace(Request))
            return FullStatus;

        if (Node != null)
            sb.AppendLine($"Node : {Node}");

        if (Took != null)
            sb.AppendLine($"Took : {Took}");

        sb.AppendLine("# Request:");
        sb.AppendLine(RequestBeautifyWithCall);

        return sb.ToString();
    }

    public string ToStringFlatWithoutResponse()
    {
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrWhiteSpace(Request))
            return FullStatus;

        if (Node != null)
            sb.AppendLine($"Node : {Node}");

        if (Took != null)
            sb.AppendLine($"Took : {Took}");

        sb.AppendLine("# Request:");
        sb.AppendLine(Request);

        return sb.ToString();
    }
}