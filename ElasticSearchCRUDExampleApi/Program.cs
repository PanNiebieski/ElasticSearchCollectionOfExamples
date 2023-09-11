using ElasticSearchCRUDExampleApi;
using ElasticSearchCRUDExampleApi.Dto;
using ElasticSearchCRUDExampleApi.ElasticSearch;
using ElasticSearchCRUDExampleApi.ElasticSearch.Operations;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCrudEs(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var res = app.Services.GetService<ElasticSearchCreateIndex>().TryCreateIndexIfDontExistAsync();
Console.WriteLine(res.Result);


app.MapPost("/add", async ([FromServices] AddMissionToElasticSearch action, AddMissionRequest request) =>
{
    var result = await action.Execute(request);

    if (result.Success)
        return Results.Ok(result.Result);

    Console.WriteLine(result.Message);
    return Results.BadRequest();

})
.WithName("Add")
.WithOpenApi();

app.MapPost("/update", async ([FromServices] UpdateMissionToElasticSearch action, UpdateMissionRequest request) =>
{
    var result = await action.Execute(request);

    if (result.Status == OperationStatus.NotFoundNothingToUpdate)
        return Results.NotFound();

    if (result.Success)
        return Results.Ok();

    Console.WriteLine(result.Message);
    return Results.BadRequest();
})
.WithName("Update")
.WithOpenApi();

app.MapPost("/delete", async ([FromServices] DeleteMissonFromElasticSearch action, DeleteMissionRequest request) =>
{
    var result = await action.Execute(request);

    if (result.Success)
        return Results.Ok();

    Console.WriteLine(result.Message);
    return Results.BadRequest();
})
.WithName("Delete")
.WithOpenApi();

app.MapPost("/get", async ([FromServices] GetMissionsFromElasticSearch action, GetMissionsRequest request) =>
{
    var result = await action.Execute(request);

    if (result.Success && result.Result != null && result.Result.Count == 0)
        return Results.NotFound();

    if (result.Success)
        return Results.Ok(result.Result);

    Console.WriteLine(result.Message);
    return Results.BadRequest();
})
.WithName("Get")
.WithOpenApi();

app.Run();
