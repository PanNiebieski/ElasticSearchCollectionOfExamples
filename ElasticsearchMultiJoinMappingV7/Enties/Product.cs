using Nest;

[ElasticsearchType(RelationName = "product")]
public class Product : BaseDocument
{
    [Keyword]
    public string Name { get; set; }

    public decimal Price { get; set; }

}
