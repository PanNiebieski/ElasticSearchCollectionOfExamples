using Nest;

[ElasticsearchType(RelationName = "author")]
public class Author
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Author()
    {
    }
}