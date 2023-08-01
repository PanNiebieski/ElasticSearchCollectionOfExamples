using ElasticsearchEnglishLanguageASPCore.ElasticSearch;
using Nest;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddElasticsearch(config);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var start = app.Services.GetService<ElasticsearchStartup>();
await start.RunAsync();

app.MapGet("/query", async (string q, IElasticClient _elasticClient) =>
{
    if (string.IsNullOrEmpty(q))
    {
        return new List<Book>();
    }

    var response = await _elasticClient.SearchAsync<Book>(s =>
       s.Query(sq =>
           sq.MultiMatch(mm => mm
               .Query(q)
               .Fuzziness(Fuzziness.Auto)
           )
       )
   );

    return response.Documents?.ToList();

})
.WithName("Query")
.WithOpenApi();

app.Run();
