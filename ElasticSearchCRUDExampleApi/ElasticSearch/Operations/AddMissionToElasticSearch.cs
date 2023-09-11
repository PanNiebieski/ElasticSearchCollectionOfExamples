using ElasticSearchCRUDExampleApi.Dto;
using ElasticSearchCRUDExampleApi.Enties;
using Microsoft.Extensions.Options;
using Nest;

namespace ElasticSearchCRUDExampleApi.ElasticSearch.Operations;

public class AddMissionToElasticSearch
{
    private readonly ElasticSearchOptions _options;
    private readonly ElasticClient _client;
    private readonly Mapper _mapper;

    public AddMissionToElasticSearch(IOptions<ElasticSearchOptions> options,
        ElasticSearchClientCreator creator,
        Mapper mapper)
    {
        _client = creator.CreateClient();
        _options = options.Value;
        _mapper = mapper;
    }


    public async Task<OperationResult<GameMissionDto>> Execute(AddMissionRequest request)
    {
        var entity = _mapper.Map(request);

        var missions = await _client.SearchAsync<GameMission>(q =>
            q.Index(_options.Index.Name)
            .Query(rq => rq.Term(f => f.SoliderId, entity.SoliderId))
            .Sort(s => s.Descending(s => s.Number)).Size(1));

        if (!missions.IsValid)
            return new OperationResult<GameMissionDto>(missions.GetErrorDetails());

        entity.Number = 1;

        if (missions.Documents.Count > 0)
            entity.Number = missions.Documents.First().Number + 1;

        var indexRequest = new IndexRequest<GameMission>(entity, _options.Index.Name);

        if (_options.Index.RefreshInterval == -1)
            indexRequest.Refresh = Elasticsearch.Net.Refresh.True;

        var result = await _client.IndexAsync(indexRequest);

        if (!result.IsValid)
            return new OperationResult<GameMissionDto>(result.GetErrorDetails());

        GameMissionDto toReturn = _mapper.Map(entity);

        return new OperationResult<GameMissionDto>(toReturn);
    }
}
