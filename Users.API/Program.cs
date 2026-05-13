using CORE.APP.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Users.APP.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString(nameof(UsersDb));
builder.Services.AddDbContext<DbContext, UsersDb>(options => options.UseSqlite(connectionString));

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UsersDb).Assembly));
builder.Services.AddSingleton<ITokenAuthService, TokenAuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecurityKey"] ?? string.Empty)),
            ValidIssuer = builder.Configuration["Issuer"],
            ValidAudience = builder.Configuration["Audience"],
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Users API",
        Version = "v1"
    });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token as: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DbContext>();

    var group = db.Set<Group>().FirstOrDefault(g => g.Title == "General");
    if (group is null)
    {
        group = new Group { Title = "General" };
        db.Set<Group>().Add(group);
        db.SaveChanges();
    }

    var adminRole = db.Set<Role>().FirstOrDefault(r => r.Name == "Admin");
    if (adminRole is null)
    {
        adminRole = new Role { Name = "Admin" };
        db.Set<Role>().Add(adminRole);
    }

    var userRole = db.Set<Role>().FirstOrDefault(r => r.Name == "User");
    if (userRole is null)
    {
        userRole = new Role { Name = "User" };
        db.Set<Role>().Add(userRole);
    }

    db.SaveChanges();

    var adminUser = db.Set<User>()
        .Include(u => u.UserRoles)
        .FirstOrDefault(u => u.UserName == "admin");
    if (adminUser is null)
    {
        adminUser = new User
        {
            UserName = "admin",
            Password = "Admin123!",
            FirstName = "System",
            LastName = "Admin",
            Gender = Genders.Man,
            RegistrationDate = DateTime.Now,
            Score = 10,
            IsActive = true,
            Address = "Baku",
            GroupId = group.Id
        };
        db.Set<User>().Add(adminUser);
        db.SaveChanges();
    }
    else
    {
        adminUser.Password = "Admin123!";
        adminUser.IsActive = true;
        adminUser.GroupId = group.Id;
        adminUser.FirstName ??= "System";
        adminUser.LastName ??= "Admin";
        adminUser.Address ??= "Baku";
        if (adminUser.RegistrationDate == default)
            adminUser.RegistrationDate = DateTime.Now;
        db.Set<User>().Update(adminUser);
    }

    if (!db.Set<UserRole>().Any(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id))
        db.Set<UserRole>().Add(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });
    if (!db.Set<UserRole>().Any(ur => ur.UserId == adminUser.Id && ur.RoleId == userRole.Id))
        db.Set<UserRole>().Add(new UserRole { UserId = adminUser.Id, RoleId = userRole.Id });

    var memberUser = db.Set<User>()
        .Include(u => u.UserRoles)
        .FirstOrDefault(u => u.UserName == "member");
    if (memberUser is null)
    {
        memberUser = new User
        {
            UserName = "member",
            Password = "Member123!",
            FirstName = "Regular",
            LastName = "Member",
            Gender = Genders.Woman,
            RegistrationDate = DateTime.Now,
            Score = 7.5m,
            IsActive = true,
            Address = "Baku",
            GroupId = group.Id
        };
        db.Set<User>().Add(memberUser);
        db.SaveChanges();
    }
    else
    {
        memberUser.Password = "Member123!";
        memberUser.IsActive = true;
        memberUser.GroupId = group.Id;
        memberUser.FirstName ??= "Regular";
        memberUser.LastName ??= "Member";
        memberUser.Address ??= "Baku";
        if (memberUser.RegistrationDate == default)
            memberUser.RegistrationDate = DateTime.Now;
        db.Set<User>().Update(memberUser);
    }

    if (!db.Set<UserRole>().Any(ur => ur.UserId == memberUser.Id && ur.RoleId == userRole.Id))
        db.Set<UserRole>().Add(new UserRole { UserId = memberUser.Id, RoleId = userRole.Id });

    db.SaveChanges();
}

app.Run();
