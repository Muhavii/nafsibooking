using nafsibooking.Services;
using nafsibooking.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=nafsi.db"));

builder.Services.AddRazorPages();
builder.Services.AddScoped<IEventService, DatabaseEventService>();
builder.Services.AddScoped<IPromoterRequestService, DatabasePromoterRequestService>();
builder.Services.AddSingleton<IAdminAuthService, SimpleAdminAuthService>();
builder.Services.AddScoped<IUserAuthService, SimpleUserAuthService>();
builder.Services.AddScoped<IUserService, DatabaseUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Apply pending migrations automatically at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch
    {
        // Ignore migration errors at startup for now
    }
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
