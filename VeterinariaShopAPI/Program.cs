global using Microsoft.EntityFrameworkCore;
global using VeterinariaShopAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using VeterinariaShopAPI.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("TPIApiBearerAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Acá pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "TPIApiBearerAuth" }
                }, new List<string>() }
    });
});


builder.Services.AddDbContext<DataContext>(
                dbContextOptions => dbContextOptions.UseSqlite(
                        builder.Configuration["ConnectionStrings:VeterinariaDBConnectionString"]))
                .AddIdentityCore<User>().AddDefaultTokenProviders()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<DataContext>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddHttpContextAccessor();




builder.Services.AddIdentityCore<Admin>().AddEntityFrameworkStores<DataContext>();
builder.Services.AddIdentityCore<Client>().AddEntityFrameworkStores<DataContext>();

builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("NewPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await SeedData();

app.Run();

async Task SeedData()
{
    var scopeFactory = app!.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    context.Database.EnsureCreated();

    if (userManager.Users.Count() < 4)
    {
        logger.LogInformation("Creando usuario de prueba");

        if (roleManager.Roles.Count() < 3)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = "Administrador"
            });
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = "Cliente"
            });
        }
        var admin = new Admin("Juan", "Pérez")
        {
            Email = "administrador@email.com",
            UserName = "admin",
        };

        var cliente1 = new Client("José", "Gonzáles")
        {
            Email = "cliente1@email.com",
        };
        var cliente2 = new Client("María", "López")
        {
            Email = "cliente2@email.com",
        };
        var cliente3 = new Client("Juan", "Hidalgo")
        {
            Email = "juan@hidalgo.com"
        };



        await userManager.CreateAsync(admin, "123qwe");
        await userManager.AddToRoleAsync(admin, "Administrador");
        await userManager.CreateAsync(cliente1, "123qwe");
        await userManager.AddToRoleAsync(cliente1, "Cliente");
        await userManager.CreateAsync(cliente2, "123qwe");
        await userManager.AddToRoleAsync(cliente2, "Cliente");
        await userManager.CreateAsync(cliente3, "123qwe");
        await userManager.AddToRoleAsync(cliente3, "Cliente");

    }
}