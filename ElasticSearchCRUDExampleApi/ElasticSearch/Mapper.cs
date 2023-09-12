using ElasticSearchCRUDExampleApi.Dto;
using ElasticSearchCRUDExampleApi.Enties;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ElasticSearchCRUDExampleApi.ElasticSearch;

public class Mapper
{
    public GameMission Map(AddMissionRequest request)
    {
        GameMission mission = new GameMission();
        mission.Title = request.Name;
        mission.SoliderId = request.SoliderId;
        mission.Phases = Map(request.Phases);

        return mission;
    }

    private List<Phase> Map(List<PhaseDto> phases)
    {
        List<Phase> list = new List<Phase>();

        foreach (var item in phases)
        {
            list.Add(Map(item));
        }

        return list;
    }

    private Phase Map(PhaseDto phase)
    {
        return new Phase() { Text = phase.Text, Type = phase.PhaseType };
    }

    internal GameMissionDto Map(GameMission entity)
    {
        GameMissionDto dto = new GameMissionDto();
        dto.Number = entity.Number;
        dto.SoliderId = entity.SoliderId;
        dto.Phases = Map(entity.Phases);
        dto.Name = entity.Title;

        return dto;
    }

    private List<PhaseDto> Map(List<Phase> phases)
    {
        List<PhaseDto> list = new();

        foreach (var item in phases)
        {
            var dto = Map(item);
            list.Add(dto);
        }

        return list;
    }

    private PhaseDto Map(Phase phases)
    {
        return new PhaseDto() { PhaseType = phases.Type, Text = phases.Text };
    }


    internal GameMission Map(UpdateMissionRequest request)
    {
        GameMission mission = new GameMission();

        mission.Number = request.Number;
        mission.SoliderId = request.SoliderId;
        mission.Title = request.Body.Name;
        mission.Phases = Map(request.Body.Phases);

        return mission;
    }

    internal ImmutableList<GameMissionDto> Map(List<GameMission> missions)
    {
        List<GameMissionDto> list = new();

        foreach (var item in missions)
        {
            list.Add(Map(item));
        }

        return list.ToImmutableList();
    }
}
