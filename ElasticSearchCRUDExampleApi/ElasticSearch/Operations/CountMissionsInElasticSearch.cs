using ElasticSearchCRUDExampleApi.Enties;
using Microsoft.Extensions.Options;
using Nest;

namespace ElasticSearchCRUDExampleApi.ElasticSearch.Operations;

public class CountMissionsInElasticSearch
{
    private readonly ElasticSearchOptions _options;
    private readonly ElasticClient _client;

    public CountMissionsInElasticSearch(IOptions<ElasticSearchOptions> options,
        ElasticSearchClientCreator creator)
    {
        _client = creator.CreateClient();
        _options = options.Value;
    }

    public async Task<OperationResult<long>> Execute()
    {
        var result = await _client.CountAsync<GameMission>
            (f => f.Query(q => q.Exists(f => f.Field(f => f.SoliderId))));

        if (!result.IsValid)
            return new OperationResult<long>(result.GetErrorDetails());

        return new OperationResult<long>(result.Count);
    }
}
