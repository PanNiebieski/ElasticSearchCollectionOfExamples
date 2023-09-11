namespace ElasticSearchCRUDExampleApi.ElasticSearch;

public class ElasticSearchOptions
{
    public ElasticSearchOptions()
    {
        Connection = new ConnectionOptions();
        Debug = new DebugOptions();
        Index = new IndexOptions();
    }

    public const string SectionName = "elasticsearch";

    public IndexOptions Index { get; set; }

    public DebugOptions Debug { get; set; }

    public ConnectionOptions Connection { get; set; }

    public class ConnectionOptions
    {
        public bool AuthRequired { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string AddressEndPoint { get; set; }

        public string Protocol { get; set; }

        public bool DisableAutoProxyDetection { get; set; }

        public int RequestTimeout { get; set; }

        public bool EnableHttpCompression { get; set; }
    }

    public class IndexOptions
    {
        public string Name { get; set; }

        public int NumberOfShards { get; set; }

        public int NumberOfReplicas { get; set; }

        public int RefreshInterval { get; set; }

    }

    public class DebugOptions
    {
        public bool EnableDebugMode { get; set; }

        public bool PrettyJson { get; set; }

        public bool DisableDirectStreaming { get; set; }
    }
}


//public class BulkOptions
//{
//    public bool ContinueAfterDroppedDocuments { get; set; }

//    public int BackOffRetries { get; set; }

//    public int BackOffTimeFromSeconds { get; set; }

//    public int MaxDegreeOfParallelism { get; set; }
//}

