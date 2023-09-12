using Nest;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSearchCRUDExampleApi.Enties;

[ElasticsearchType(RelationName = "mis")]
public class GameMission
{
    [Text]
    public string Title { get; set; }

    [Number]
    public long Number { get; set; }

    [Nested]
    public List<Phase> Phases { get; set; }

    [Keyword]
    public string SoliderId { get; set; }

    [NotMapped]
    public string Id { get; set; }

    public GameMission()
    {
        Phases = new List<Phase>();
    }
}
