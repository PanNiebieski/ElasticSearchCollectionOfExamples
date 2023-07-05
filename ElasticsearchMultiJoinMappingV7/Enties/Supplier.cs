using Nest;

[ElasticsearchType(RelationName = "supplier")]
public class Supplier : BaseDocument
{
    [Keyword]
    public string SupplierDescription { get; set; }

}
