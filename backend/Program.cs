using backend.Services.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IUtilityRepo, UtilityRepoImpl>();
builder.Services.AddSingleton<IUtilityService, UtilityServiceImpl>();
builder.Services.AddSingleton<IScrapper, ScrapperImpl>();
builder.Services.AddSingleton<IEboksService, EboksServiceImpl>();
builder.Services.AddHostedService<DataPullerImpl>();
builder.Services.AddSingleton<IRtoRepo, RtoRepoImpl>();
builder.Services.AddSingleton<IFacade, FacadeImpl>();
builder.Services.AddSingleton<IRtoDataService, RtoDataServiceImpl>();
builder.Services.AddSingleton<IUserPref, UserPrefImpl>();
builder.Services.AddSingleton<IUserPrefService, UserPrefServiceImpl>();
builder.Services.AddHostedService<AzureHubImpl>();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR(
    options =>
    {
        options.EnableDetailedErrors = true;
        options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
        options.KeepAliveInterval = TimeSpan.FromMinutes(30);
    }
);


// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.UseCors(
    options =>
    {
        options.WithOrigins("http://localhost:3000");
        options.WithOrigins("https://dev.apps.kamstrup.com");
        options.WithOrigins("https://test.apps.kamstrup.com");
        options.WithOrigins("https://apps.kamstrup.com");
        options.AllowCredentials();
        options.AllowAnyHeader();
        options.AllowAnyMethod();
    }
);


app.MapHub<AzureHubImpl>("/hub");

app.Run();
