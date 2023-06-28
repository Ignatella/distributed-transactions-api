using DistributedTransactionsApi.Core;
using DistributedTransactionsApi.Services;
using DistributedTransactionsApi.Shared;
using DistributedTransactionsApi.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationDbContexts(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UserUtility>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DepartmentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationSwagger(builder.Configuration);

builder.Host
    .UseLogging(builder.Configuration)
    .UseSystemd();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandler();

app.UseSwagger();
app.UseApplicationSwaggerUI();

app.UseApplicationCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();