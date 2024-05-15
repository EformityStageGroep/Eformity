using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using Organizer.Authorization;
using Organizer.Contexts;
using Organizer.Middleware;
using Organizer.Repositories;
using Organizer.Services;

var builder = WebApplication.CreateBuilder(args);

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();

// Ignore cookies on start
builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options => options.Events = new RejectSessionCookieWhenAccountNotInCacheEvents());


// Add DbContext
builder.Services.AddDbContext<OrganizerContext>();
builder.Services.AddDbContext<TenantDbContext>();
builder.Services.AddDbContext<UserDbContext>();

// Add repositories and services
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IGraphClientService, GraphClientService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ITeamsRepository, TeamsRepository>();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserResolver>();

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateTeamPolicy", policy =>
        policy.Requirements.Add(new CreateTeamRequirement()));
});

// Register the policy handler
builder.Services.AddScoped<IAuthorizationHandler, CreateTeamHandler>();

// Add MVC services
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Your existing middleware registrations
app.UseMiddleware<TenantResolver>();
app.UseMiddleware<UserResolver>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=CompanyAdminDashboard}/{id?}");

app.MapRazorPages();
app.MapControllers();
app.Run();

internal class RejectSessionCookieWhenAccountNotInCacheEvents : CookieAuthenticationEvents
{
    public async override Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        try
        {
            var tokenAcquisition = context.HttpContext.RequestServices.GetRequiredService<ITokenAcquisition>();
            string token = await tokenAcquisition.GetAccessTokenForUserAsync(
                scopes: new[] { "profile" },
                user: context.Principal);
        }
        catch (MicrosoftIdentityWebChallengeUserException ex) when (AccountDoesNotExitInTokenCache(ex))
        {
            context.RejectPrincipal();
        }
    }

    /// <summary>
    /// Is the exception thrown because there is no account in the token cache?
    /// </summary>
    /// <param name="ex">Exception thrown by <see cref="ITokenAcquisition"/>.GetTokenForXX methods.</param>
    /// <returns>A boolean telling if the exception was about not having an account in the cache</returns>
    private static bool AccountDoesNotExitInTokenCache(MicrosoftIdentityWebChallengeUserException ex)
    {
        return ex.InnerException is MsalUiRequiredException && (ex.InnerException as MsalUiRequiredException).ErrorCode == "user_null";
    }
}
