using SportsCalendar.Api;
using SportsCalendar.Application;
using SportsCalendar.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.Run();