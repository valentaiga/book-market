using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogger();

builder.Services.ConfigureMediatR();
builder.Services.ConfigureMapper();
builder.Services.ConfigureDatabase();
builder.Services.ConfigureValidation();
builder.Services.ConfigureControllers();

var app = builder.Build();

app.AddMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

await app.RunAsync();