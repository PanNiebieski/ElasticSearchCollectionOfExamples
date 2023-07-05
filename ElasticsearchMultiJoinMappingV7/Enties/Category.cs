using Nest;


[ElasticsearchType(RelationName = "category")]
public class Category : BaseDocument
{
    [Keyword]
    public string CategoryDescription { get; set; }

}
