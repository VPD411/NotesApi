using NotesApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddSingleton(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    return new DatabaseInitializer(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
initializer.Initialize();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseStaticFiles();
app.UseDefaultFiles();

app.MapControllers();

app.Run();