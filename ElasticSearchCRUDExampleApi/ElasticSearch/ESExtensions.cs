using Elasticsearch.Net;
using ElasticSearchCRUDExampleApi.Enties;
using Nest;
using System.Text;

namespace ElasticSearchCRUDExampleApi.ElasticSearch;

public static class ESExtensions
{
    public static string GetErrorDetails(this ResponseBase response)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("DebugInformation");
        sb.AppendLine(response.DebugInformation);
        sb.AppendLine("ServerError");
        sb.AppendLine(response.ServerError.TryToStringGetServerError());
        sb.AppendLine("ApiCall");
        sb.AppendLine(response.ApiCall.TryToStringGetApiCall());
        sb.AppendLine("OrginalException");
        sb.AppendLine(response.OriginalException.TryToStringOrginalException());

        return sb.ToString();
    }

    public static string GetErrorDetails(this ISearchResponse<GameMission> response)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("DebugInformation");
        sb.AppendLine(response.DebugInformation);
        sb.AppendLine("ServerError");
        sb.AppendLine(response.ServerError.TryToStringGetServerError());
        sb.AppendLine("ApiCall");
        sb.AppendLine(response.ApiCall.TryToStringGetApiCall());
        sb.AppendLine("OrginalException");
        sb.AppendLine(response.OriginalException.TryToStringOrginalException());

        return sb.ToString();
    }

    public static string? TryToStringGetApiCall(this IApiCallDetails apiCallDetails)
    {
        if (apiCallDetails == null)
            return string.Empty;

        return apiCallDetails.ToString();
    }

    public static string? TryToStringGetServerError(this ServerError serverError)
    {
        if (serverError == null)
            return string.Empty;

        return serverError.ToString();
    }

    public static string? TryToStringOrginalException(this Exception exception)
    {
        if (exception == null)
            return string.Empty;

        return exception.ToString();
    }
}
