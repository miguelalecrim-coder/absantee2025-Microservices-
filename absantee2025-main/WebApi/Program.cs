using Application.DTO;


using Domain.Factory;

using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Application.Messaging;
using Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AbsanteeContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services

builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<IMessagePublisher, MassTransitPublisher>();

//Repositories

builder.Services.AddTransient<IProjectRepository, ProjectRepositoryEF>();


//Factories


builder.Services.AddTransient<IProjectFactory, ProjectFactory>();


//Mappers

builder.Services.AddTransient<ProjectDataModelConverter>();

builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO

    cfg.CreateMap<Project, ProjectDTO>();
    cfg.CreateMap<ProjectDTO, Project>();

});


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProjectCreatedConsumer>();
    x.AddConsumer<ProjectUpdatedConsumer>();


    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.ReceiveEndpoint("project-write-sync", e =>
        {
            e.ConfigureConsumer<ProjectCreatedConsumer>(context);
            e.ConfigureConsumer<ProjectUpdatedConsumer>(context);
            
        });
    });
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();



app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
