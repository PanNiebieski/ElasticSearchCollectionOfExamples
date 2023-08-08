using Nest;

[ElasticsearchType(RelationName = "product")]
public class Product : BaseDocument
{
    [Keyword]
    public string Name { get; set; }

    [Number]
    public decimal Price { get; set; }

    [Text]
    public string Specyfication { get; set; }

    public string Description { get; set; }
}