using DistributedTransactionsApi.DependencyInjection;
using DistributedTransactionsApi.Shared;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationDbContexts(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationSwagger(builder.Configuration);

builder.Host
    .UseLogging(builder.Configuration)
    .UseSystemd();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseApplicationSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(config =>
{
    config.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .Build();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();