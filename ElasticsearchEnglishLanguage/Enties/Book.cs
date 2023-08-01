using Elasticsearch.Net;
using Nest;

[ElasticsearchType(RelationName = "book", IdProperty = nameof(Id))]
public class Book
{
    public int Id { get; set; }

    public Author Author { get; set; }

    [Text(Boost = 1.5)]
    public string Title { get; set; }

    public string Opening { get; set; }

    [StringEnum]
    public BookGenre Genre { get; set; }

    [Ignore]
    public int InitialPublishYear { get; set; }

}


