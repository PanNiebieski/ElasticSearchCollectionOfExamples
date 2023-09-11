namespace ElasticSearchCRUDExampleApi.Dto;

public class AddMissionRequest
{
    public string Name { get; set; }

    public List<PhaseDto> Phases { get; set; }

    public string SoliderId { get; set; }
}
