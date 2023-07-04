using Nest;


public abstract class BaseDocument
{
    public int Parent { get; set; }

    public JoinField JoinField { get; set; }

    public BaseDocument()
    {

    }
}

[ElasticsearchType(RelationName = "product")]
public class Product : BaseDocument
{
    public int Id { get; set; }

    [Keyword]
    public string Name { get; set; }

    public decimal Price { get; set; }

}

[ElasticsearchType(RelationName = "stock")]
public class Stock : BaseDocument
{
    [Keyword]
    public string Country { get; set; }

    public int Quantity { get; set; }

}

[ElasticsearchType(RelationName = "supplier")]
public class Supplier : BaseDocument
{
    [Keyword]
    public string SupplierDescription { get; set; }

}


[ElasticsearchType(RelationName = "category")]
public class Category : BaseDocument
{
    [Keyword]
    public string CategoryDescription { get; set; }

}
