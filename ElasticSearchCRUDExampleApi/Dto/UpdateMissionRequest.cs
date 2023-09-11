namespace ElasticSearchCRUDExampleApi.Dto;

public class UpdateMissionRequest
{
    public string SoliderId { get; set; }

    public int Number { get; set; }

    public UpdateMissionBody Body { get; set; }

    public class UpdateMissionBody
    {
        public string Name { get; set; }

        public List<PhaseDto> Phases { get; set; }
    }

}

