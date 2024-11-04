using HealthCareAppointments.DBContext;
using HealthCareAppointments.Entities;
using HealthCareAppointments.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("HealthCareDbContextConnection") ?? throw new InvalidOperationException("Connection string 'HealthCareDbContextConnection' not found.");

//Register Services
builder.Services.AddScoped<IAppointment, AppointmentOperations>();
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; //This is for ignore any refrence cycle while searlizanation.
});

//below is for register DbContext service and get connection string.
builder.Services.AddDbContext<HealthCareContext>(options =>
{
    options.UseSqlServer(
            builder.Configuration["ConnectionStrings:HealthCareDbContextConnection"]
        );
});

builder.Services.AddIdentity<ApplicationUsers, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<HealthCareContext>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
   {
       options.RequireHttpsMetadata = false;
       options.SaveToken = true;
       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
       {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
           ValidateAudience = false,
           ValidateIssuer = false,
           //ValidateIssuer = true,
           //ValidateAudience = true,
           //ValidAudience = builder.Configuration["Jwt:Audience"],
           //ValidIssuer = builder.Configuration["Jwt:Issuer"],
           //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
       };
   }
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

////Add middleware componnents.
app.UseAuthentication(); //This is for identity authentication
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

DBInitializer.Seed(app);

app.Run();
