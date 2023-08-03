using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticsearchMultiJoinMappingV7;

public static class CreatorOfIndexes
{
    public static ElasticClient _ElasticClient;
    public const int _IndexingDocumentCount = 1000;
    public static string _IndexName = "multiplejoinindextest13";

    public static void CreateIndexAndMappings(int numberOfReplicas, int numberOfShards, int refreshInterval)
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

    public static void IndexDocuments(List<BaseDocument> documents)
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

    public static void IndexChildDocuments(List<BaseDocument> documents)
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

    public static ConnectionSettings GetConnection()
    {
        Uri node = new Uri("http://localhost:9201/");

        return new ConnectionSettings(node).EnableHttpCompression()
        .DisableDirectStreaming()
        .DefaultMappingFor<BaseDocument>(m => m.IndexName(_IndexName))
        .DefaultMappingFor<Category>(m => m.IndexName(_IndexName))
        .DefaultMappingFor<Supplier>(m => m.IndexName(_IndexName))
        .DefaultMappingFor<Stock>(m => m.IndexName(_IndexName))
        .DefaultMappingFor<Product>(m => m.IndexName(_IndexName))
        .EnableApiVersioningHeader();
    }

}
