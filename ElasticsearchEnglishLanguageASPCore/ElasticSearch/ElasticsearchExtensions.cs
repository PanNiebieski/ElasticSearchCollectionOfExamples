using ElasticsearchEnglishLanguageASPCore.ElasticSearch;
using Nest;

public static class ElasticsearchExtensions
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new ConnectionSettings(new Uri(configuration["ElasticsearchSettings:uri"]));
        var defaultIndex = configuration["ElasticsearchSettings:defaultIndex"];
        if (!string.IsNullOrEmpty(defaultIndex))
            settings = settings.DefaultIndex(defaultIndex);
        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);
        services.AddSingleton<ElasticsearchCreateIndex>();
        services.AddSingleton<ElasticsearchAddBooks>();
        services.AddSingleton<ElasticsearchStartup>();
    }
}