using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ServiceApp.API.DAL;
using ServiceApp.API.Models;
using ServiceApp.API.Services;
using ServiceApp.API.Services.Abstract;
using ServiceApp.API.Utils;
using ServiceApp.API.Middleware;
using ServiceApp.API.Factories;
using ServiceApp.API.DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));

});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();

});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew =TimeSpan.Zero
                    };
                });


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton(AutoMapperConfig.Initialize());
builder.Services.AddSingleton<IFactory, EmailAccountFactory>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJiraService, JiraService>();

builder.Services.AddHttpClient("JiraHttpClient", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetConnectionString("JiraUrl"));
    httpClient.Timeout = new TimeSpan(0, 0, 30);
    httpClient.DefaultRequestHeaders.Clear();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ApiCorsPolicy", builder =>
        {
            builder.WithOrigins("https://localhost:7104/");
            builder.WithOrigins("https://localhost:44334/")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); ;
        });
});
builder.Services.AddSwaggerDocument();

var app = builder.Build();

app.UseSwagger(o => o.SerializeAsV2 = true);
app.UseSwaggerUI();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
    {
        context.Request.Path = "/index.html"; await next();
    }
});

app.UseCors("ApiCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();



app.MapControllers();

app.Run();
