using ElasticSearchCRUDExampleApi.ElasticSearch;
using ElasticSearchCRUDExampleApi.ElasticSearch.Operations;

namespace ElasticSearchCRUDExampleApi;

public static partial class CrudInstaller
{
    public static IServiceCollection AddCrudEs
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ElasticSearchOptions>
            (c => configuration.GetSection(ElasticSearchOptions.SectionName).Bind(c));

        services.AddSingleton<AddMissionToElasticSearch>();
        services.AddSingleton<DeleteMissonFromElasticSearch>();
        services.AddSingleton<GetMissionsFromElasticSearch>();
        services.AddSingleton<UpdateMissionToElasticSearch>();
        services.AddSingleton<ElasticSearchClientCreator>();
        services.AddSingleton<ElasticSearchCreateIndex>();
        services.AddSingleton<UpdateNameInMissionInElasticSearch>();
        services.AddSingleton<CountMissionsInElasticSearch>();
        services.AddSingleton<Mapper>();

        return services;
    }

}
