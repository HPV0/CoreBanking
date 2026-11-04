using Asp.Versioning;
using CoreBanking.API.Exceptions;
using CoreBanking.API.Swagger;
using CoreBanking.Application.Common;
using CoreBanking.Application.Interfaces.Common;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Application.Interfaces.IServices;
using CoreBanking.Application.Mappers;
using CoreBanking.Application.Services;
using CoreBanking.Infrastructure.Data.Context;
using CoreBanking.Infrastructure.Persistence;
using CoreBanking.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseSnakeCaseNamingConvention());


builder.Services.AddSingleton<IMoneyCalculator, MoneyCalculator>();

builder.Services.AddScoped<IClinetService, ClinetService>();
builder.Services.AddScoped<IAccountBalanceService, AccountBalanceService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ClientMapper>();
builder.Services.AddScoped<CurrencyMapper>();
builder.Services.AddScoped<AccountMapper>();
builder.Services.AddScoped<TransactionMapper>();

// We need AddProblemDetails, for better view of Custom Errors
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomValidationExceptionHandler>();
builder.Services.AddExceptionHandler<FluentValidationExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessLogicExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
// We need GlobalExceptionHandler, for better view of Server Errors
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");

    options.InstanceName = "CoreBanking:";
});
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("Redis")!));


builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);

        options.AssumeDefaultVersionWhenUnspecified = true;

        options.ReportApiVersions = true;

        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";

        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Core Banking API V1",
        Version = "v1"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Core Banking API V2",
        Version = "v2"
    });

    options.DocInclusionPredicate((documentName, apiDescription) =>
        apiDescription.GroupName == documentName);

    options.OperationFilter<IdempotencyKeyOperationFilter>();
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "API V2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
