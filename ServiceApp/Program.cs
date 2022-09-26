using System.Net;
using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise.RichTextEdit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using ServiceApp.ApiClient;
using ServiceApp.Authentication;
using ServiceApp.Middleware;
using ServiceApp.Services;
using ServiceApp.Services.Implementation;
using ServiceApp.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons()
    .AddBlazoriseRichTextEdit(options => {});

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IApiClient, ApiClient>(s => new ApiClient(new HttpClient
{
    BaseAddress = new Uri(builder.Configuration.GetConnectionString("ServiceApplicationAPI"))
}));

builder.Services.AddScoped<IApiClientBase, ApiClient>(
    x => (ApiClient)x.GetService<IApiClient>());


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<PageHistoryState>();


var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseMiddleware<ErrorHandlerMiddleware>();
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();