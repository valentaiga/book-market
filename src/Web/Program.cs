using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ConfigureLogger();

builder.Services.ConfigureMediatR();
builder.Services.ConfigureMapper();
builder.Services.ConfigureDatabase();
builder.Services.ConfigureValidation();
builder.Services.ConfigureControllers();

var app = builder.Build();

app.AddSwagger();
app.AddMiddleware();

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

await app.RunAsync();