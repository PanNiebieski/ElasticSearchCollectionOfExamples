using ElasticSearchCRUDExampleApi.Dto;
using ElasticSearchCRUDExampleApi.Enties;
using Microsoft.Extensions.Options;
using Nest;
using System.Collections.Immutable;

namespace ElasticSearchCRUDExampleApi.ElasticSearch.Operations;

public class GetMissionsFromElasticSearch
{
    private readonly ElasticSearchOptions _options;
    private readonly ElasticClient _client;
    private readonly Mapper _mapper;

    public GetMissionsFromElasticSearch(IOptions<ElasticSearchOptions> options,
        ElasticSearchClientCreator creator,
        Mapper mapper)
    {
        _client = creator.CreateClient();
        _options = options.Value;
        _mapper = mapper;
    }

    public async Task<OperationResult<ImmutableList<GameMissionDto>>> Execute(GetMissionsRequest request)
    {
        var result = await _client.SearchAsync<GameMission>(q => q.Index(_options.Index.Name)
            .Query(rq => rq.Term(f => f.SoliderId, request.SoliderId))
            .Sort(s => s.Ascending(f => f.Number)));

        if (!result.IsValid)
            return new OperationResult<ImmutableList<GameMissionDto>>(result.GetErrorDetails());

        List<GameMission> missions = new();

        foreach (var item in result.Hits)
        {
            item.Source.Id = item.Id;
            missions.Add(item.Source);
        }

        var mappedResults = _mapper.Map(missions);

        return new OperationResult<ImmutableList<GameMissionDto>>(mappedResults);
    }
}
