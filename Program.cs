using ApiGraph.src.application.services;
using ApiGraph.src.infrastructure.services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MicrosoftGraphOptions>(builder.Configuration.GetSection("MicrosoftGraph"));

builder.Services.AddScoped(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    string tenantId = configuration["AzureAd:TenantId"] ?? "";
    string clientId = configuration["AzureAd:ClientId"] ?? "";
    string clientSecret = configuration["AzureAd:ClientSecret"] ?? "";

    return new GraphClient(tenantId, clientId, clientSecret);
});

builder.Services.AddScoped<ISharepointService, SharepointService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
