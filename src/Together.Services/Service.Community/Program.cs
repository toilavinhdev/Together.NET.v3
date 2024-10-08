using Infrastructure.Logging;
using Infrastructure.PostgreSQL;
using Infrastructure.SharedKernel;
using Infrastructure.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Service.Community;
using Service.Community.Domain;

var builder = WebApplication.CreateBuilder(args);
builder.SetupEnvironment<AppSettings>(out var appSettings);
builder.SetupSerilog();

var services = builder.Services;
services.AddSharedKernel<Program>(appSettings);
services.AddPostgresDbContext<CommunityContext>(appSettings.PostgresConfig);
services.AddGrpc();

var app = builder.Build();
app.UseSharedKernel(appSettings);
app.UseGrpc(appSettings.GrpcEndpoints.ServiceCommunity, _ => {});
await CommunityContextInitialization.SeedAsync(app.Services);
app.Run();