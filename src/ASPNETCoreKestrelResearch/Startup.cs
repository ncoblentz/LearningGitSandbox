using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ASPNETCoreKestrelResearch.Data;
using ASPNETCoreKestrelResearch.Models;
using ASPNETCoreKestrelResearch.Services;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ASPNETCoreKestrelResearch.Security;
using Microsoft.AspNetCore.Http;
using NetEscapades.AspNetCore.SecurityHeaders;
using NetEscapades.AspNetCore.SecurityHeaders.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ASPNETCoreKestrelResearch
{
    public class Startup
    {
        private string ContentRootPath = string.Empty;

        public Startup(IHostingEnvironment env)
        {
            this.ContentRootPath = env.ContentRootPath;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));            
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Path.Combine("Filename ="+this.ContentRootPath, "aspnetcorekestrel.db")));            

            //todo: http://andrewlock.net/adding-default-security-headers-in-asp-net-core            
            services.AddCustomHeaders();                        

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddPasswordValidator<CustomPasswordValidator<ApplicationUser>>();

            services.Configure<IdentityOptions>(options =>
            {
                options.SecurityStampValidationInterval = TimeSpan.FromMinutes(1);
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.Cookies.ApplicationCookie.CookieHttpOnly = true;
                //options.Cookies.ApplicationCookie.CookieSecure = CookieSecurePolicy.Always;
                options.Cookies.ApplicationCookie.SlidingExpiration = true;                
                options.SecurityStampValidationInterval = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
            });

            services.Configure<PasswordHasherOptions>(options =>
            {                
                options.IterationCount = 10000;

            });

            services.Configure<MvcOptions>(options =>
            {
                options.CacheProfiles.Add("DefaultNoCacheProfile", new CacheProfile
                {
                    NoStore = true,
                    Location = ResponseCacheLocation.None
                });
                options.Filters.Add(new ResponseCacheAttribute
                {
                    CacheProfileName = "DefaultNoCacheProfile"                    
                });
            });            
            services.AddMvc();

            //services.Configure<MvcOptions>(options => options.Filters.Add(new RequireHttpsAttribute()));
            //services.Configure<MvcOptions>(options => options.SslPort = 443);

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.Use(async (context, next) =>
            {                
                context.Request.Scheme = "https";
                HostString httpsHostString = new HostString("loadbalancer", 443);
                context.Request.Host = httpsHostString;             
                await next.Invoke();                
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseCustomHeadersMiddleware(new HeaderPolicyCollection()
                .AddContentTypeOptionsNoSniff()
                .AddFrameOptionsDeny()
                //.AddStrictTransportSecurityMaxAge()
                .AddXssProtectionBlock()
            //.AddCustomHeader("Content-Security-Policy", "somevaluehere")
            //.AddCustomHeader("X-Content-Security-Policy", "somevaluehere")
            //.AddCustomHeader("X-Webkit-CSP", "somevaluehere")                
            );

            app.UseStaticFiles();

            app.UseIdentity();
            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715                            

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "oidccookies",                
                Events = new MyCookieAuthenticationEvents(app.ApplicationServices.GetService<ApplicationDbContext>(), loggerFactory)                
            });

            var oidcOptions = new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "oidccookies",
                //AutomaticChallenge = true,
                Authority = "https://loadbalancer:1943/",
                ClientId = "mvc.hybrid",
                ClientSecret = "secret",
                ResponseType = "code id_token",
                SaveTokens = true,
                GetClaimsFromUserInfoEndpoint = true,
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",                    
                }
            };

            oidcOptions.Scope.Clear();
            oidcOptions.Scope.Add("openid");
            oidcOptions.Scope.Add("profile");
            oidcOptions.Scope.Add("mvcaccess");

            app.UseOpenIdConnectAuthentication(oidcOptions);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",                    
                    template: "{controller=Home}/{action=Index}/{id?}");                
            });            
        }
    }

    class OpenIdConnectEventsRedirectUriFix : OpenIdConnectEvents
    {
        public override Task RedirectToIdentityProvider(RedirectContext context)
        {
            UriBuilder redirectUriBuilder = new UriBuilder(context.ProtocolMessage.RedirectUri);
            redirectUriBuilder.Scheme = "https";
            redirectUriBuilder.Port = 443;                        
            context.ProtocolMessage.RedirectUri = redirectUriBuilder.ToString();
            return base.RedirectToIdentityProvider(context);
        }
        public override Task RedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            UriBuilder redirectUriBuilder = new UriBuilder(context.ProtocolMessage.PostLogoutRedirectUri);
            redirectUriBuilder.Scheme = "https";
            redirectUriBuilder.Port = 443;
            context.ProtocolMessage.PostLogoutRedirectUri = redirectUriBuilder.ToString();
            return base.RedirectToIdentityProviderForSignOut(context);
        }        
    }

    class MyCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILoggerFactory _loggerFactory;
        public MyCookieAuthenticationEvents(ApplicationDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _loggerFactory = loggerFactory;
        }        
        public override async Task SignedIn(CookieSignedInContext context)
        {
            var logger = _loggerFactory.CreateLogger("MyCookieAuthenticationEvents");
            logger.LogInformation("Entering SignedIn Event");
            string name = context.Principal.Identity.Name;
            Claim subClaim = context.Principal.Claims.First(c => c.Type == "sub");
            OIDCUser user = await _dbContext.OIDCUsers.FirstOrDefaultAsync(u => u.Subject == subClaim.Value);            
            if(user==null)
            {
                logger.LogInformation("Creating user");
                _dbContext.OIDCUsers.Add(new OIDCUser()
                {
                    Name = name,
                    Subject = subClaim.Value
                });
                await _dbContext.SaveChangesAsync();
            }
            else
                logger.LogInformation("User already created");
        }
    }
}
