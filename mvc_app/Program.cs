using Microsoft.EntityFrameworkCore;
using mvc_app.Services;
using Microsoft.IdentityModel.Tokens;//JWT
using Microsoft.AspNetCore.Authentication.BearerToken;
using mvc_app.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

//namespace mvc_app
//{
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //Add service products DI container
        builder.Services.AddScoped<IServiceProducts, ServiceProducts>();
        builder.Services.AddDbContext<ProductContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        //Identity context
        builder.Services.AddDbContext<UserContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Identity configuration (cookie-based authentication)
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            // Password settings
            options.SignIn.RequireConfirmedEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredUniqueChars = 0;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();

        // JWT Configuration for API authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        // Add cookie-based authentication for Identity
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme; // Cookie-based auth for Identity
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme; // For external login providers (optional)
        }).AddCookie();

        //builder.Services.AddAuthorization();
        builder.Services.AddControllersWithViews();
        var app = builder.Build();

        app.UseRouting(); //Important include first
        //identity
        app.UseAuthentication(); //Next
        app.UseAuthorization(); //Next

        app.UseStaticFiles();
        //https://localhost:[port]/
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
        //API Controllers
        app.MapControllers();
        app.Run();
    }
}
//}
