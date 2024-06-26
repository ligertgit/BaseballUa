using BaseballUa.Data;
using Microsoft.EntityFrameworkCore;
using BaseballUa.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireUppercase = false;
	options.Password.RequireNonAlphanumeric = false;
});

builder.Services.AddDbContext<BaseballUaDbContext>(options =>
					options.UseSqlServer(builder.Configuration.GetConnectionString("BaseballUaConnectString")));

builder.Services.AddDefaultIdentity<BaseballUaUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BaseballUaDbContext>();

//builder.Services.AddDefaultIdentity<BaseballUaUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<BaseballUaDbContext>();

//builder.Services.AddScoped<ICrud<Category>, CategoriesCrud>();
//builder.Services.AddScoped<ICrud<Tournament>, TournamentsCrud>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
