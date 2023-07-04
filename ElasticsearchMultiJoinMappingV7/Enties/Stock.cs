using Nest;

[ElasticsearchType(RelationName = "stock")]
public class Stock : BaseChildDocument
{
    [Keyword]
    public string Country { get; set; }

    public int Quantity { get; set; }

}
