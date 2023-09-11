using Nest;

namespace ElasticSearchCRUDExampleApi.Dto;

public class GameMissionDto
{
    public string Name { get; set; }

    public long Number { get; set; }

    public List<PhaseDto> Phases { get; set; }

    public string SoliderId { get; set; }
}
