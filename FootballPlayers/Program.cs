using FootballPlayers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
   
    app.UseHttpsRedirection();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseDeveloperExceptionPage();

app.MapGet("/", context =>
{
    context.Response.Redirect("/AddPlayer");
    return Task.CompletedTask;
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});
app.MapControllers();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    DbTeams.Initialize(context);
}

app.Run();