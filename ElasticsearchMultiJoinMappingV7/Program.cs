using Nest;


_ElasticClient = new ElasticClient(GetConnection());
CreateIndexAndMappings(numberOfReplicas: 0, numberOfShards: 5, refreshInterval: -1);

#region Documents
List<BaseParentDocument> products = new List<BaseParentDocument>()
{
    new Product()
    {
        Id = 1,
        Name = "IPhone 7",
        Price = 100,
        JoinField = "product",
    },

    new Product()
    {
        Id = 2,
        Name = "IPhone 8",
        Price = 100,
        JoinField = "product"
    },
        new Product()
    {
        Id = 3,
        Name = "Laptop MSI",
        Price = 5000,
        JoinField = "product"
    }
};
var suppliers = new List<BaseChildDocument>()
{
    new Supplier()
    {
        SupplierDescription="Apple",
        Parent = 1,
        JoinField = JoinField.Link("supplier", 1),
    },

    new Supplier()
    {
        SupplierDescription="A supplier",
        Parent = 1,
        JoinField = JoinField.Link("supplier", 1)
    },

    new Supplier()
    {
        SupplierDescription="Another supplier",
        Parent = 2,
        JoinField = JoinField.Link("supplier", 2)
    }
};
var stocks = new List<BaseChildDocument>()
{
    new Stock()
    {
        Country="USA",
        JoinField = JoinField.Link("stock", 1)
    },

    new Stock()
    {
        Country="UK",
        JoinField = JoinField.Link("stock", 2)
    },

    new Stock()
    {
        Country="Germany",
        JoinField = JoinField.Link("stock", 2)
    }
};
var categoriees = new List<BaseChildDocument>()
{
    new Category()
    {
        CategoryDescription= "Electronic",
        JoinField = JoinField.Link("category", 1)
    },

    new Category()
    {
        CategoryDescription = "Smart Phone",
        JoinField = JoinField.Link("category", 2)
    },

    new Category()
    {
        CategoryDescription = "Phone",
        JoinField = JoinField.Link("category", 2)
    }
};
#endregion

IndexDocuments(products);
IndexChildDocuments(categoriees);
IndexChildDocuments(stocks);
IndexChildDocuments(suppliers);

await SearchAsync();
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
                    .Index<BaseChildDocument>()
                    .Map<BaseChildDocument>(m => m
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

static void IndexDocuments(List<BaseParentDocument> documents)
{
    if (documents != null && documents.Count != 0)
    {
        int lastIndex = (int)Math.Ceiling((decimal)documents.Count / _IndexingDocumentCount);

        for (int i = 1; i <= lastIndex; i++)
        {
            List<BaseParentDocument> items = documents.Skip(1000 * (i - 1)).Take(_IndexingDocumentCount).ToList();
            var descriptor = new BulkDescriptor();

            if (items != null && items.Count > 0)
                for (int index = 0; index < items.Count; index++)
                {
                    var indexingDocument = items[index];
                    if (indexingDocument != null)
                        descriptor.Index<BaseParentDocument>(o => o.Document(indexingDocument)
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

static void IndexChildDocuments(List<BaseChildDocument> documents)
{
    if (documents != null && documents.Count != 0)
    {
        int lastIndex = (int)Math.Ceiling((decimal)documents.Count / _IndexingDocumentCount);

        for (int i = 1; i <= lastIndex; i++)
        {
            List<BaseChildDocument> items = documents.Skip(1000 * (i - 1)).Take(_IndexingDocumentCount).ToList();
            var descriptor = new BulkDescriptor();

            if (items != null && items.Count > 0)
                for (int index = 0; index < items.Count; index++)
                {
                    var indexingDocument = items[index];
                    if (indexingDocument != null)
                        descriptor.Index<BaseChildDocument>(o => o.Document(indexingDocument)
                            //It's so important. Child document must be in the same routing
                            .Routing(indexingDocument.Parent)
                            //.Id(indexingDocument.Id) if a child had also a id
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
    .DefaultMappingFor<BaseChildDocument>(m => m.IndexName(_IndexName))
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
        .Match(m => m.Field(f => f.Name).Query("7")))
    .Size(20));

    foreach (var data in fullTextSearchResponse.Documents)
    {
        Console.WriteLine($"{data.Name} {data.Price}");
    }
}

static async Task SearchAsync2()
{
    var fullTextSearchResponse = await _ElasticClient.SearchAsync<Product>(s => s
    .Index(_IndexName)
    .Query(q => q.HasChild<Stock>(s => s.Query(cq => cq.MatchAll(m => m.Name("USA")))))
    .Size(10));

    foreach (var data in fullTextSearchResponse.Documents)
    {
        Console.WriteLine($"{data.Name} {data.Price}");
    }
}

public partial class Program
{
    public static ElasticClient _ElasticClient;
    public const int _IndexingDocumentCount = 1000;
    public static string _IndexName = "multiplejoinindex4";
}