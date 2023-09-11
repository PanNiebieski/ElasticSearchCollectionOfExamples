using Nest;

namespace ElasticSearchCRUDExampleApi.ElasticSearch;

public class IndexResponse
{
    public bool IsValid { get; set; }

    public bool Acknowledge { get; set; }

    public string DebugInformation { get; set; }

    public IndexResponse()
    {
        IsValid = true; Acknowledge = true;
    }

    public IndexResponse(CreateIndexResponse indexResponse)
    {
        IsValid = indexResponse.IsValid;
        Acknowledge = indexResponse.Acknowledged;
        DebugInformation = indexResponse.DebugInformation;
    }
}
