using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SP23.P02.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// sets up our database connection
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

builder.Services.AddControllers();
builder.Services.AddIdentity<User,Role>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<DataContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Events.OnRedirectToLogin = content =>
    {
        //401 Unauthorized response status code indicates that the client request has not been
        //completed because it lacks valid authentication credentials for the requested resource.
        content.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = content =>
    {
        //The HTTP 403 Forbidden response status code indicates that the server
        //understands the request but refuses to authorize it
        content.Response.StatusCode = 403;
        return Task.CompletedTask;
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    var services = scope.ServiceProvider;

    await SeedHelper.MigrateAndSeed(db, services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();

//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
// Hi 383 - this is added so we can test our
// web project automatically. More on that later
public partial class Program { }