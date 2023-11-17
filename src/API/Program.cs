using API.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddDbContext<BlogContext>(
    x => x.UseSqlServer(
        config.GetConnectionString("BlogConnection"),
        options =>
        {
            //TODO: Here we can configure for Split Query Options
        })
    );

builder.Services.AddControllers();
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(content =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: content.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 5,
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                Window = TimeSpan.FromSeconds(10)
            }));
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Blogs API",
        Description = "An ASP.NET Core Web API to manage Blogs",
        TermsOfService = new Uri("https://github.com/incognitotaj"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Salman Taj",
            Url = new Uri("https://www.linkedin.com/in/salman-horizons/"),
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://www.linkedin.com/in/salman-horizons/")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddIdentityServices(config);

var app = builder.Build();

using var scope = app.Services.CreateAsyncScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{

    var context = services.GetRequiredService<BlogContext>();
    await context.Database.MigrateAsync();

    //var contextIdentity = services.GetRequiredService<AppIdentityDbContext>();
    //await contextIdentity.Database.MigrateAsync();

    //var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
    //await IdentityDataSeeder.SeedAsync(userManager);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex.Message);
}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Blogs API");
        options.RoutePrefix = string.Empty;
        options.DocExpansion(DocExpansion.None);
    });
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

app.Run();
