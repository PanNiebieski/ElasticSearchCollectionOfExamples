using Nest;

[ElasticsearchType(RelationName = "supplier")]
public class Supplier : BaseChildDocument
{
    [Keyword]
    public string SupplierDescription { get; set; }

}
