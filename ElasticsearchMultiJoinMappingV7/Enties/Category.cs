using Nest;


[ElasticsearchType(RelationName = "category")]
public class Category : BaseChildDocument
{
    [Keyword]
    public string CategoryDescription { get; set; }

}
