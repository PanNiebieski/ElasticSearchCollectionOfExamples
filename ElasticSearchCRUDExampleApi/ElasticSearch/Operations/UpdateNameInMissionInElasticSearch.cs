using ElasticSearchCRUDExampleApi.Dto;
using ElasticSearchCRUDExampleApi.Enties;
using Microsoft.Extensions.Options;
using Nest;

namespace ElasticSearchCRUDExampleApi.ElasticSearch.Operations;

public class UpdateNameInMissionInElasticSearch
{
    private readonly ElasticSearchOptions _options;
    private readonly ElasticClient _client;
    private readonly Mapper _mapper;

    public UpdateNameInMissionInElasticSearch(IOptions<ElasticSearchOptions> options,
    ElasticSearchClientCreator creator,
    Mapper mapper)
    {
        _client = creator.CreateClient();
        _options = options.Value;
        _mapper = mapper;
    }

    public async Task<OperationResult> Execute(UpdateNameRequest request)
    {
        var searchResult = await _client.SearchAsync<GameMission>(q =>
            q.Index(_options.Index.Name)
            .Query(rq => rq.Term(f => f.SoliderId, request.SoliderId)
                && rq.Term(f => f.Number, request.Number)
        ));

        if (!searchResult.IsValid)
            return new OperationResult(searchResult.GetErrorDetails());

        if (searchResult.Hits.Count == 0)
            return OperationResult.NotFoundNothingToUpdate
                ($"Mission don't exist with SoliderId {request.SoliderId} and Number {request.Number}");


        var scriptPrams = new Dictionary<string, object>() { { "name", request.newTitle } };

        var result = await _client.UpdateByQueryAsync<GameMission>(uq => uq.
            Query(q => q.Term(f => f.Id, searchResult.Hits.First().Id))
            .Script(s => s.Source("ctx._source.title = params.title;").Params(scriptPrams)));

        if (!result.IsValid)
            return new OperationResult(result.GetErrorDetails());

        return new OperationResult();
    }
}
