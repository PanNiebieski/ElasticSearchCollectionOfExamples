using Elasticsearch.Net;
using ElasticSearchCRUDExampleApi.Enties;
using Microsoft.Extensions.Options;
using Nest;

namespace ElasticSearchCRUDExampleApi.ElasticSearch;

public class ElasticSearchClientCreator
{
    private readonly ElasticSearchOptions _options;

    public ElasticSearchClientCreator(IOptions<ElasticSearchOptions> options)
    {
        _options = options.Value;
    }

    public ElasticClient CreateClient()
    {
        var uri = new Uri($"{_options.Connection.Protocol}://{_options.Connection.AddressEndPoint}");

        var settings = new ConnectionSettings(uri)
            .DefaultIndex(_options.Index.Name)
            .DefaultMappingFor<GameMission>(m => m.IndexName(_options.Index.Name))
            .EnableApiVersioningHeader()
            .RequestTimeout(TimeSpan.FromMilliseconds(_options.Connection.RequestTimeout))
            .EnableHttpCompression(_options.Connection.EnableHttpCompression)
            .DisableDirectStreaming(_options.Debug.DisableDirectStreaming)
            .PrettyJson(_options.Debug.PrettyJson);

        if (_options.Debug.EnableDebugMode)
            settings.EnableDebugMode(call => OnRequestCompleted(call));

        if (_options.Connection.AuthRequired)
            settings.BasicAuthentication(_options.Connection.UserName, _options.Connection.Password);

        return new ElasticClient(settings);
    }

    private void OnRequestCompleted(IApiCallDetails call)
    {

    }
}
