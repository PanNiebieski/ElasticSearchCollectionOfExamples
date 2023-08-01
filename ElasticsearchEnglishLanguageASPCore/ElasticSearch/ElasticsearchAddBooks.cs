using Microsoft.Extensions.Logging;
using Nest;

public class ElasticsearchAddBooks
{
    private readonly ILogger<ElasticsearchAddBooks> _logger;
    private readonly IElasticClient _elasticClient;

    public ElasticsearchAddBooks(ILogger<ElasticsearchAddBooks> logger, IElasticClient elasticClient)
    {
        _logger = logger;
        _elasticClient = elasticClient;
    }

    public async Task InsertAsync()
    {
        foreach (var book in StaticCollection.GetBooks())
        {
            var existsResponse = await _elasticClient.DocumentExistsAsync<Book>(book);
            // If the document already exists, we're going to update it; otherwise insert it
            // Note:  You may get existsResponse.IsValid = false for a number of issues
            // ranging from an actual server issue, to mismatches with indices (e.g. a
            // mismatch on the datatype of Id).
            if (existsResponse.IsValid && existsResponse.Exists)
            {
                var updateResponse = await _elasticClient.UpdateAsync<Book>(book, u => u.Doc(book));
                if (!updateResponse.IsValid)
                {
                    var errorMsg = "Problem updating document in Elasticsearch.";
                    _logger.LogError(updateResponse.OriginalException, errorMsg);
                    throw new Exception(errorMsg);
                }
            }
            else
            {
                var insertResponse = await _elasticClient.IndexDocumentAsync(book);
                if (!insertResponse.IsValid)
                {
                    var errorMsg = "Problem inserting document to Elasticsearch.";
                    _logger.LogError(insertResponse.OriginalException, errorMsg);
                    throw new Exception(errorMsg);
                }
            }
        }
    }
}
