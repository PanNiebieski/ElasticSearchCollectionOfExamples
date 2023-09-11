using Nest;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSearchCRUDExampleApi.Enties;

[ElasticsearchType(RelationName = "pha")]
public class Phase
{
    [Text]
    public string Text { get; set; }

    [Number]
    public int Type { get; set; }

    [NotMapped]
    public PhaseType PhaseType
    {
        get
        {
            return (PhaseType)Type;
        }
        set
        {
            Type = (int)value;
        }
    }
}