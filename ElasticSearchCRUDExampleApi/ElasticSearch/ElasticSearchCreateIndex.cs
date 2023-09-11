using ElasticSearchCRUDExampleApi.Enties;
using Microsoft.Extensions.Options;
using Nest;

namespace ElasticSearchCRUDExampleApi.ElasticSearch;

public class ElasticSearchCreateIndex
{
    private readonly ElasticSearchOptions _options;
    private readonly ElasticClient _client;

    public ElasticSearchCreateIndex(IOptions<ElasticSearchOptions> options, ElasticSearchClientCreator cret)
    {
        _options = options.Value;
        _client = cret.CreateClient();
    }

    public async Task<IndexResponse> TryCreateIndexIfDontExistAsync()
    {
        var exisiting = await _client.Indices.ExistsAsync(_options.Index.Name);

        if (exisiting.Exists)
            return new IndexResponse();

        var newIndexResponse = await _client.Indices.CreateAsync(GetCreateIndexDescriptor());

        return new IndexResponse(newIndexResponse);
    }

    private CreateIndexDescriptor GetCreateIndexDescriptor()
    {
        CreateIndexDescriptor createIndex = new CreateIndexDescriptor(_options.Index.Name);

        createIndex.Settings(s => s
            .NumberOfReplicas(_options.Index.NumberOfReplicas)
            .NumberOfShards(_options.Index.NumberOfShards)
            .RefreshInterval(_options.Index.RefreshInterval)
            ).Index<GameMission>()
            .Map<GameMission>(m => m.AutoMap<GameMission>());

        return createIndex;
    }
}
