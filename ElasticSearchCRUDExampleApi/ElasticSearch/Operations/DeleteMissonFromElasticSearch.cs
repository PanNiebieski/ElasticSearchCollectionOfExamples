using ElasticSearchCRUDExampleApi.Dto;
using ElasticSearchCRUDExampleApi.Enties;
using Microsoft.Extensions.Options;
using Nest;

namespace ElasticSearchCRUDExampleApi.ElasticSearch.Operations;

public class DeleteMissonFromElasticSearch
{
    private readonly ElasticSearchOptions _options;
    private readonly ElasticClient _client;
    private readonly Mapper _mapper;

    public DeleteMissonFromElasticSearch(IOptions<ElasticSearchOptions> options,
        ElasticSearchClientCreator creator,
        Mapper mapper)
    {
        _client = creator.CreateClient();
        _options = options.Value;
        _mapper = mapper;
    }

    public async Task<OperationResult> Execute(DeleteMissionRequest request)
    {
        var result = await _client.DeleteByQueryAsync<GameMission>(q =>
            q.Index(_options.Index.Name)
            .Query(rq => rq.Term(f => f.SoliderId, request.SoliderId)
                && rq.Term(f => f.Number, request.Number)
        ));

        if (!result.IsValid)
            return new OperationResult(result.GetErrorDetails());

        return new OperationResult();
    }
}
