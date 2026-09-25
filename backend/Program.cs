using Microsoft.EntityFrameworkCore;
using EnhanzerAssessment.Api.Data;
using EnhanzerAssessment.Api.Services;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://enhanzer-fullstack-assessment.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configure Forwarded Headers for Cloud Reverse Proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Application services
builder.Services.AddHttpClient();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<PurchaseBillService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

// Forwarded headers must be processed before HTTPS redirection.
app.UseForwardedHeaders();

// Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS redirection
app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAngularClient");

// Authorization
app.UseAuthorization();

// Controllers
app.MapControllers();

app.Run();