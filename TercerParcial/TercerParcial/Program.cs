using UPB.BussinessLogic.Managers;
using Serilog;
using TercerParcial.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<StudentCodeManager>();


Log.Logger = new LoggerConfiguration()
       .WriteTo.Console()
       .WriteTo.File(builder.Configuration.GetSection("Logging").GetSection("FileLocation").Value + "logs-.log", rollingInterval: RollingInterval.Day)
       .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
Log.Logger = new LoggerConfiguration()
       .WriteTo.Console()
       .WriteTo.File(builder.Configuration.GetSection("Logging").GetSection("FileLocation").Value + "logs-.log", rollingInterval: RollingInterval.Day)
       .CreateLogger();

Log.Information("INIT SERVER");

//app.UseErrorHandlingMiddleware();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
