using Microsoft.OpenApi.Models;
using MoooCart.DB.Base;
using MoooCart.DB.Repositories;
using MoooCart.DB.Services;
using MoooCart.lib.Base;
using MoooCart.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Serilog for logging and error tracking
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
Log.Logger.Information("MoooCart API just started ......");

// 2. Add controllers and resolve JSON infinite cycle issues
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ICartService, CartServices>();
builder.Services.AddScoped<ICart, CartRepo>();

// 3. Add Swagger for API testing and documentation
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddMemoryCache();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MoooCart API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// 4. Inject database and business logic services
builder.Services.AddInjectionOptionsDB(builder.Configuration);
builder.Services.AddInjectionOptions();

// (Blazor CORS settings have been removed as this is now a pure API)

try
{
    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.AddMiddleWareDb();

    // 5. Configure Development environment settings
    //if (app.Environment.IsDevelopment())
    //{
        app.UseSwagger();
        app.UseSwaggerUI();
    //}

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    Log.Logger.Information("MoooCart API trying to work now ........");

    app.Run();

    Log.Logger.Information("MoooCart API is working now .........");
}
catch (Exception ex)
{
    Log.Logger.Error(ex, "MoooCart API has been stopped .......");
}
finally
{
    Log.Logger.Information("MoooCart API is closed ........");
    Log.CloseAndFlush();
}