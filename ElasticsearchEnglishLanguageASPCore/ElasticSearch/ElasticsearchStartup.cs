namespace ElasticsearchEnglishLanguageASPCore.ElasticSearch;

public class ElasticsearchStartup
{
    private ElasticsearchCreateIndex _createIndex;
    private ElasticsearchAddBooks _addbooks;

    public ElasticsearchStartup(ElasticsearchCreateIndex createIndex, ElasticsearchAddBooks addbooks)
    {
        _createIndex = createIndex;
        _addbooks = addbooks;
    }

    public async Task RunAsync()
    {
        await _createIndex.StartAsync();
        await _addbooks.InsertAsync();
    }
}