using MaterialGenerator.Api.GenerateMaterialSet;
using MaterialGenerator.Application;
using MaterialGenerator.Application.Contracts.GenerateMaterialSet;
using MaterialGenerator.Web;
using Microsoft.AspNetCore.Mvc;
using RowanWillis.Common.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnableTryItOutByDefault());
}

app.UseHttpsRedirection();

app.MapPost("/GenerateMaterialSet", async (
        [FromBody] GenerateMaterialSetRequest request,
        [FromServices] ICommandSender sender) =>
    {
        return (await sender.Send<GenerateMaterialSet, GenerateMaterialSetResult>(new()
        {
            Id = request.Id,
            Name = request.Name,
            BaseColour = request.BaseColour,
        }))
        .ToHttpResult<GenerateMaterialSetResult, GenerateMaterialSetResponse>(result => new()
        {
            GeneratedResources = result.GeneratedResources
        });
    })
    .WithName("GenerateMaterialSet")
    .WithOpenApi();

app.Run();