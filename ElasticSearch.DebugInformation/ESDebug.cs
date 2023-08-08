using System.Text;

namespace ElasticSearch.DebugInformation;

public static class ESDebug
{
    public static DebugInformationElasticSearch Structurize(string debuginformation)
    {
        int requestWordLenght = "# Request:".Length + 2;
        int responseWordLenght = "# Response:".Length + 2;

        var reqIndex = debuginformation.IndexOf("# Request:");
        var resIndex = debuginformation.IndexOf("# Response:");
        var lenghtResponse = resIndex - reqIndex;

        var requestText = debuginformation.Substring(reqIndex + requestWordLenght,
            lenghtResponse - responseWordLenght);

        var responseText = debuginformation.Substring(resIndex + responseWordLenght);

        var statusText = debuginformation.Substring(0, reqIndex);

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
        Request = request;
        Response = response;
        Status = status;

        if (!string.IsNullOrWhiteSpace(Request))
            RequestBeautify = JsonUtil.Beautify(Request);

        if (!string.IsNullOrWhiteSpace(Response))
            ResponseBeautify = JsonUtil.Beautify(Response);
    }

    public string Request { get; set; }

    public string RequestBeautify { get; set; }

    public string Response { get; set; }

    public string ResponseBeautify { get; set; }

    public string Status { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(Status);
        sb.AppendLine("# Request:");
        sb.AppendLine(RequestBeautify);
        sb.AppendLine("# Response:");
        sb.AppendLine(ResponseBeautify);

        return sb.ToString();
    }

    public string ToStringFlat()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(Status);
        sb.AppendLine("# Request:");
        sb.AppendLine(Request);
        sb.AppendLine("# Response:");
        sb.AppendLine(Response);

        return sb.ToString();
    }

    public string ToStringWithoutResponse()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(Status);
        sb.AppendLine("# Request:");
        sb.AppendLine(RequestBeautify);

        return sb.ToString();
    }

    public string ToStringFlatWithoutResponse()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(Status);
        sb.AppendLine("# Request:");
        sb.AppendLine(Request);

        return sb.ToString();
    }
}
