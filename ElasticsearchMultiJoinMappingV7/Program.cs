
using Nest;
using static ElasticsearchMultiJoinMappingV7.StaticCollections;


_ElasticClient = new ElasticClient(GetConnection());
CreateIndexAndMappings(numberOfReplicas: 0, numberOfShards: 1, refreshInterval: -1);


IndexDocuments(GetProducts());
IndexChildDocuments(GetStocks());
IndexChildDocuments(GetSuppliers());
IndexChildDocuments(GetCategoriees());

await SearchAsync();

await SearchAsync3();

await SearchAsync4();

await SearchAsync2();

static void CreateIndexAndMappings(int numberOfReplicas, int numberOfShards, int refreshInterval)
{
    if (_ElasticClient.Indices.Exists(_IndexName).Exists)
    {
        return;
    }

    var createIndexResult = _ElasticClient.Indices.Create(_IndexName, x => x
                     .Settings(s => s
                                     .NumberOfReplicas(numberOfReplicas)
                                     .RefreshInterval(refreshInterval)
                                     .NumberOfShards(numberOfShards))
                    .Index<BaseDocument>()
                    .Map<BaseDocument>(m => m
                        .RoutingField(r => r.Required())
                        .AutoMap<Product>()
                        .Properties<Product>(props => props
                        //You can describe your properties below for Manual mapping
                        .Text(s => s
                           .Name(n => n.Name)
                           .Analyzer("default")
                           .Fields(pprops => pprops))
                      )
                                            .AutoMap<Category>()
                                            .AutoMap<Supplier>()
                                            .AutoMap<Stock>()
                                            //You can add more join types here
                                            .Properties(props => props
                                                        .Join(j => j
                                                            .Name(p => p.JoinField)
                                                                //This is so important here. You can describe a relation that product is parent and the others is join type.
                                                                .Relations(r => r
                                                                .Join<Product>("category", "supplier", "stock")
                                                                        )
                                                        )
                                                    )
                                            )
                );

    if (!createIndexResult.IsValid || !createIndexResult.Acknowledged)
    {
        throw new Exception("Error on mapping!");
    }
}

static void IndexDocuments(List<BaseDocument> documents)
{
    if (documents != null && documents.Count != 0)
    {
        int lastIndex = (int)Math.Ceiling((decimal)documents.Count / _IndexingDocumentCount);

        for (int i = 1; i <= lastIndex; i++)
        {
            List<BaseDocument> items = documents.Skip(1000 * (i - 1)).Take(_IndexingDocumentCount).ToList();
            var descriptor = new BulkDescriptor();

            if (items != null && items.Count > 0)
                for (int index = 0; index < items.Count; index++)
                {
                    var indexingDocument = items[index];
                    if (indexingDocument != null)
                        descriptor.Index<BaseDocument>(o => o.Document(indexingDocument)
                            .Routing(indexingDocument.Id)
                            .Id(indexingDocument.Id)
                            .Index(_IndexName));
                }

            //You can use BulkAsync if you need
            var response = _ElasticClient.Bulk(descriptor);

            if (!response.IsValid || response.ItemsWithErrors.Any())
            {
                throw new Exception("Error on indexing!");
            }
        }
    }
}

static void IndexChildDocuments(List<BaseDocument> documents)
{
    if (documents != null && documents.Count != 0)
    {
        int lastIndex = (int)Math.Ceiling((decimal)documents.Count / _IndexingDocumentCount);

        for (int i = 1; i <= lastIndex; i++)
        {
            List<BaseDocument> items = documents.Skip(1000 * (i - 1)).Take(_IndexingDocumentCount).ToList();
            var descriptor = new BulkDescriptor();

            if (items != null && items.Count > 0)
                for (int index = 0; index < items.Count; index++)
                {
                    var indexingDocument = items[index];
                    if (indexingDocument != null)
                        descriptor.Index<BaseDocument>(o => o.Document(indexingDocument)
                            //It's so important. Child document must be in the same routing
                            .Routing(indexingDocument.Parent)
                            .Id(indexingDocument.Id)
                            .Index(_IndexName)).Refresh(Elasticsearch.Net.Refresh.True);
                }

            //You can use BulkAsync if you need
            var response = _ElasticClient.Bulk(descriptor);

            if (!response.IsValid || response.ItemsWithErrors.Any())
            {
                throw new Exception("Error on indexing!");
            }
        }
    }
}


static ConnectionSettings GetConnection()
{
    Uri node = new Uri("http://localhost:9200/");

    return new ConnectionSettings(node).EnableHttpCompression()
    .DisableDirectStreaming()
    .DefaultMappingFor<BaseDocument>(m => m.IndexName(_IndexName))
    .DefaultMappingFor<Category>(m => m.IndexName(_IndexName))
    .DefaultMappingFor<Supplier>(m => m.IndexName(_IndexName))
    .DefaultMappingFor<Stock>(m => m.IndexName(_IndexName))
    .DefaultMappingFor<Product>(m => m.IndexName(_IndexName))
    .EnableApiVersioningHeader();
}

static async Task SearchAsync()
{

    var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
    .Index(_IndexName)
    .Query(q => q
        .Match(m => m.Field(f => f.Name).Query("MSI")))
    .Size(20));
    Console.WriteLine(fullTextSearchResponse.DebugInformation);
    foreach (var data in fullTextSearchResponse.Documents)
    {
        Console.WriteLine($"{data.Name} {data.Price}");
    }
}

static async Task SearchAsync3()
{

    var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
    .Index(_IndexName)
    .Query(q => q
        .Wildcard(m => m.Field(f => f.Name).Value("*hone*")))
    .Size(20));
    Console.WriteLine(fullTextSearchResponse.DebugInformation);
    foreach (var data in fullTextSearchResponse.Documents)
    {
        Console.WriteLine($"{data.Name} {data.Price}");
    }
}

static async Task SearchAsync4()
{

    var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
    .Index(_IndexName)
    .Query(q => q
    .Ids(c => c
        .Values(1,4)
    )));
    Console.WriteLine(fullTextSearchResponse.DebugInformation);
    foreach (var data in fullTextSearchResponse.Documents)
    {
        Console.WriteLine($"{data.Name} {data.Price}");
    }
}

static async Task SearchAsync2()
{
    //    Console.WriteLine($"========");
    //    var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
    //    .Index(_IndexName)
    //    .Query(q => q.HasChild<Stock>
    //    (c => c
    //    .MaxChildren(1)
    //    .MinChildren(0)
    //    .Query(qq => qq.MatchAll())
    //)));

    //    foreach (var data in fullTextSearchResponse.Documents)
    //    {
    //        Console.WriteLine($"{data.Name} {data.Price}");
    //    }
    Console.WriteLine($"========");
    var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
    .Index(_IndexName)
    .Query(q => q.HasChild<Stock>(s => s.Query(k => k.Wildcard(a => a.Field(field => field.Country).Value("P*")))
    ))
    .Size(10));

    Console.WriteLine(fullTextSearchResponse.DebugInformation);

    foreach (var data in fullTextSearchResponse.Documents)
    {
        Console.WriteLine($"{data.Name} {data.Price}");
    }
}

public partial class Program
{
    public static ElasticClient _ElasticClient;
    public const int _IndexingDocumentCount = 1000;
    public static string _IndexName = "multiplejoinindex51";
}