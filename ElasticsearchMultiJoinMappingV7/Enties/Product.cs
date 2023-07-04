using Nest;

[ElasticsearchType(RelationName = "product")]
public class Product : BaseParentDocument
{
    [Keyword]
    public string Name { get; set; }

    public decimal Price { get; set; }

}
